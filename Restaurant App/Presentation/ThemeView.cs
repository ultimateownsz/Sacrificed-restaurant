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
            if(year == -1)
            {
                return;
            }
            int month = MonthChoice(year);
            if(month == 0) // The reasone why this checks for 0 and not -1 cuz i always do +1 to month while making it in MonthChoice(year)
            {
                return;
            }

            // This is to check if the chosen month has a theme, if not look at else
            // If it does have a theme then u enter a new mini menu
            if(ThemeMenuManager.GetThemeByYearAndMonth(month, year) is not null)
            {
                // In this menu u choose what happens with the chosen month (that does have a theme attached to it)
                string banner2 = $"{ThemeMenuManager.GetMonthName(month)}\nChoose:\n\n";
                List<string> options2 = new List<string>{"Edit the theme for this month", "Delete the theme for this month"};
                int selection = SelectionPresent.Show(options2, banner2, false).index;
                // This is for updating the theme the else is for deleting
                if(selection == 0)
                {
                    themeName = ThemeInputValidator.GetValidString();
                    ThemeMenuManager.UpdateThemeSchedule(month, year, themeName);
                    // Console.Clear();
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

            Console.WriteLine("Press escape to go back to admin menu, or press anykey to keep editing...");

        } while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape); //loops keeps going until user clicks escape at end of process

        return;

    }

    // Down here the selection menu for the year.
    public static int YearChoice()
    {
        List<string> optionsYears = Enumerable.Range(DateTime.Now.Year, 1000).Select(y => y.ToString()).ToList();
        string bannerYear = $"Select a year to edit its themes\n\n";  
        int year =  SelectionPresent.Show(optionsYears, bannerYear, true).index;
        if(year != -1)
        {
            year = DateTime.Now.Year - 1 + optionsYears.Count() - year;
            return year;
        }

        return year;

    }

    // Down here the selection menu for the months, u cant choose the current month.
    public static int MonthChoice(int year)
    {
        int month;
        string bannerMonths = $"Year: {year}\nSelect a month to edit the theme\n\n";
        List<string> optionsMonths = Enumerable.Range(1, 12).Select(m => ThemeMenuManager.GetMonthThemeName(m, year)).ToList();

        do
        {
            month = 1 + SelectionPresent.Show(optionsMonths, bannerMonths, false).index;
            if (month == 0) break;
            if (DateTime.Now.Month >= month && DateTime.Now.Year == year)
            {
                Console.WriteLine(" Invalid input. Please enter a month that is not in the past or the current month.");
                Console.ReadKey();
            }
        } while (DateTime.Now.Month >= month && DateTime.Now.Year == year);

        return month;

    }
}
