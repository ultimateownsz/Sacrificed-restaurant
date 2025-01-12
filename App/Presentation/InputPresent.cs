using System.Runtime.InteropServices;
using System.Xml.XPath;

public static class InputHelper
{
    /// <summary>
    /// Handles generic user input with validation.
    ///</summary>
    public static T GetValidatedInput<T>(
        string prompt,
        Func<string, (T? result, string? error)> validateAndParse,
        int reservedLines = 3,
        string? menuTitle = null,
        Action? showHelpAction = null)
    {
        int maxAttempts = 2;  // allow 2 attempts

        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            // Clear the input area, including space for the menu title
            ClearInputArea(reservedLines);

            // Display the menu title at the top
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(menuTitle);

             // Optionally show the help section
            showHelpAction?.Invoke();

            // Display the prompt at the reserved space
            Console.SetCursorPosition(0, reservedLines);

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
            // Console.ForegroundColor = ConsoleColor.Red;
            // ControlHelpPresent.DisplayFeedback($"Invalid input: {error}");

            // show remaining attempts
            int remainingAttempts = maxAttempts - attempt;
            if (remainingAttempts > 0)
            {
                ControlHelpPresent.DisplayFeedback($"You have {remainingAttempts} attempt{(remainingAttempts > 1 ? "s" : "")} remaining.");
                // Console.ResetColor();
            }
            else
            {
                // Throw exception after last attempt
                ControlHelpPresent.DisplayFeedback("Too many invalid attempts. Operation will now be canceled.");
                Thread.Sleep(1500);
                throw new OperationCanceledException("Too many invalid attempts.");
            }
        }
        throw new OperationCanceledException("Input failed");
    }

    // Clears the input area and reserves space for prompts.
    public static void ClearInputArea(int reservedLines, int startLine = 0, int? preserveStartLine = null, int? preserveEndLine = null)
    {
        for (int i = startLine; i < Console.WindowHeight - reservedLines + 1; i++) // clear reserved lines + input line
        {
            // Skip clearing lines that need to be preserved
            if (preserveStartLine.HasValue && preserveEndLine.HasValue && i >= preserveStartLine && i <= preserveEndLine)
                continue;

            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth)); // clear each line
        }
        Console.SetCursorPosition(0, startLine); // Reset the cursor to the start line
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
                    // Console.Beep();  // beep to indicate no input (optional)
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
            ? (null, $"{fieldName} cannot be empty. Please enter a {fieldName}.")
            : (input, null);
    }
}