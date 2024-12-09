public static class InputPresentation
{
    /// <summary>
    /// Handles generic user input with validation.
    ///</summary>
    public static T GetValidatedInput<T>(
        string prompt,
        Func<string, (T? result, string? error)> validateAndParse)
        // where T : struct
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

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
            Console.WriteLine($"Invalid input: {error}");
        }
    }

    public static bool CheckForQuit(string input, string quitKeyword)
    {
        return input.Trim().ToLower() == quitKeyword.ToLower();
    }
}