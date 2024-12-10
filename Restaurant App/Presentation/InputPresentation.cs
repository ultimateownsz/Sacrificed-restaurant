public static class InputHelper
{
    /// <summary>
    /// Handles generic user input with validation.
    ///</summary>
    public static T GetValidatedInput<T>(
        string prompt,
        Func<string, (T? result, string? error)> validateAndParse)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input;
            try
            {
                // read input with Escape key
                input = ReadInputWithEscapeKey();
                if (input == null)  // escape key pressed
                {
                    Console.WriteLine("\nInput process was canceled by pressing Escape.");
                    throw new OperationCanceledException("Input process was canceled by pressing Escape.");
                    // return default;
                }
            }
            catch (OperationCanceledException)
            {
                // Console.WriteLine("\nInput process was canceled.");
                // return default;
                throw;
            }
            

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Invalid input: Input cannot be null or empty.");
                continue;
            }

            var (result, error) = validateAndParse(input);
            if (error == null && result != null)
            {
                return result;
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid input: {error}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Reads user input and detects the Escape key.
    /// Returns null if the Escape key is pressed.
    /// </summary>
    private static string? ReadInputWithEscapeKey()
    {
        var inputBuilder = new System.Text.StringBuilder();
        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true);
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                // throw new OperationCanceledException("Escape key pressed. User canceled input.");
                return null;  // escape key detected
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();  // move to the next line after Enter
                break;
            }
            
            if (keyInfo.Key == ConsoleKey.Backspace && inputBuilder.Length > 0)
            {
                inputBuilder.Remove(inputBuilder.Length - 1, 1);
                Console.WriteLine("\b \b");  // handle backspace for console
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                inputBuilder.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }
        return inputBuilder.ToString();
    }

    /// <summary>
    /// Validates that the input is not null or empty and provides a generic error message.
    /// </summary>
    public static (string? result, string? error) InputNotNull(string input, string fieldName)
    {
        return string.IsNullOrWhiteSpace(input)
            ? (null, $"{fieldName} cannot be empty.")
            : (input, null);
    }
}