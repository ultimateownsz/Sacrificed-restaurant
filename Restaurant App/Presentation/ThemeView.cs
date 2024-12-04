using Project;

static class ThemeView
{
    public static void YearAndMonthInputs()
    {
        int year = ThemeInputValidator.ValidateYear();
        
        string banner = "Select Month\n\n";            
        List<string> options = Enumerable.Range(1, 12).Select(m => ThemeMenuManager.GetMonthName(m)).ToList();
        int month;
        do
        {
            month = options.Count() - SelectionPresent.Show(options, banner, true).index;

            if (DateTime.Now.Month > month && DateTime.Now.Year == year)
            {
                Console.WriteLine("Invalid input. Please enter a month that is not in the past.");
                Console.ReadKey();
            }
        } while (DateTime.Now.Month > month && DateTime.Now.Year == year);

    }

    public static void SetOrUpdateTheme()
    {
        do
        {
            Console.Clear();
            YearAndMonthInputs();

            int? month = ThemeInputValidator.GetValidMonth("\nEnter 'Q' to quit or month (1-12): ");
            if (month == null)
                return;

            // // check if there is a theme planned
            var key = month ?? 0;
            var existingTheme = ThemeMenuManager.GetThemeByYearAndMonth(key);            

            if (existingTheme == null || existingTheme.Name == "Not scheduled")
            {
                // no theme exists, ask to add a new one
                Console.WriteLine($"\nNo theme exists for {ThemeMenuManager.GetMonthName(month)}.");
                string addTheme = ThemeInputValidator.GetValidString("\nDo you want to create a new theme? (y/n) ");
                if (addTheme.ToLower() == "y" || addTheme.ToLower() == "yes")
                {
                    string newThemeName = ThemeInputValidator.GetValidString("\nEnter the theme name: ");
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
                        YearAndMonthInputs();
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
                
                string UpdateTheme = ThemeInputValidator.GetValidString("\nDo you want to update the theme name? (y/n): ");

                if (UpdateTheme.ToLower() == "y" || UpdateTheme.ToLower() == "yes")
                {
                    string newThemeName = ThemeInputValidator.GetValidString("\nEnter the new theme name: ");
                    existingTheme.Name = newThemeName;
                    existingTheme.Month = month; // Update the month

                    bool isDuplicate;
                    // pass the existingTheme to the logic layer method for updating
                    if (ThemeMenuManager.AddOrUpdateTheme(existingTheme, month ?? 0, out isDuplicate))
                    {
                        Console.WriteLine("\nTheme updated successfully.");
                        Console.Clear();
                        YearAndMonthInputs();
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
            string retry = ThemeInputValidator.GetValidString("\nDo you want to manage another theme? (y/n): ");
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
        YearAndMonthInputs();

        int? scheduledMonth = ThemeInputValidator.GetValidMonth("\nEnter the month of the theme to delete (1-12) ");
        //int scheduledYear = ThemeInputValidator.GetValidYear("\nEnter the year of the theme to delete (YYYY): ", DateTime.Now.Year);

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
