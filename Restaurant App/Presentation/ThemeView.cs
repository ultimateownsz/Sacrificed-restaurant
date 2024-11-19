static class ThemeView
{
    public static void DisplayAllThemes()
    {
        Console.Clear();

        var monthlyThemes = ThemeMenuManager.GetMonthlyDisplay();

        Console.WriteLine("{0,-12} {1}: {2}", "Month", "Year", "Theme");
        Console.WriteLine(new string('-', 42));

        foreach (var theme in monthlyThemes)
        {
            string monthName = ThemeMenuManager.GetMonthName(theme.month);
            Console.WriteLine("{0,-12} {1}: theme - '{2}'", monthName, theme.year, theme.themeName);
        }
    }

    public static void SetOrUpdateTheme()
    {
        do
        {
            Console.Clear();
            DisplayAllThemes();

            int month = InputValidator.GetValidMonth("Enter the month (1-12): ");
            int year = InputValidator.GetValidYear("Enter the year (YYYY): ", DateTime.Now.Year);

            // check if there is a theme planned
            var existingTheme = ThemeMenuManager.ThemeScheduledByYear.ContainsKey(year)
                ? ThemeMenuManager.ThemeScheduledByYear[year].FirstOrDefault(t => t.ScheduledMonth == month)
                : null;

            if (existingTheme != null && existingTheme.ThemeName != "Not scheduled")
            {
                // no theme exists, ask to add a new one
                Console.WriteLine($"\nNo theme exists for {ThemeMenuManager.GetMonthName(month)} {year}: '{existingTheme.ThemeName}'.");
                string addTheme = InputValidator.GetValidString("\nDo you want to create a new theme? (y/n) ");
                if (addTheme == "y" || addTheme == "yes")
                {
                    string newThemeName = InputValidator.GetValidString("Enter the theme name: ");
                    var newTheme = new ThemeMenuModel
                    {
                        ThemeName = newThemeName,
                        ScheduledYear = year,
                        ScheduledMonth = month
                    };

                    if (ThemeMenuManager.AddOrUpdateTheme(newTheme, year, month))
                    {
                        Console.WriteLine("\nTheme updated succesfully.");
                        Console.Clear();
                        DisplayAllThemes();
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update the theme.");
                    }
                }
            }
            else
            {
                // A theme exists, prompt to update it
                Console.WriteLine($"\nA theme already exists for {ThemeMenuManager.GetMonthName(month)} {year}: '{existingTheme.ThemeName}'.");
                
                string UpdateTheme = InputValidator.GetValidString("Do you want to update the theme name? (y/n): ");

                if (UpdateTheme == "y" || UpdateTheme == "yes")
                {
                    string newThemeName = InputValidator.GetValidString("Enter the new theme name: ");
                    existingTheme.ThemeName = newThemeName;
                    existingTheme.ScheduledMonth = month; // Update the month

                    if (ThemeMenuManager.AddOrUpdateTheme(existingTheme, year, month))
                    {
                        Console.WriteLine("\nTheme updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update the theme.");
                    }
                }
            }

            // Ask the user if they want to manage another theme
            string retry = InputValidator.GetValidString("\nDo you want to manage another theme? (y/n): ");
            if (retry != "y" && retry != "yes")
            {
                break;
            }
        } while (true);

        Console.Clear();
        AdminMenu.AdminStart();
    }
    

    public static void DeleteTheme()
    {
        DisplayAllThemes();

        int scheduledMonth = InputValidator.GetValidMonth("Enter the month of the theme to delete (1-12) ");
        int scheduledYear = InputValidator.GetValidYear("Enter the year of the theme to delete (YYYY): ", DateTime.Now.Year);

        var theme = ThemeMenuManager.ThemeScheduledByYear.ContainsKey(scheduledYear)
            ? ThemeMenuManager.ThemeScheduledByYear[scheduledYear].FirstOrDefault(t => t.ScheduledMonth == scheduledMonth)
            : null;

        if (theme != null)
        {
            if (ThemeMenuManager.DeleteTheme(theme))
            {
                Console.WriteLine($"Theme '{theme.ThemeName}' for {ThemeMenuManager.GetMonthName(scheduledMonth)} {scheduledYear} deleted.");
            }
            else
            {
                Console.WriteLine("Failed to delete theme. The specified theme may not exist.");
            }
        }
        else
        {
            Console.WriteLine("No theme found for the specified month and year.");
        }
    }
}
