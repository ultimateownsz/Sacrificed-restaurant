// the presentation layer uses this class to validate the input of the user
using Project;

public static class ThemeInputValidator
{
    public static long GetValidLong(string prompt, long minValue = 0)
    {
        long result;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (long.TryParse(input, out result) && result >= minValue)
            {
                return result;
            }
            Console.WriteLine("Invalid input. Please enter a valid long number.");
        }
    }

    public static string GetValidString()
    {
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter theme name: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            var themeName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(themeName) && !themeName.Any(char.IsDigit))
            {
                themeName = char.ToUpper(themeName[0]) + themeName.Substring(1);
                return themeName;
            }

            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter theme name: {themeName}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid theme name...");
            Console.WriteLine("Press any key to retry or ESCAPE to go back");
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.B)
            {
                return null;
            }
        }
    }

    public static string? GetValidThemeMenu()
    {
        List<string> Themes = ThemeMenuManager.GetAllThemes();
        Themes.Add("No theme");

        while (true)
        {
            string banner = "Choose theme:\n\n";
            var themeName = SelectionPresent.Show(Themes, banner, false).text;
            if(themeName == "No theme") return "0";
            else if (themeName != "")
            {
                return themeName;
            }
            else
            {
                Console.WriteLine("?");
                return null;
            }
        }
    }

    public static int? GetValidMonth(string prompt)
    {
        int result;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (input == "q")
                return null;

            if (int.TryParse(input, out result) && result >= 1 && result <= 12)
            {
                return result;
            }
            Console.WriteLine("Invalid input. Please enter a month between 1 and 12.");
        }
    }
}