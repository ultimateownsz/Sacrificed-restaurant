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

    public static string GetValidString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            Console.WriteLine("Invalid input. Please enter a non empty string.");
        }
    }

    public static int ValidateYear()
    {
        int result;
        int minYear = DateTime.Now.Year;
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter year: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            var year = Console.ReadLine();

            if (int.TryParse(year, out result) && result >= minYear)
            {
                return result;
            }
            
            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter year: {year}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid year...");
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