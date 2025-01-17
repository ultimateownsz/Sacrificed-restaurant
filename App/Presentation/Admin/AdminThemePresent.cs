using App.Logic.Theme;
using Restaurant;

namespace App.Presentation.Admin;
static class AdminThemePresent
{
    public static void ThemedEditing()
    {
        while (true)
        {
            string? themeName;
            int year = YearChoice();
            if (year == -1)
                return;

            monthSelection:
            int month = MonthChoice(year);
            if (month == 0) // The reason why this checks for 0 and not -1 because +1 is done in MonthChoice(year)
                continue;

            // Check if the chosen month has a theme, if not proceed with else
            if (ThemeManageLogic.GetThemeByYearAndMonth(month, year) is not null)
            {
            // Menu to choose actions for a month with a theme attached
            middleMenu:
                string banner2 = $"{ThemeManageLogic.GetMonthName(month)}\nChoose:";
                List<string> options2 = new List<string> { "Choose an existing theme for this month", "Add a new theme to this month", "Delete the theme for this month" };
                int selection = SelectionPresent.Show(options2, banner: banner2).ElementAt(0).index;
                if (selection == -1)
                    goto monthSelection;

                if (selection == 0)
                {
                    themeName = ThemeValidateLogic.GetValidThemeMenu();
                    if (themeName == null || themeName == "REQUEST_PROCESS_EXIT")
                    {
                        goto middleMenu;
                    }
                    else
                    {
                        ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);
                        goto monthSelection;
                    }
                }
                else if (selection == 1)
                {
                    themeName = ThemeValidateLogic.GetValidString();
                    if (themeName == null || themeName == "REQUEST_PROCESS_EXIT")
                    {
                        Console.Clear();
                        Console.WriteLine("Failed to update theme");
                        goto middleMenu;
                    }
                    ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);
                    Console.WriteLine($"The theme has been updated to {themeName}");
                }
                else if (selection == 2)
                {
                    ThemeManageLogic.DeleteMonthTheme(month, year);

                    Console.Clear();
                    Console.WriteLine("This theme has been deleted!");

                    Thread.Sleep(1000);
                    goto monthSelection;

                }
            }
            else
            {
            middleMenu2:
                string banner2 = $"{ThemeManageLogic.GetMonthName(month)}\nChoose:";
                List<string> options2 = new List<string> { "Choose an existing theme for this month", "Add a new theme to this month" };
                int selection = SelectionPresent.Show(options2, banner: banner2).ElementAt(0).index;
                if (selection == -1)
                    goto monthSelection;

                if (selection == 0)
                {
                    themeName = ThemeValidateLogic.GetValidThemeMenu();
                    if (themeName == null || themeName == "REQUEST_PROCESS_EXIT")
                    {
                        goto middleMenu2;
                    }
                    else
                    {
                        ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);
                        goto monthSelection;
                    }
                }
                else
                {
                    themeName = ThemeValidateLogic.GetValidString();

                }

                if (themeName == null)
                {
                    Console.Clear();
                    Console.WriteLine("Failed to update theme");
                    goto middleMenu2;
                }
                ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);
                Console.WriteLine($"The theme has been updated to {themeName}");
            }

            break;
        }
    }

    public static int YearChoice()
    {
        // Fetch available years from the database and add future years
        List<int> availableYears = ThemeManageLogic.GetAvailableYears();
        int minYear = DateTime.Now.Year;
        int currentYearIndex = availableYears.IndexOf(minYear);

        if (currentYearIndex == -1)
        {
            availableYears.Insert(0, minYear); // Ensure the current year is included if not present
            currentYearIndex = 0;
        }

        int currentIndex = currentYearIndex;
        string message = string.Empty; // Message for errors or additional info

        while (true)
        {
            TerminableUtilsPresent.Write("Select a year to edit its themes:\n");

            // Highlight the currently selected year in Blue
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(availableYears[currentIndex]);
            Console.ResetColor();

            // Navigation options
            Console.WriteLine("\n(r)eset, (esc)ape");

            // Display any error or info messages
            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"\n{message}");
            }

            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    message = string.Empty; // Clear message on valid navigation
                    if (currentIndex < availableYears.Count - 1)
                    {
                        currentIndex++;
                    }
                    else
                    {
                        // Dynamically add a new future year and navigate to it
                        int nextYear = availableYears[^1] + 1;
                        availableYears.Add(nextYear);
                        currentIndex++;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (currentIndex > 0)
                    {
                        message = string.Empty; // Clear message on valid navigation
                        currentIndex--;
                    }
                    else
                    {
                        message = "Cannot navigate to years in the past.";
                    }
                    break;

                case ConsoleKey.R:
                    message = string.Empty; // Clear message on reset
                    currentIndex = currentYearIndex; // Reset to the current year
                    break;

                case ConsoleKey.Escape:
                    return -1; // Indicate user wants to go back

                case ConsoleKey.Enter:
                    return availableYears[currentIndex]; // Return the selected year

                default:
                    message = "Invalid input. Use arrow keys, 'R', 'B', or Enter.";
                    break;
            }
        }
    }

    public static int MonthChoice(int year)
    {
        int month;
        string banner
            = $"Select month in {year}, or go back with <ESC>\n";
        List<string> optionsMonths = Enumerable.Range(1, 12)
            .Select(m => ThemeManageLogic.GetMonthThemeName(m, year))
            .ToList();

        while (true)
        {

            var selection = SelectionPresent.Show(optionsMonths, banner: banner);
            month = selection.ElementAt(0).index + 1;

            if (month - 1 == -1) return 0;

            if (DateTime.Now.Month == month && DateTime.Now.Year == year && ThemeManageLogic.GetThemeByYearAndMonth(month, year) is null) // only returns the current month if it has no theme
            {
                return month;
            }
            if (DateTime.Now.Month >= month && DateTime.Now.Year == year)
            {
                Console.WriteLine("Invalid input. Please select a month that is not in the past or the current month.");
                Console.ReadKey();
            }
            else
            {
                return month; // Return the selected month if valid
            }
        }
    }
}
