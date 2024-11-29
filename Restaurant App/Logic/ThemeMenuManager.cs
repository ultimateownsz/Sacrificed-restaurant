// This class handles all the logic for the themes, adding, deleting, and eventually activating themes

using Project;

static class ThemeMenuManager
{
    public static Dictionary<int, ThemeModel> ThemeSchedule = new();
    
    public static List<(int month, string themeName)> GetMonthlyDisplay()
    {
        var displayData = new List<(int month, string themeName)>();
        int startMonth = DateTime.Now.Month;

        for (int i = 0; i < 12; i++)
        {
            var key = startMonth;
            if (!ThemeSchedule.TryGetValue(key, out var theme))
            {
                theme = new ThemeModel()
                {
                    Month = startMonth,
                    Name = "Not scheduled"
                };
            }
            
            displayData.Add((theme.Month ?? 0, theme.Name));

            startMonth++;
            if (startMonth > 12)
            {
                startMonth = 1;
            }
        }
        return displayData;
    }

    public static void UpdateThemeSchedule(int startYear, int startMonth, List<ThemeModel> themes)
    {
        foreach (var theme in themes)
        {
            var key = theme.Month ?? 0;
            ThemeSchedule[key] = theme;  // add or update
        }

        for (int i = 0; i < 12; i++)
        {
            var key = startMonth;
            if (!ThemeSchedule.ContainsKey(key))
            {
                ThemeSchedule[key] = new ThemeModel()
                {
                    Month = startMonth,
                    Name = "Not scheduled"
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

    public static ThemeModel? GetThemeByYearAndMonth(int key)
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
    public static bool AddOrUpdateTheme(ThemeModel theme, int scheduledYear, int scheduledMonth, out bool isDuplicate)
    {
        if (theme == null) throw new ArgumentNullException(nameof(theme));

        // check if the theme name already exists
        if (ThemeSchedule.Values.Any(t => t.Name.Equals(theme.Name, StringComparison.OrdinalIgnoreCase)))
        {
            isDuplicate = true;
            return false;  // return false due to duplicate theme
        }

        isDuplicate = false;  // no duplicate found
        
        var key = (theme.Month ?? 0);
        if (ThemeSchedule.ContainsKey(key))
        {
            if (!Access.Themes.Update(theme)) return false;
        }
        else
        {
            if (!Access.Themes.Write(theme)) return false;
        }

        ThemeSchedule[key] = theme;
        return true;
    }

    public static bool IsFutureDate(int year, int month)
    {
        DateTime currentDate = DateTime.Now;
        return year > currentDate.Year || (year == currentDate.Year && month >= currentDate.Month);
    }

    public static bool DeleteTheme(ThemeModel theme)
    {
        if (theme == null) throw new ArgumentNullException(nameof(theme));

        var key = theme.Month ?? 0;
        if (ThemeSchedule.Remove(key))
        {
            Access.Themes.Delete(theme.ID);
            return true;
        }
        return false;
    }
}
