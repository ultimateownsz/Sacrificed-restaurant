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
            if (year == -1) return;

            int month = MonthChoice(year);
            if (month == 0) // The reason why this checks for 0 and not -1 because +1 is done in MonthChoice(year)
            {
                return;
            }

            // Check if the chosen month has a theme, if not proceed with else
            if (ThemeMenuLogic.GetThemeByYearAndMonth(month, year) is not null)
            {
                // Menu to choose actions for a month with a theme attached
                string banner = $"{ThemeMenuLogic.GetMonthName(month)}\nChoose:\n\n";
                List<string> options = new List<string> { "Edit the theme for this month", "Delete the theme for this month" };
                dynamic selection = SelectionPresent.Show(options, banner, false);

                if (selection.index == -1) return;  // escape pressed

                if (selection.index == 0)
                {
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        themeName = ThemeInputValidator.GetValidString();
                        ThemeMenuLogic.UpdateThemeSchedule(month, year, themeName);
                        Console.WriteLine($"The theme has been updated to {themeName}");
                    }
                    );
                }
                else
                {
                    ThemeMenuLogic.DeleteMonthTheme(month, year);
                    Console.Clear();
                    Console.WriteLine("This theme has been deleted");
                }
            }
            // else
            // {
            //     themeName = ThemeInputValidator.GetValidString();
            //     ThemeMenuLogic.UpdateThemeSchedule(month, year, themeName);
            //     Console.WriteLine($"The theme has been updated to {themeName}");
            // }

            Console.WriteLine("Press <escape> to go back to admin menu, or press any key to keep editing...");

        } while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape);

        return;
    }

    public static int YearChoice()
    {
        // Fetch available years from the database and add future years
        List<int> availableYears = ThemeMenuLogic.GetAvailableYears();
        int minYear = DateTime.Now.Year;
        int currentYearIndex = availableYears.IndexOf(minYear);

        if (currentYearIndex == -1)
        {
            availableYears.Insert(0, minYear); // Ensure the current year is included if not present
            currentYearIndex = 0;
        }

        int currentIndex = currentYearIndex;
        // string message = string.Empty; // Message for errors or additional info

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Use the up and down arrow keys to select a year to edit it's themes:\n");

            // Highlight the currently selected year in yellow
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(availableYears[currentIndex]);
            Console.ResetColor();

            // Display the footer
            ControlsHelperPresent.Clear();
            ControlsHelperPresent.ResetToDefault();
            ControlsHelperPresent.AddOptions("Reset a year", "<r>");
            ControlsHelperPresent.ShowHelp();
            // Console.WriteLine("\nControls  :\n");
            // Console.WriteLine("Navigate    : <arrows>");
            // Console.WriteLine("Select      : <enter>");
            // Console.WriteLine("Reset a year: <R>");
            // Console.WriteLine("Exit        : <escape>");

            // Display any error or info messages
            // if (!string.IsNullOrEmpty(message))
            // {
            //     Console.WriteLine($"\n{message}");
            // }

            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    // message = string.Empty; // Clear message on valid navigation
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
                        // message = string.Empty; // Clear message on valid navigation
                        currentIndex--;
                    }
                    else
                    {
                        ControlsHelperPresent.DisplayFeedback("Cannot navigate to years in the past.");
                        // message = "Cannot navigate to years in the past.";
                    }
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    ControlsHelperPresent.DisplayFeedback("You can only navigate using the Up and Down arrows.");
                    // message = "You can only navigate using the Up and Down arrows.";
                    break;

                case ConsoleKey.R:
                    // message = string.Empty; // Clear message on reset
                    currentIndex = currentYearIndex; // Reset to the current year
                    break;

                case ConsoleKey.Escape:
                    return -1; // Indicate user wants to go back

                case ConsoleKey.Enter:
                    return availableYears[currentIndex]; // Return the selected year

                default:
                    ControlsHelperPresent.DisplayFeedback("Invalid input. Use <arrows>, <r>, <escape>, or <enter>.");
                    // message = "Invalid input. Use <arrows>, <r>, <escape>, or <enter>.";
                    break;
            }
        }
    }

    public static int MonthChoice(int year)
    {
        int month;
        string bannerMonths = $"Select month in {year}, or press <escape>\n\n";
        List<string> optionsMonths = Enumerable.Range(1, 12)
            .Select(m => ThemeMenuLogic.GetMonthThemeName(m, year))
            .ToList();

        while (true)
        {
            // brother, what? cm'on
            //Console.Clear();
            //Console.WriteLine(bannerMonths);
            //for (int i = 0; i < optionsMonths.Count; i++)
            //{
            //    Console.WriteLine($"{i + 1}. {optionsMonths[i]}");
            //}

            //Console.WriteLine("\n(b)ack")
            //var key = Console.ReadKey(intercept: true);
            
            //if (key.Key == ConsoleKey.B)
            //{
            //    return 0; // Indicate going back
            //}

            month = 1 + SelectionPresent.Show(optionsMonths, bannerMonths, false).index;
            if (month == 0)
            {
                return 0; // Ensure a return value for the back option
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
