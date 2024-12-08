using Project;

static class ThemeView
{
    public static void YearAndMonthInputs()
    {
        ConsoleKeyInfo key;
        do
        {
            // The year input gets all made and validated in ThemeInputValidator.ValidateYear()
            Console.WriteLine("Please note: You can only enter a year from 2024 onwards for future themes. However, you can still view months in 2024 with themes that were already made in the past.");
            int year = ThemeInputValidator.ValidateYear();
            string themeName;
            
            // Down here the selection menu for the months is done, rn u cant choose the current month
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

            // This is to check if the chosen month has a theme, if not look at else
            // If it does have a theme then u enter a new mini menu
            if(ThemeMenuManager.GetThemeByYearAndMonth(month, year) is not null)
            {
                // In this menu u choose what happens with the chosen month (that does have a theme attached to it)
                string banner2 = $"Choose: {ThemeMenuManager.GetMonthThemeName(month, year)}\n\n";
                List<string> options2 = new List<string>{"Edit the theme for this month", "Delete the theme for this month"};
                int selection = SelectionPresent.Show(options2, banner2, false).index;
                // This is for updating the theme the else is for deleting
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
        } while ((key = Console.ReadKey(true)).Key != ConsoleKey.Escape); //loops keeps going until user clicks escape at end of process
        return;
    }


}
