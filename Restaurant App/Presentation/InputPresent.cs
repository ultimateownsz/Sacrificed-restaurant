using System.Xml.XPath;

public static class InputHelper
{
    /// <summary>
    /// Handles generic user input with validation.
    ///</summary>
    public static T GetValidatedInput<T>(
        string prompt,
        Func<string, (T? result, string? error)> validateAndParse)
    {
        int maxAttempts = 2;  // allow only one valid entry

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            // display the prompt in yellow
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(prompt);
            Console.ForegroundColor = ConsoleColor.White;

            string? input = ReadInputWithEscapeKey();

            if (input == null)  // escape key pressed
            {
                throw new OperationCanceledException("Input process was canceled by pressing 'Escape' key.");
            }

             // Validate input
            var (result, error) = validateAndParse(input);

            if (error == null && result != null)
            {
                return result; // Return valid input
            }

            // display error message
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid input: {error}");
            Console.ResetColor();

            // show remaining attempts
            int remainingAttempts = maxAttempts - attempt;
            if (remainingAttempts > 0)
            {
                Console.WriteLine($"Attempts remaining: {remainingAttempts}\n");
            }
            else
            {
                // Throw exception after last attempt
                Thread.Sleep(1500);
                throw new OperationCanceledException("Too many invalid attempts.");
            }
        }
        throw new OperationCanceledException("Input failed");
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

    /// <summary>
    /// Validates that the input is not null or empty and provides a generic error message.
    /// Can be used if you don't directly have a method made to validate specifc values, see it as a test method
    /// </summary>
    public static (string? result, string? error) InputNotNull(string input, string fieldName)
    {
        return string.IsNullOrWhiteSpace(input)
            ? (null, $"{fieldName} cannot be empty.")
            : (input, null);
    }
}