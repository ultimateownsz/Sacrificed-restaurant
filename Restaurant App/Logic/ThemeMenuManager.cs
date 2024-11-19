// This class handles all the logic for the themes, adding, deleting, and eventually activating themes
static class ThemeMenuManager
{
    public static List<(int year, int month, string themeName)> GetMonthlyDisplay()
    {
        var displayData = new List<(int year, int month, string themeName)>();

        int startMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;

        // Check if scheduled themes dict is updated with themes
        var themes = GetScheduledThemes(); // Retrieve themes from database
        UpdateThemeSchedule(currentYear, startMonth, themes); // Populate ThemeScheduledByYear

        int month = startMonth;
        int year = currentYear;

        for (int i = 0; i < 12; i++)
        {
            // Check if there's a theme scheduled for this month
            var themeForMonth = ThemeScheduledByYear.ContainsKey(year)
                ? ThemeScheduledByYear[year].FirstOrDefault(t => t.ScheduledMonth == month)
                : null;

            string themeName = themeForMonth?.ThemeName ?? "Not scheduled";

            // Add each month, with either the theme name or "Not scheduled"
            displayData.Add((year, month, themeName));

            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
        }

        return displayData;
    }

    // Temporary theme method
    public static Dictionary<int, List<ThemeMenuModel>> ThemeScheduledByYear = new();

    public static List<ThemeMenuModel> GetScheduledThemes()
    {
        // Retrieve all themes from the database without year and month
        var themes = ThemesAccess.GetAllThemes().ToList();

        // Use a dict for scheduling information for demo purposes
        int startYear = DateTime.Now.Year;
        int startMonth = DateTime.Now.Month;

        UpdateThemeSchedule(startYear, startMonth, themes);

        return themes;
    }

    // Update scheduling information in dict
    private static void UpdateThemeSchedule(int startYear, int startMonth, List<ThemeMenuModel> themes)
    {
        ThemeScheduledByYear.Clear();
        int year = startYear;
        int month = startMonth;
        int themeIndex = 0; // Track the index of the theme we're assigning

        for (int i = 0; i < 12; i++)
        {
            // Make sure dict has current year key
            if (!ThemeScheduledByYear.ContainsKey(year))
            {
                ThemeScheduledByYear[year] = new List<ThemeMenuModel>();
            }

            ThemeMenuModel theme;

            // Set theme's scheduled month and year for display purposes
            if (themeIndex < themes.Count)
            {
                theme = themes[themeIndex];
                theme.ScheduledYear = year;
                theme.ScheduledMonth = month;
                themeIndex++; // Move to the next theme
            }
            else
            {
                theme = new ThemeMenuModel
                {
                    ScheduledYear = year,
                    ScheduledMonth = month,
                    ThemeName = "Not scheduled"
                };
            }

            // Add the theme to the dict for the specific year
            ThemeScheduledByYear[year].Add(theme);

            // Move to the next month
            month++;
            if (month > 12)
            {
                month = 1;
                year++;
            }
        }
    }

    public static string GetMonthName(int month)
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

    public static bool AddOrUpdateTheme(ThemeMenuModel theme, int scheduledYear, int scheduledMonth)
    {
        try
        {
            if (theme == null)
            {
                throw new ArgumentNullException(nameof(theme));
            }

            // Set the year and month
            theme.ScheduledYear = scheduledYear;
            theme.ScheduledMonth = scheduledMonth;

            // Check if a theme already exists in the database
            var existingTheme = ThemesAccess.GetById(theme.MenuId);

            if (existingTheme != null)
            {
                // Update the existing theme
                if (!ThemesAccess.UpdateTheme(theme))
                {
                    return false;
                }
            }
            else
            {
                // Add a new theme
                if (!ThemesAccess.Write(theme))
                {
                    return false;
                }
            }

            // Synchronize the dictionary with the database
            UpdateThemeSchedule(DateTime.Now.Year, DateTime.Now.Month, ThemesAccess.GetAllThemes().ToList());
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static bool IsFutureDate(int year, int month)
    {
        DateTime currentDate = DateTime.Now;
        return year > currentDate.Year || (year == currentDate.Year && month >= currentDate.Month);
    }

    public static bool DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemesAccess.GetById(theme.MenuId) == null)
        {
            return false;
        }

        if (ThemeScheduledByYear.ContainsKey(theme.ScheduledYear))
        {
            ThemeScheduledByYear[theme.ScheduledYear].RemoveAll(t => t.MenuId == theme.MenuId);
        }
        ThemesAccess.DeleteTheme(theme.MenuId); // Delete from the database

        return true;
    }
}
