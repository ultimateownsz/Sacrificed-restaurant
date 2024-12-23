public static class MenuHelperPresent
{   
    // Dictionary for key-to-action mappings (static controls)
    private static readonly Dictionary<string, string> StaticControls = new()
    {
        { "Navigate", "<arrows>" },
        { "Select", "<enter>" },
        { "Exit", "<escape>" }
    };
    
    public static void SelectionHelp(List<string> options, int? selectedIndex)
    {
        // Calculate the footer start position
        int startLine = Console.WindowHeight - (StaticControls.Count + 4);  // addional margin

        // Clear the space before displaying new controls
        ClearFooterSpace(startLine, Console.WindowHeight);
        Console.SetCursorPosition(0, startLine);
        
        // add dynamic guidance for the currently selected option
        Console.WriteLine("HELP:\n");
        if (selectedIndex.HasValue && selectedIndex.Value >= 0 && selectedIndex.Value < options.Count)
        {
            string currentOption = options[selectedIndex.Value];
            Console.WriteLine($"Current Option: {currentOption}");
            Console.WriteLine($"Press <enter> to select {currentOption}.");
        }
        else
        {
            Console.WriteLine($"No valid option selected.");
        }
    }

    public static void Show()
    {
        Console.WriteLine("\nHELP:\n");
        foreach (var control in StaticControls)
        {
            Console.WriteLine($"{control.Key,-10}: {control.Value}");
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