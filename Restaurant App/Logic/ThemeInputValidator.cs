// the presentation layer uses this class to validate the input of the user
using Project;
using Project.Presentation;

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

    public static string? GetValidString()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            var themeName = Terminable.ReadLine("Enter theme name: ");
            if (themeName == null) return null;

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

        while (true)
        {
            List<string> Themes = ThemeMenuManager.GetAllThemes();
            var themeName = SelectionPresent.Show(Themes, banner: "Choose theme:").ElementAt(0).text;
            
            if(themeName == null)
                return "REQUEST_PROCESS_EXIT";
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