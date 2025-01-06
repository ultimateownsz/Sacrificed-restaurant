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
                var selection = SelectionPresent.Show(options, banner, false);

                if (selection.index == -1) return;  // escape pressed

                if (selection.index == 0)
                {
                    ControlHelpPresent.Clear();
                    ControlHelpPresent.AddOptions("Escape", "<escape>");
                    ControlHelpPresent.ShowHelp();
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        themeName = InputHelper.GetValidatedInput<string>(
                            "Enter the new theme for this month: ",
                            themeName => (themeName, string.IsNullOrWhiteSpace(themeName) ? "Theme name cannot be empty." : null),
                            menuTitle: "EDIT THEME",
                            showHelpAction: () => ControlHelpPresent.ShowHelp()
                        );
                        
                        // ThemeInputValidator.GetValidString();
                        ThemeMenuLogic.UpdateThemeSchedule(month, year, themeName);
                        ControlHelpPresent.DisplayFeedback($"The theme has been updated to {themeName}", "bottom", "success");
                        ControlHelpPresent.ResetToDefault();
                    }
                    );
                }
                else // Delete the theme for this month
                {
                    ThemeMenuLogic.DeleteMonthTheme(month, year);
                    Console.Clear();
                    ControlHelpPresent.DisplayFeedback("This theme has been deleted", "bottom", "success");
                }
            }
            else // Menu to add a theme to a month without a theme
            {
                ControlHelpPresent.Clear();
                ControlHelpPresent.AddOptions("Escape", "<escape>");
                ControlHelpPresent.ShowHelp();
                TryCatchHelper.EscapeKeyException(() =>
                {
                    themeName = InputHelper.GetValidatedInput<string>(
                        "Enter the theme for this month: ",
                        themeName => (themeName, string.IsNullOrWhiteSpace(themeName) ? "Theme name cannot be empty." : null),
                        menuTitle: "ADD THEME",
                        showHelpAction: () => ControlHelpPresent.ShowHelp()
                    );
                    ThemeMenuLogic.UpdateThemeSchedule(month, year, themeName);
                    ControlHelpPresent.DisplayFeedback($"The theme has been added: {themeName}", "bottom", "success");
                    ControlHelpPresent.ResetToDefault();
                }
                );
                
            }

            ControlHelpPresent.DisplayFeedback("Go back to previous menu, or press any key to keep editing...", "bottom", feedbackType: "tip");

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
            Console.WriteLine("Select a year to edit its themes:\n");

            // Highlight the currently selected year in yellow
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(availableYears[currentIndex]);
            Console.ResetColor();

            // Display the footer
            ControlHelpPresent.Clear();
            ControlHelpPresent.ResetToDefault();
            ControlHelpPresent.AddOptions("Reset a year", "<r>");
            ControlHelpPresent.ShowHelp();

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
                        ControlHelpPresent.DisplayFeedback("Cannot navigate to years in the past.");
                        // message = "Cannot navigate to years in the past.";
                    }
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                    ControlHelpPresent.DisplayFeedback("You can only navigate using the 'Up' and 'Down' <arrows>.");
                    // message = "You can only navigate using the Up and Down arrows.";
                    break;

                case ConsoleKey.R:
                    // message = string.Empty; // Clear message on reset
                    ControlHelpPresent.DisplayFeedback("Year has been reset to the current year.", "bottom", "success");
                    currentIndex = currentYearIndex; // Reset to the current year
                    break;

                case ConsoleKey.Escape:
                    ControlHelpPresent.DisplayFeedback("Returning to the previous menu...", "bottom", "error");
                    return -1; // Indicate user wants to go back

                case ConsoleKey.Enter:
                    return availableYears[currentIndex]; // Return the selected year

                default:
                    ControlHelpPresent.DisplayFeedback("Invalid input. Use <arrows>, <r>, <escape>, or <enter>.", "bottom", "error");
                    // message = "Invalid input. Use <arrows>, <r>, <escape>, or <enter>.";
                    break;
            }
        }
    }

    public static int MonthChoice(int year)
    {
        int month;
        string bannerMonths = $"Select month in {year}\n\n";
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
                ControlHelpPresent.DisplayFeedback("Invalid input. Please select a month that is not in the past or the current month.");
                Console.ReadKey();
            }
            else
            {
                return month; // Return the selected month if valid
            }
        }
    }
}
