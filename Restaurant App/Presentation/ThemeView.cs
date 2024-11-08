static class ThemeView
{
    // show all planned themes
    public static void DisplayAllThemes()
    { 
        Console.Clear();
        var themes = ThemeMenuManager.GetScheduledThemes();
        
        Console.WriteLine("Scheduled themes by month: ");
        if (!themes.Any())
        {
            Console.WriteLine("No themes scheduled.");
            return;
        }

        for (int month = 1; month <= 12; month++)
        {
            string monthName = ThemeMenuManager.GetMonthName(month);

            var themeForMonth = themes.FirstOrDefault(t => t.ScheduledMonth == month);

            if (themeForMonth != null)
            {
                Console.WriteLine($"The theme of the month {monthName} is '{themeForMonth.ThemeName}'.");
                break;
            }
            else
            {
                Console.WriteLine($"The theme of {monthName} is not scheduled.");
            }
        }
    }

    public static void AddTheme()
    {
        Console.Clear();

        int month;
        int year;

        while (true)
        {
            month = InputValidator.GetValidMonth("Enter scheduled month (1-12) ");
            year = InputValidator.GetValidYear("Enter scheduled year (YYYY) ", DateTime.Now.Year);

            if (ThemeMenuManager.IsFutureDate(year, month))
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid date. Please enter a month and year in the future.");
            }
        }

        while (true)
        {
            string themeName = InputValidator.GetValidString("Enter theme (name): ");
            var theme = new ThemeMenuModel { ThemeName = themeName };
            
            if (ThemeMenuManager.ValidateToAddTheme(theme, year, month) == true)
            {
                ThemeMenuManager.AddTheme(theme, year, month);
                Console.WriteLine($"Theme '{theme.ThemeName}', scheduled for {ThemeMenuManager.GetMonthName(theme.ScheduledMonth)} {theme.ScheduledYear}.");
                break;
            }
            else
            {
                Console.WriteLine("Failed to add theme. Make sure the scheduled date is in the future.");
            }

            if (!AskToAddTheme(true))
            {
                break;
            }
        }
    }

    public static bool AskToAddTheme(bool isDatabaseEmpty)
    {
        string question = isDatabaseEmpty
        ? "No themes scheduled. Would you like to add a theme? (y/n): "
        : "Would you like to add a theme? (y/n): ";

        string input = InputValidator.GetValidString(question);
        return input == "y" || input == "yes";
    }

    public static void UpdateTheme()
    {
        DisplayAllThemes();
        Console.Clear();
        int newScheduledMonth, newScheduledYear;
        while (true)
        {
            newScheduledMonth = InputValidator.GetValidMonth("Enter new scheduled month (1-12) ");
            newScheduledYear = InputValidator.GetValidYear("Enter new scheduled year (YYYY) ", 2024);

            string newTheme = InputValidator.GetValidString("Enter new theme name: ");
            
            var theme = new ThemeMenuModel
            {
                ScheduledYear = newScheduledYear,
                ScheduledMonth = newScheduledMonth,
                ThemeName = newTheme
            };
            
            if (ThemeMenuManager.UpdateTheme(theme))
            {
                Console.WriteLine($"Theme for {ThemeMenuManager.GetMonthName(newScheduledMonth)} {newScheduledYear} updated to '{newTheme}.");
            }
            else
            {
                Console.WriteLine("Failed to update theme. Make sure a theme is scheduled for the specified month and year.");
            }
        }
    }


    public static void DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemeMenuManager.DeleteTheme(theme) )
        {
            Console.WriteLine($"Theme: {theme.ThemeName}, with ID: {theme.MenuId} deleted.");
        }
    }

}