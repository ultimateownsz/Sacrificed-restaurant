// this class handles all the logic for the themes, adding, deleting, and eventually activating themes

static class ThemeMenuManager
{
    
    public static List<(int year, int month, string themeName)> GetMonthlyDisplay(int startYear)
    {
        var displayData = new List<(int year, int month, string themeName)>();

        // check if scheduled themes dict is updated with themes
        var themes = GetScheduledThemes();  // retrieve themes from database
        UpdateThemeSchedule(startYear, 1, themes);  // populate ThemeScheduledByYear

        foreach (var yearEntry in ThemeScheduledByYear)
        {
            int year = yearEntry.Key;

            for (int month = 1; month <= 12; month++)
            {
                // check if there's a theme scheduled for this month
                var themeForMonth = yearEntry.Value.FirstOrDefault(t => t.ScheduledMonth == month);
                string theme = themeForMonth?.ThemeName ?? "Not scheduled";

                // add each month, with either the theme name or "Not scheduled"
                displayData.Add((year, month, theme));
            }
        }
        return displayData;
    }
    
    
    // temporary theme method
    public static Dictionary<int, List<ThemeMenuModel>> ThemeScheduledByYear = new();
    public static List<ThemeMenuModel> GetScheduledThemes()
    {
        // retrieve all themes from the database without year and month
        var themes = ThemesAccess.GetAllThemes().ToList();

        // use a dict for scheduling information for demo purposes
        int startYear = DateTime.Now.Year;
        int startMonth = DateTime.Now.Month;

        UpdateThemeSchedule(startYear, startMonth, themes);

        return themes;
    }

    // update scheduling information in dict
    private static void UpdateThemeSchedule(int startYear, int startMonth, List<ThemeMenuModel> themes)
    {
        ThemeScheduledByYear.Clear();
        int year = startYear;
        int month = startMonth;

        foreach (var theme in themes)
        {
            // make sure dict has current year key
            if (!ThemeScheduledByYear.ContainsKey(year))
            {
                ThemeScheduledByYear[year] = new List<ThemeMenuModel>();
            }

            // set theme's scheduled month and year for display purposes
            theme.ScheduledYear = year;
            theme.ScheduledMonth = month;

            // add the theme do the dict for the specific year
            ThemeScheduledByYear[year].Add(theme);

            // move to the next month
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

    // checks if a theme can be added for a future month
    public static bool ValidateToAddTheme(ThemeMenuModel theme, int scheduledYear, int scheduledMonth)
    {
        if (!IsFutureDate(scheduledYear, scheduledMonth))
        {
            return false;
        }
        
        if (ThemesAccess.GetAllThemes().Any(t => t.MenuId == theme.MenuId))
        {
            return false;
        }
        return true;
    }

    // add a theme to the database if call of 'CanAddTheme' returns true
    public static void AddTheme(ThemeMenuModel theme, int scheduledYear, int scheduledMonth)
    {
        
        // prevent adding a theme if it's already scheduled for month and year
        if (ThemeScheduledByYear.ContainsKey(scheduledYear) &&
            ThemeScheduledByYear[scheduledYear].Any(t => t.ScheduledMonth == scheduledMonth))
        {
            return;
        }

        // make sure if the date is in the future
        if (!IsFutureDate(scheduledYear, scheduledMonth))
        {
            return;
        }
        
        // store scheduling information temporarily in the dict -> later in the database
        if (!ThemeScheduledByYear.ContainsKey(scheduledYear))
        {
            ThemeScheduledByYear[scheduledYear] = new List<ThemeMenuModel>();
        }
        
        theme.ScheduledYear = scheduledYear;
        theme.ScheduledMonth = scheduledMonth;
        ThemeScheduledByYear[scheduledYear].Add(theme);

        // only save theme name and menu id to the database
        ThemesAccess.AddTheme(new ThemeMenuModel { ThemeName = theme.ThemeName, MenuId = theme.MenuId });

    }

    // edit an existing theme (with calendar constraints)
    public static bool UpdateTheme(ThemeMenuModel newTheme, int scheduledYear, int scheduledMonth)
    {
        // check if the date is in the future
        if (IsFutureDate(scheduledYear, scheduledMonth))
        {
            // look for an existing theme in the temporary scheduling dict
            if (ThemeScheduledByYear.ContainsKey(scheduledYear))
            {
                var existingTheme = ThemeScheduledByYear[scheduledYear]
                    .FirstOrDefault(t => t.ScheduledMonth == scheduledMonth);
            
                if (existingTheme != null)
                {
                    existingTheme.ThemeName = newTheme.ThemeName;
                    newTheme.MenuId = existingTheme.MenuId;
                }
                // update in the database using the correct MenuId
            }
            // ensure the theme exists and update it
            return ThemesAccess.UpdateTheme(newTheme);
        }
        // theme not found or date invalid
        return false;
    }

    public static bool IsFutureDate(int year, int month)
    {
        DateTime currentDate = DateTime.Now;
        DateTime targetDate = new DateTime(year, month, 1);
        return targetDate > currentDate;
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
        else
        {
            // remove from the dict
            if (ThemeScheduledByYear.ContainsKey(theme.ScheduledYear))
            {
                ThemeScheduledByYear[theme.ScheduledYear].RemoveAll(t => t.MenuId == theme.MenuId);
            }
            ThemesAccess.DeleteTheme(theme.MenuId);  // delete only from the database
        }
        return true;
    }

}