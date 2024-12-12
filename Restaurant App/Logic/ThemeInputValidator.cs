// the presentation layer uses this class to validate the input of the user

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
                return themeName;
            }
            
            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter theme name: {themeName}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid theme name...");
            Console.ReadKey();
        }
    }

    public static int ValidateYear()
    {
        int result;
        int minYear = DateTime.Now.Year;
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"Please note: You can only enter a year from {minYear} onwards for future themes.\nHowever, you can still view months in 2024 with themes that were already made in the past.\n\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter year (or type (b)ack to exit): ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;

            var year = Console.ReadLine().ToLower();

            if(year == "b" || year == "back")
            {
                return -1;
            }
            else if (int.TryParse(year, out result) && result >= minYear)
            {
                return result;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter year: {year}", Console.ForegroundColor);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nThemes can only be made for the year 2024 and beyond. Please try again....");
            Console.ReadKey();
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