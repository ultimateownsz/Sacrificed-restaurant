// This class handles all the logic for the themes, adding, deleting, and eventually activating themes

static class ThemeMenuManager
{
    public static Dictionary<(int year, int month), ThemeMenuModel> ThemeSchedule = new();
    
    public static List<(int year, int month, string themeName)> GetMonthlyDisplay()
    {
        var displayData = new List<(int year, int month, string themeName)>();

        int currentYear = DateTime.Now.Year;
        int startMonth = DateTime.Now.Month;

        for (int i = 0; i < 12; i++)
        {
            var key = (currentYear, startMonth);
            if (!ThemeSchedule.TryGetValue(key, out var theme))
            {
                theme = new ThemeMenuModel
                {
                    ScheduledYear = currentYear,
                    ScheduledMonth = startMonth,
                    ThemeName = "Not scheduled"
                };
            }
            
            displayData.Add((theme.ScheduledYear, theme.ScheduledMonth ?? 0, theme.ThemeName));

            startMonth++;
            if (startMonth > 12)
            {
                startMonth = 1;
                currentYear++;
            }
        }
        return displayData;
    }

    public static void UpdateThemeSchedule(int startYear, int startMonth, List<ThemeMenuModel> themes)
    {
        foreach (var theme in themes)
        {
            var key = (theme.ScheduledYear, theme.ScheduledMonth ?? 0);
            ThemeSchedule[key] = theme;  // add or update
        }

        for (int i = 0; i < 12; i++)
        {
            var key = (startYear, startMonth);
            if (!ThemeSchedule.ContainsKey(key))
            {
                ThemeSchedule[key] = new ThemeMenuModel
                {
                    ScheduledYear = startYear,
                    ScheduledMonth = startMonth,
                    ThemeName = "Not scheduled"
                };
            }

            startMonth++;
            if (startMonth > 12)
            {
                startMonth = 1;
                startYear++;
            }
        }
    }

    public static ThemeMenuModel? GetThemeByYearAndMonth((int year, int month) key)
    {
        if (ThemeSchedule.TryGetValue(key, out var theme))
        {
            return theme;
        }
        return null;  // Return null if no theme exists for the given year and month
    }

    public static string GetMonthName(int? month)
    {
        return month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => "Invalid month"
        };
    }

    // return falsew hether the failure was due to a duplicate theme name.
    public static bool AddOrUpdateTheme(ThemeMenuModel theme, int scheduledYear, int scheduledMonth, out bool isDuplicate)
    {
        if (theme == null) throw new ArgumentNullException(nameof(theme));

        // check if the theme name already exists
        if (ThemeSchedule.Values.Any(t => t.ThemeName.Equals(theme.ThemeName, StringComparison.OrdinalIgnoreCase)))
        {
            isDuplicate = true;
            return false;  // return false due to duplicate theme
        }

        isDuplicate = false;  // no duplicate found
        
        var key = (theme.ScheduledYear, theme.ScheduledMonth ?? 0);
        if (ThemeSchedule.ContainsKey(key))
        {
            if (!ThemesAccess.UpdateTheme(theme)) return false;
        }
        else
        {
            if (!ThemesAccess.Write(theme)) return false;
        }

        ThemeSchedule[key] = theme;
        return true;
    }

    public static bool IsFutureDate(int year, int month)
    {
        DateTime currentDate = DateTime.Now;
        return year > currentDate.Year || (year == currentDate.Year && month >= currentDate.Month);
    }

    public static bool DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null) throw new ArgumentNullException(nameof(theme));

        var key = (theme.ScheduledYear, theme.ScheduledMonth ?? 0);
        if (ThemeSchedule.Remove(key))
        {
            ThemesAccess.DeleteTheme(theme.MenuId);
            return true;
        }
        return false;
    }
}
