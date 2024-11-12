static class ThemeView
{
    // show all planned themes
    public static void DisplayAllThemes()
    { 
        Console.Clear();

        // for now we use dict for the year & month values, next sprint should be from database
        var monthlyThemes = ThemeMenuManager.GetMonthlyDisplay();

        // Console.WriteLine("\nScheduled themes by month: ");
        Console.WriteLine("{0,-12} {1}: {2}", "Month", "Year", "Theme");
        Console.WriteLine(new string('-', 42));  // add a dotted line for clarity
        foreach (var theme in monthlyThemes)
        {
            string monthName = ThemeMenuManager.GetMonthName(theme.month);
            // Console.WriteLine($"{monthName} {theme.year}: theme - '{theme.themeName}'.");
            Console.WriteLine("{0,-12} {1}: theme - '{2}", monthName, theme.year, theme.themeName);
        }
    }

    public static void AddTheme()
    {
        bool isDatabaseEmpty = !ThemeMenuManager.GetScheduledThemes().Any();
        
        int month;
        int year;
        
        do
        {
            if (!AskToAddTheme("add", isDatabaseEmpty))
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
                Console.Clear();
                DisplayAllThemes();
                break;
            }
            else
            {
                Console.WriteLine("\nFailed to add theme. Make sure the scheduled date is in the future.");
            }

        } while (true);
        Console.Clear();
        AdminMenu.AdminStart();
    }

    //  
    private static bool AskToAddTheme(string action, bool isDatabaseEmpty)
    {
        DisplayAllThemes();
        string question = action.ToLower() == "add"
            ? (isDatabaseEmpty
                ? "\nNo Themes schedulded. Would you like to add a theme? (y/n) "
                : "\nWould you like to add another theme? (y/n) ")
            : "\nWould you like to update a theme month? (y/n) ";
        string input = InputValidator.GetValidString(question).ToLower();
        Console.Clear();
        return input.ToLower() == "y" || input.ToLower() == "yes";
    }

    public static void UpdateTheme()
    {   
        int newScheduledMonth, newScheduledYear;

        do
        {
            Console.Clear();
            DisplayAllThemes();
            if (!AskToAddTheme("update", false))
                {
                    AdminMenu.AdminStart();
                    return;
                }

            newScheduledMonth = InputValidator.GetValidMonth("\nEnter the month you want to edit (1-12) ");
            newScheduledYear = InputValidator.GetValidYear("\nEnter the year you want to edit (YYYY) ", DateTime.Now.Year);
        
            string newTheme = InputValidator.GetValidString("\nEnter a new theme (name): ");
        
            var theme = new ThemeMenuModel
            {
                ScheduledYear = newScheduledYear,
                ScheduledMonth = newScheduledMonth,
                ThemeName = newTheme
            };

            // attempt to update only if a theme exists for the specified month and year
            if (ThemeMenuManager.ThemeScheduledByYear.ContainsKey(newScheduledYear) &&
                ThemeMenuManager.ThemeScheduledByYear[newScheduledYear].Any(t => t.ScheduledMonth == newScheduledMonth))
                {
                    if (ThemeMenuManager.UpdateTheme(theme, newScheduledYear, newScheduledMonth))
                    {
                        Console.WriteLine($"\nTheme for {ThemeMenuManager.GetMonthName(newScheduledMonth)} {newScheduledYear} updated to '{newTheme}'.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update theme. Make sure a theme is scheduled for the specified month and year");
                    }
                }
            else
            {
                Console.WriteLine("\nNo theme found for the specified month and year.");
            }

        } while (true);
        Console.Clear();
        AdminMenu.AdminStart();
    }

// // prompt admin to try again or exit
                // string tryAgain = InputValidator.GetValidString("\nWould you like to update another theme? (y/n)").ToLower();
                // if (tryAgain != "y" && tryAgain != "yes")
                // {
                //     Console.WriteLine("Returning to admin menu.");
                //     break;
                // }

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