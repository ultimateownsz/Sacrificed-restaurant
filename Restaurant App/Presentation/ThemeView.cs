using Project;

static class ThemeView
{
    public static void ThemedEditing()
    {
        ConsoleKeyInfo key;
        do
        {
            string themeName;
            int year = YearChoice();
            if (year == -1)
            {
                return;
            }
            int month = MonthChoice(year);
            if (month == 0) // The reason why this checks for 0 and not -1 because +1 is done in MonthChoice(year)
            {
                return;
            }

            // Check if the chosen month has a theme, if not proceed with else
            if (ThemeMenuManager.GetThemeByYearAndMonth(month, year) is not null)
            {
                // Menu to choose actions for a month with a theme attached
                string banner2 = $"{ThemeMenuManager.GetMonthName(month)}\nChoose:\n\n";
                List<string> options2 = new List<string> { "Edit the theme for this month", "Delete the theme for this month" };
                int selection = SelectionPresent.Show(options2, banner2, false).index;

                if (selection == 0)
                {
                    themeName = ThemeInputValidator.GetValidString();
                    ThemeMenuManager.UpdateThemeSchedule(month, year, themeName);
                    Console.WriteLine($"The theme has been updated to {themeName}");
                }
                else
                {
                    ThemeMenuManager.DeleteMonthTheme(month, year);
                    Console.Clear();
                    Console.WriteLine("This theme has been deleted");
                }
            }
            else
            {
                themeName = ThemeInputValidator.GetValidString();
                ThemeMenuManager.UpdateThemeSchedule(month, year, themeName);
                Console.WriteLine($"The theme has been updated to {themeName}");
            }

            Console.WriteLine("Press Escape to go back to admin menu, or press any key to keep editing...");

        } while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape);

        return;
    }

    public static int YearChoice()
    {
        // Fetch available years from the database and add future years
        List<int> availableYears = ThemeMenuManager.GetAvailableYears();
        int minYear = DateTime.Now.Year;
        int currentYearIndex = availableYears.IndexOf(minYear);
        if (currentYearIndex == -1)
        {
            availableYears.Insert(0, minYear); // Ensure the current year is included if not present
            currentYearIndex = 0;
        }

        int currentIndex = currentYearIndex;
        string message = string.Empty; // Message to display

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Select a year to edit its themes:");

            // Highlight the current year in yellow
            for (int i = 0; i < availableYears.Count; i++)
            {
                if (i == currentIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n{availableYears[i]}"); // Highlighted year
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"\n{availableYears[i]}"); // Regular year
                }
            }

            Console.WriteLine("\n(r)eset, (b)ack");

            // Display message if applicable
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
                        // Add a new future year dynamically
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

                case ConsoleKey.B:
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
        string bannerMonths = $"Year: {year}\nSelect a month to edit the theme\n\n";
        List<string> optionsMonths = Enumerable.Range(1, 12)
            .Select(m => ThemeMenuManager.GetMonthThemeName(m, year))
            .ToList();

        do
        {
            month = 1 + SelectionPresent.Show(optionsMonths, bannerMonths, false).index;
            if (month == 0) break;

            if (DateTime.Now.Month >= month && DateTime.Now.Year == year)
            {
                Console.WriteLine("Invalid input. Please select a month that is not in the past or the current month.");
                Console.ReadKey();
            }
        } while (DateTime.Now.Month >= month && DateTime.Now.Year == year);

        return month;
    }
}
