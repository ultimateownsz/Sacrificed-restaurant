static class ThemeView
{
    // show all planned themes
    public static void DisplayAllThemes()
    { 
        Console.Clear();
        var themes = ThemeMenuManager.GetScheduledThemes().ToList();
        
        Console.WriteLine("Scheduled themes by month: ");
        if (themes == null || !themes.Any())
        {
            return;
        }

        foreach (var theme in themes)
        {
            if (theme.ScheduledMonth < 1 || theme.ScheduledMonth > 12)
            {
                Console.WriteLine($"The theme of {theme.ScheduledMonth} is not scheduled.");
                continue;
            }

            string monthName = ThemeMenuManager.GetMonthName(theme.ScheduledMonth);
            Console.WriteLine($"The theme of the month {monthName} {theme.ScheduledYear} is '{theme.ThemeName}' with MenuId: {theme.MenuId}.");
        }
    }

    public static void AddTheme()
    {
        bool isDatabaseEmpty = !ThemeMenuManager.GetScheduledThemes().Any();
        
        int month;
        int year;
        
        do
        {
            if (!AskToAddTheme(isDatabaseEmpty))
            {
                AdminMenu.AdminStart();
                return;
            }

            Console.Clear();
            DisplayAllThemes();
            month = InputValidator.GetValidMonth("\nEnter scheduled month (1-12) ");
            year = InputValidator.GetValidYear("\nEnter scheduled year (YYYY) ", DateTime.Now.Year);

            if (ThemeMenuManager.IsFutureDate(year, month))
            {
                break;
            }
            else
            {
                Console.WriteLine("\nInvalid date. Please enter a month and year in the future.");
            }
            Console.Clear();
            DisplayAllThemes();

        } while (!ThemeMenuManager.IsFutureDate(year, month));
        
        do
        {
            string themeName = InputValidator.GetValidString("\nEnter theme (name): ");
            var theme = new ThemeMenuModel
            { 
                ThemeName = themeName,
                ScheduledMonth = month,
                ScheduledYear = year
            
            };
            
            if (ThemeMenuManager.ValidateToAddTheme(theme, year, month))
            {
                ThemeMenuManager.AddTheme(theme, year, month);
                Console.WriteLine($"\nTheme '{theme.ThemeName}', scheduled for {ThemeMenuManager.GetMonthName(theme.ScheduledMonth)} {theme.ScheduledYear}.");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("\nFailed to add theme. Make sure the scheduled date is in the future.");
            }

            if (!AskToAddTheme(false))
            {
                AdminMenu.AdminStart();
                return;
            }
        } while (true);
        Console.Clear();
        AdminMenu.AdminStart();
    }

    private static bool AskToAddTheme(bool isDatabaseEmpty)
    {
        if (isDatabaseEmpty)
        {
            Console.WriteLine("\nNo themes scheduled. Would you like to add a theme? (y/n): ");
        }
        else
        {
            Console.WriteLine("\nWould you like to add another theme? (y/n): ");
        }
        DisplayAllThemes();
        string question = "\nWould you like to add another theme? (y/n): ";
        string input = InputValidator.GetValidString(question);
        return input.ToLower() == "y" || input.ToLower() == "yes";
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

            string newTheme = InputValidator.GetValidString("Enter new theme (name): ");
            
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