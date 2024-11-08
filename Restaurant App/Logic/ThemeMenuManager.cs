// this class handles all the logic for the themes, adding, deleting, and eventually activating themes

static class ThemeMenuManager
{
    // temporary theme method
    public static List<ThemeMenuModel> GetScheduledThemes()
    {
        var themes = ThemesAccess.GetAllThemes().ToList();

        // test purposes
        int startYear = DateTime.Now.Year;
        int startMonth = DateTime.Now.Month;

        for (int i = 0; i < themes.Count; i++)
        {
            themes[i].ScheduledYear = startYear;
            themes[i].ScheduledMonth = (startMonth + i) % 12;
            if (themes[i].ScheduledMonth < startMonth)
            {
                themes[i].ScheduledYear++;
            }
        }
        return themes;
    }
    
    
    // // retrieve all themes
    // public static List<ThemeMenuModel> ViewThemes()
    // {
    //     // view all themes in chronological order
    //     return ThemesAccess.GetAllThemes()
    //         .OrderBy(t => t.ScheduledYear)
    //         .ThenBy(t => t.ScheduledMonth)
    //         .ToList();
    // }

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
        theme.ScheduledYear = scheduledYear;
        theme.ScheduledMonth = scheduledMonth;
        ThemesAccess.AddTheme(theme);
    }


    // edit an existing theme (no calendar constraints)
    public static bool UpdateTheme(ThemeMenuModel newTheme)
    {
        return ThemesAccess.UpdateTheme(newTheme);
    }

    // edit an existing theme (with calendar constraints)
    public static bool UpdateTheme(ThemeMenuModel newTheme, int scheduledYear, int scheduledMonth)
    {
        if (IsFutureDate(scheduledYear, scheduledMonth))
        {
            return ThemesAccess.UpdateTheme(newTheme);
        }
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
            ThemesAccess.DeleteTheme(theme.MenuId);
        }
        return true;
    }

}