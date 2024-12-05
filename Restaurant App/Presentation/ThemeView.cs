using Project;

static class ThemeView
{
    public static void YearAndMonthInputs()
    {
        ConsoleKeyInfo key;
        do
        {
            // The year input gets all made and validated in ThemeInputValidator.ValidateYear()
            int year = ThemeInputValidator.ValidateYear();
            string themeName;
            
            string banner = $"Year: {year}\nSelect a month to edit the theme\n\n";            
            List<string> options = Enumerable.Range(1, 12).Select(m => ThemeMenuManager.GetMonthThemeName(m, year)).ToList();
            int month;
            do
            {
                month = 1 + SelectionPresent.Show(options, banner, false).index;

                if (DateTime.Now.Month >= month && DateTime.Now.Year == year)
                {
                    Console.WriteLine("Invalid input. Please enter a month that is not in the past or the current month.");
                    Console.ReadKey();
                }
            } while (DateTime.Now.Month >= month && DateTime.Now.Year == year);

            if(ThemeMenuManager.GetThemeIDByYearAndMonth(month, year) is not null)
            {
                string banner2 = $"Choose: {ThemeMenuManager.GetMonthThemeName(month, year)}\n\n";
                List<string> options2 = new List<string>{"Edit the theme for this month", "Delete the theme for this month"};
                int selection = SelectionPresent.Show(options2, banner2, false).index;
                if(selection == 0)
                {
                    themeName = ThemeInputValidator.GetValidString();
                    ThemeMenuManager.UpdateThemeSchedule(month, year, themeName);
                }
                else
                {
                    ThemeMenuManager.DeleteMonthTheme(month, year);
                }
            }
            else
            {
                themeName = ThemeInputValidator.GetValidString();
                ThemeMenuManager.UpdateThemeSchedule(month, year, themeName);
            }
            Console.Clear();
            Console.WriteLine("Press escape to go back to admin menu, or press anykey to keep editing...");
        } while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape);

    }

    // public static void SetOrUpdateTheme()
    // {
    //     do
    //     {
    //         Console.Clear();
    //         YearAndMonthInputs();



    //         // // check if there is a theme planned
    //         var key = month ?? 0;
    //         var existingTheme = ThemeMenuManager.GetThemeByYearAndMonth(key, 0);            

    //         if (existingTheme == null || existingTheme.Name == "Not scheduled")
    //         {
    //             // no theme exists, ask to add a new one
    //             Console.WriteLine($"\nNo theme exists for {ThemeMenuManager.GetMonthThemeName(month, 0)}.");
    //             string addTheme = ThemeInputValidator.GetValidString("\nDo you want to create a new theme? (y/n) ");
    //             if (addTheme.ToLower() == "y" || addTheme.ToLower() == "yes")
    //             {
    //                 string newThemeName = ThemeInputValidator.GetValidString("\nEnter the theme name: ");
    //                 var newTheme = new ThemeModel
    //                 {
    //                     Name = newThemeName,
    //                     Month = month 
    //                 };

    //                 bool isDuplicate;
    //                 if (ThemeMenuManager.AddOrUpdateTheme(newTheme, month ?? 0, out isDuplicate))
    //                 {
    //                     Console.WriteLine("\nTheme updated succesfully.");
    //                     Console.Clear();
    //                     YearAndMonthInputs();
    //                 }
    //                 else if (isDuplicate)
    //                 {
    //                     Console.WriteLine($"\nA theme with the name '{newTheme.Name}' already exists. Please choose a different name.");
    //                 }
    //                 else
    //                 {
    //                     Console.WriteLine($"\nFailed to update the theme.");
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             // A theme already exists, prompt to update it
    //             Console.WriteLine($"\nA theme already exists for {ThemeMenuManager.GetMonthThemeName(month, 0)}: '{existingTheme.Name}'.");
                
    //             string UpdateTheme = ThemeInputValidator.GetValidString("\nDo you want to update the theme name? (y/n): ");

    //             if (UpdateTheme.ToLower() == "y" || UpdateTheme.ToLower() == "yes")
    //             {
    //                 string newThemeName = ThemeInputValidator.GetValidString("\nEnter the new theme name: ");
    //                 existingTheme.Name = newThemeName;
    //                 existingTheme.Month = month; // Update the month

    //                 bool isDuplicate;
    //                 // pass the existingTheme to the logic layer method for updating
    //                 if (ThemeMenuManager.AddOrUpdateTheme(existingTheme, month ?? 0, out isDuplicate))
    //                 {
    //                     Console.WriteLine("\nTheme updated successfully.");
    //                     Console.Clear();
    //                     YearAndMonthInputs();
    //                 }
    //                 else if (isDuplicate)
    //                 {
    //                     Console.WriteLine($"\nA theme with the name '{existingTheme.Name}' already exists. Please choose a different name.");
    //                 }
    //                 else
    //                 {
    //                     Console.WriteLine("\nFailed to update the theme.");
    //                 }
    //             }
    //         }

    //         // Ask the user if they want to manage another theme
    //         string retry = ThemeInputValidator.GetValidString("\nDo you want to manage another theme? (y/n): ");
    //         if (retry.ToLower() != "y" && retry.ToLower() != "yes")
    //         {
    //             break;
    //         }
    //     } while (true);

    //     Console.Clear();
    //     return;
    // }
    

    // public static void DeleteTheme()
    // {
    //     YearAndMonthInputs();

    //     int? scheduledMonth = ThemeInputValidator.GetValidMonth("\nEnter the month of the theme to delete (1-12) ");
    //     //int scheduledYear = ThemeInputValidator.GetValidYear("\nEnter the year of the theme to delete (YYYY): ", DateTime.Now.Year);

    //     var key = scheduledMonth ?? 0;
    //     var theme = ThemeMenuManager.GetThemeByYearAndMonth(key, 0);

    //     if (theme != null && theme.Name != "Not scheduled")
    //     {
    //         if (ThemeMenuManager.DeleteTheme(theme))
    //         {
    //             Console.WriteLine($"Theme '{theme.Name}' for {ThemeMenuManager.GetMonthThemeName(scheduledMonth, 0)} deleted.");
    //         }
    //         else
    //         {
    //             Console.WriteLine("Failed to delete theme. The specified theme may not exist.");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine("No theme found for the specified month and year.");
    //     }
    // }
}
