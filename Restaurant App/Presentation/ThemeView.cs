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

            int? month = ThemeInputValidator.GetValidMonth("\nEnter 'Q' to quit or month (1-12): ");
            if (month == null)
                return;

            int year = ThemeInputValidator.GetValidYear("\nEnter the year (YYYY): ", DateTime.Now.Year);

            // // check if there is a theme planned
            var key = (year, month ?? 0);
            var existingTheme = ThemeMenuManager.GetThemeByYearAndMonth(key);            

            if (existingTheme == null || existingTheme.ThemeName == "Not scheduled")
            {
                // no theme exists, ask to add a new one
                Console.WriteLine($"\nNo theme exists for {ThemeMenuManager.GetMonthName(month)} {year}.");
                string addTheme = ThemeInputValidator.GetValidString("\nDo you want to create a new theme? (y/n) ");
                if (addTheme.ToLower() == "y" || addTheme.ToLower() == "yes")
                {
                    string newThemeName = ThemeInputValidator.GetValidString("\nEnter the theme name: ");
                    var newTheme = new ThemeMenuModel
                    {
                        ThemeName = newThemeName,
                        ScheduledYear = year,
                        ScheduledMonth = month
                    };

                    bool isDuplicate;
                    if (ThemeMenuManager.AddOrUpdateTheme(newTheme, year, month ?? 0, out isDuplicate))
                    {
                        Console.WriteLine("\nTheme updated succesfully.");
                        Console.Clear();
                        DisplayAllThemes();
                    }
                    else if (isDuplicate)
                    {
                        Console.WriteLine($"\nA theme with the name '{newTheme.ThemeName}' already exists. Please choose a different name.");
                    }
                    else
                    {
                        Console.WriteLine($"\nFailed to update the theme.");
                    }
                }
            }
            else
            {
                // A theme already exists, prompt to update it
                Console.WriteLine($"\nA theme already exists for {ThemeMenuManager.GetMonthName(month)} {year}: '{existingTheme.ThemeName}'.");
                
                string UpdateTheme = ThemeInputValidator.GetValidString("\nDo you want to update the theme name? (y/n): ");

                if (UpdateTheme.ToLower() == "y" || UpdateTheme.ToLower() == "yes")
                {
                    string newThemeName = ThemeInputValidator.GetValidString("\nEnter the new theme name: ");
                    existingTheme.ThemeName = newThemeName;
                    existingTheme.ScheduledMonth = month; // Update the month

                    bool isDuplicate;
                    // pass the existingTheme to the logic layer method for updating
                    if (ThemeMenuManager.AddOrUpdateTheme(existingTheme, year, month ?? 0, out isDuplicate))
                    {
                        Console.WriteLine("\nTheme updated successfully.");
                        Console.Clear();
                        DisplayAllThemes();
                    }
                    else if (isDuplicate)
                    {
                        Console.WriteLine($"\nA theme with the name '{existingTheme.ThemeName}' already exists. Please choose a different name.");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update the theme.");
                    }
                }
            }

            // Ask the user if they want to manage another theme
            string retry = ThemeInputValidator.GetValidString("\nDo you want to manage another theme? (y/n): ");
            if (retry.ToLower() != "y" && retry.ToLower() != "yes")
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

        int? scheduledMonth = ThemeInputValidator.GetValidMonth("\nEnter the month of the theme to delete (1-12) ");
        int scheduledYear = ThemeInputValidator.GetValidYear("\nEnter the year of the theme to delete (YYYY): ", DateTime.Now.Year);

        var key = (scheduledYear, scheduledMonth ?? 0);
        var theme = ThemeMenuManager.GetThemeByYearAndMonth(key);

        if (theme != null && theme.ThemeName != "Not scheduled")
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
