public static class MenuHelperPresent
{   
    public static void Show(List<string> options)
    {
        // // Determine where to position the controls
        int startLine = Console.WindowHeight - (options.Count + 6);  // addional margin
        Console.SetCursorPosition(0, startLine);

        // Clear the space before displaying new controls
        ClearFooterSpace(startLine, Console.WindowHeight);

        Console.SetCursorPosition(0, startLine);
        Console.WriteLine("\nOPTIONS:");
        foreach (var option in options)
        {
            Console.WriteLine($"{option[0]}     : <arrows>");
            Console.WriteLine($"{option[1]}     : <enter>");
            Console.WriteLine($"{option[3]}     : <escape>");
        }
    }

    private static void ClearFooterSpace(int startLine, int endLine)
    {
        int currentLine = Console.CursorTop;
        for (int i = startLine; i < endLine; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));  // fill line with spaces
        }
        Console.SetCursorPosition(0, currentLine);  // return cursor to original position
    }
}