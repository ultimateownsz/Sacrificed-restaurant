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
                // read input and handle Escape key
                input = ReadInputWithEscapeKey();
                if (input == null)  // escape key pressed
                {
                    Console.WriteLine("\nInput process was canceled.");
                    throw new OperationCanceledException("Input process was canceled by pressing 'Escape' key.");
                }
            }
            catch (OperationCanceledException)
            {
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
                return result;  // return valid result
            }
            // display error message
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid input: {error}");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Reads user input and detects the Escape key.
    /// Prevents accidental blank lines when pressing Enter.
    /// Returns null if the Escape key is pressed.
    /// </summary>
    private static string? ReadInputWithEscapeKey()
    {
        var inputBuilder = new System.Text.StringBuilder();
        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    return null;  // escape key pressed - exit
                
                case ConsoleKey.Enter:
                    if (inputBuilder.Length > 0)
                    {
                        Console.WriteLine();  // only move to the next line if there is input
                        return inputBuilder.ToString();
                    }
                    Console.Beep();  // beep to indicate no input (optional)
                    continue;  // precent unnecessary blank Enter

                case ConsoleKey.Backspace:
                    if (inputBuilder.Length > 0)
                    {
                        inputBuilder.Remove(inputBuilder.Length - 1, 1);
                        Console.Write("\b \b");  // remove last character visually
                    }
                    continue;
                
                default:
                    if (!char.IsControl(keyInfo.KeyChar))  // ignore control keys
                    {
                        inputBuilder.Append(keyInfo.KeyChar);
                        Console.Write(keyInfo.KeyChar);
                    }
                    break;
            }
        }
    }
        // old code 
            // if (keyInfo.Key == ConsoleKey.Escape)
            // {
            //     // throw new OperationCanceledException("Escape key pressed. User canceled input.");
            //     return null;  // escape key detected
            // }

            // if (keyInfo.Key == ConsoleKey.Enter)
            // {
            //     Console.WriteLine();  // move to the next line after Enter
            //     break;
            // }
            
            // if (keyInfo.Key == ConsoleKey.Backspace && inputBuilder.Length > 0)
            // {
            //     inputBuilder.Remove(inputBuilder.Length - 1, 1);
            //     Console.WriteLine("\b \b");  // handle backspace for console
            // }
            // else if (!char.IsControl(keyInfo.KeyChar))
            // {
            //     inputBuilder.Append(keyInfo.KeyChar);
            //     Console.Write(keyInfo.KeyChar);
            // }
        // }
        // return inputBuilder.ToString();

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