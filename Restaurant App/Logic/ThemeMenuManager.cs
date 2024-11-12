// this class handles all the logic for the themes, adding, deleting, and eventually activating themes

static class ThemeMenuManager
{
    public static List<(int year, int month, string themeName)> GetMonthlyDisplay()
    {
        var displayData = new List<(int year, int month, string themeName)>();

        int startMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;
        
        // check if scheduled themes dict is updated with themes
        var themes = GetScheduledThemes();  // retrieve themes from database
        UpdateThemeSchedule(currentYear, startMonth, themes);  // populate ThemeScheduledByYear

        
        int month = startMonth;
        int year = currentYear;

        for (int i = 0; i < 12; i++)
        {
            // check if there's a theme scheduled for this month
            var themeForMonth = ThemeScheduledByYear.ContainsKey(year)
                ? ThemeScheduledByYear[year].FirstOrDefault(t => t.ScheduledMonth == month)
                : null;
            
            string themeName = themeForMonth?.ThemeName ?? "Not scheduled";

            // add each month, with either the theme name or "Not scheduled"
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
        int themeIndex = 0;  // track the index of the theme we're assigning

        for (int i = 0; i < 12; i++)
        {
            // make sure dict has current year key
            if (!ThemeScheduledByYear.ContainsKey(year))
            {
                ThemeScheduledByYear[year] = new List<ThemeMenuModel>();
            }

            ThemeMenuModel theme;

            // set theme's scheduled month and year for display purposes
            if (themeIndex < themes.Count)
            {
                theme = themes[themeIndex];
                theme.ScheduledYear = year;
                theme.ScheduledMonth = month;
                themeIndex++;  // move to the next theme
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

            // add the theme do the dict for the specific year
            ThemeScheduledByYear[year].Add(theme);

            // move to the next month and increment themeIndex
            month++;

            // Move to the next month, loop over to the next year if needed
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
        // make sure if the date is in the future
        if (!IsFutureDate(scheduledYear, scheduledMonth))
        {
            return;
        }
        
        // prevent adding a theme if it's already scheduled for month and year
        if (ThemeScheduledByYear.ContainsKey(scheduledYear) &&
            ThemeScheduledByYear[scheduledYear].Any(t => t.ScheduledMonth == scheduledMonth))
        {
            return;
        }

        if (!ValidateToAddTheme(theme, scheduledYear, scheduledMonth))
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
        // check if the date is in the future or the current month
        if (!IsFutureDate(scheduledYear, scheduledMonth))
        {
            return false;
        }

        // look for an existing theme for year and month
        if (!ThemeScheduledByYear.ContainsKey(scheduledYear) ||
            !ThemeScheduledByYear[scheduledYear].Any(t => t.ScheduledMonth == scheduledMonth))
        {
            return false;
        }
            
        // find existing theme and update data
        var existingTheme = ThemeScheduledByYear[scheduledYear]
                .FirstOrDefault(t => t.ScheduledMonth == scheduledMonth);
        
        if (existingTheme != null)
        {
            existingTheme.ThemeName = newTheme.ThemeName;
            newTheme.MenuId = existingTheme.MenuId;

            // update in the database and check if update was succesful
            bool updateSuccesful = ThemesAccess.UpdateTheme(newTheme);
            if (updateSuccesful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public static bool IsFutureDate(int year, int month)
    {
        DateTime currentDate = DateTime.Now;
        // DateTime targetDate = new DateTime(year, month, 1);
        
        if (year > currentDate.Year || (year == currentDate.Year && month >= currentDate.Month))
        {
            return true;
        } 
        return false;
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