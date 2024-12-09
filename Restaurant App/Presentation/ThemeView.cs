using Project;

static class ThemeView
{
    public static void DisplayAllThemes()
    {
        Console.Clear();

        var monthlyThemes = ThemeMenuManager.GetMonthlyDisplay();

        Console.WriteLine("{0,-12} {1}", "Month", "Theme");
        Console.WriteLine(new string('-', 42));

        foreach (var theme in monthlyThemes)
        {
            string monthName = ThemeMenuManager.GetMonthName(theme.month);
            Console.WriteLine("{0,-12}: theme - '{1}'", monthName, theme.themeName);
        }
    }

    public static void SetOrUpdateTheme()
    {
        do
        {
            Console.Clear();
            DisplayAllThemes();

            int? month = InputHelper.GetValidatedInput(
                "\nEnter 'Q' to quit or month (1-12): ", InputLogic.ParseValidMonth
            );
            if (month == null)
                return;

            // removed because fuck that
            //int year = ThemeInputValidator.GetValidYear("\nEnter the year (YYYY): ", DateTime.Now.Year);

            // // check if there is a theme planned
            var key = month ?? 0;
            var existingTheme = ThemeMenuManager.GetThemeByYearAndMonth(key);            

            if (existingTheme == null || existingTheme.Name == "Not scheduled")
            {
                // no theme exists, ask to add a new one
                // Console.WriteLine($"\nNo theme exists for { .GetMonthName(month)}.");
                string addTheme = InputHelper.GetValidatedInput("\nDo you want to create a new theme? (y/n) ", InputLogic.ParseValidString);
                if (addTheme.ToLower() == "y" || addTheme.ToLower() == "yes")
                {
                    string newThemeName = InputHelper.GetValidatedInput("\nEnter the theme name: ", InputLogic.ParseValidString);
                    var newTheme = new ThemeModel
                    {
                        Name = newThemeName,
                        Month = month 
                    };

                    bool isDuplicate;
                    if (ThemeMenuManager.AddOrUpdateTheme(newTheme, month ?? 0, out isDuplicate))
                    {
                        Console.WriteLine("\nTheme updated succesfully.");
                        Console.Clear();
                        DisplayAllThemes();
                    }
                    else if (isDuplicate)
                    {
                        Console.WriteLine($"\nA theme with the name '{newTheme.Name}' already exists. Please choose a different name.");
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
                Console.WriteLine($"\nA theme already exists for {ThemeMenuManager.GetMonthName(month)}: '{existingTheme.Name}'.");
                
                string UpdateTheme = InputHelper.GetValidatedInput("\nDo you want to update the theme name? (y/n): ", InputLogic.ParseValidString);

                if (UpdateTheme.ToLower() == "y" || UpdateTheme.ToLower() == "yes")
                {
                    string newThemeName = InputHelper.GetValidatedInput("\nEnter the new theme name: ", InputLogic.ParseValidString);
                    existingTheme.Name = newThemeName;
                    existingTheme.Month = month; // Update the month

                    bool isDuplicate;
                    // pass the existingTheme to the logic layer method for updating
                    if (ThemeMenuManager.AddOrUpdateTheme(existingTheme, month ?? 0, out isDuplicate))
                    {
                        Console.WriteLine("\nTheme updated successfully.");
                        Console.Clear();
                        DisplayAllThemes();
                    }
                    else if (isDuplicate)
                    {
                        Console.WriteLine($"\nA theme with the name '{existingTheme.Name}' already exists. Please choose a different name.");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update the theme.");
                    }
                }
            }

            // Ask the user if they want to manage another theme
            string retry = InputHelper.GetValidatedInput("\nDo you want to manage another theme? (y/n): ", InputLogic.ParseValidString);
            if (retry.ToLower() != "y" && retry.ToLower() != "yes")
            {
                break;
            }
        } while (true);

        Console.Clear();
        return;
    }
    

    public static void DeleteTheme()
    {
        DisplayAllThemes();

        int? scheduledMonth = InputHelper.GetValidatedInput("\nEnter the month of the theme to delete (1-12) ", InputLogic.ParseValidMonth);

        var key = scheduledMonth ?? 0;
        var theme = ThemeMenuManager.GetThemeByYearAndMonth(key);

        if (theme != null && theme.Name != "Not scheduled")
        {
            if (ThemeMenuManager.DeleteTheme(theme))
            {
                Console.WriteLine($"Theme '{theme.Name}' for {ThemeMenuManager.GetMonthName(scheduledMonth)} deleted.");
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
