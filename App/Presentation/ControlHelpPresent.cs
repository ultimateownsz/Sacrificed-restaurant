namespace Restaurant;

public static class ControlHelpPresent
{   
    // default navigation controls
    private static readonly Dictionary<string, string> defaultNavigationControls = new()
    {
        { "Navigate", "<arrows>" },
        { "Select", "<enter>" },
        { "Exit", "<escape>" }
    };

    // Active navigation controls (modifiable)
    private static Dictionary<string, string> navigationControls = new(defaultNavigationControls);

    // Add options to the navigation controls
    public static void AddOptions(string action, string key)
    {
        navigationControls[action] = key; // This should add or update the key-value pair
    }

    public static void Clear()
    {
        navigationControls.Clear();
    }

    // Reset to default options
    public static void ResetToDefault()
    {
        navigationControls = new Dictionary<string, string>(defaultNavigationControls);
    }


    internal static void ShowHelp(
        List<string>? options = null,
        int? selectedIndex = null,
        string? feedbackMessage = null,
        string menuContext = "default") // "admin, "user
    {
        bool showExit = menuContext != "admin" && menuContext != "user";

        var filteredControls = navigationControls
        .Where(c => showExit || c.Key != "Exit")
        .ToList();

        // Display feedback if provided
        // if (!string.IsNullOrWhiteSpace(feedbackMessage))
        // {
        //     DisplayFeedback(feedbackMessage);
        // }

        // // Preprocess options to strip out newlines
        // if (options != null)
        // {
        //     options = options.Select(option => option.Replace("\n", " ")).ToList();
        // }

        ShowControls(filteredControls, options, selectedIndex);
    }

    private static void ShowControls(
        List<KeyValuePair<string, string>> controls,
        List<string>? options = null,
        int? selectedIndex = null)
    {

        // Split navigationControls into dynamic and static controls
        var dynamicControls = controls
        .Where(c => c.Key != "Navigate" && c.Key != "Select" && c.Key != "Exit")
        .OrderBy(c => c.Key)
        .ToList();

        var staticControls = controls
        .Where(c => c.Key == "Navigate" || c.Key == "Select" || c.Key == "Exit")
        .ToList();
        
        // Calculate the footer start position
        int footerHeight = GetFooterHeight();
        int startLine = Console.WindowHeight - footerHeight;

        ClearFooterSpace(startLine, Console.WindowHeight);
        Console.SetCursorPosition(0, startLine);

        Console.ForegroundColor = ConsoleColor.White;
        Console.ResetColor();

        if (selectedIndex.HasValue && options != null && selectedIndex.Value >= 0 && selectedIndex.Value < options.Count)
        {
            // Show only if selection menu is used
            string selectedOption = options[selectedIndex.Value];
            
            if (string.IsNullOrWhiteSpace(selectedOption)) return;  // Skip empty or newline-only options
            
            string cleanOption = selectedOption.Replace("\n", "").Trim();  // Remove newlines and trim spaces

            if (controls.Any(c => c.Key == "Select"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Press {navigationControls["Select"]} to select \"{cleanOption}\".");
                Console.ResetColor();
            }
            if (controls.Any(c => c.Key == "Navigate"))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Press {navigationControls["Navigate"]} to navigate options.");
                Console.WriteLine($"Press <esc> to return.");
            }
            if (controls.Any(c => c.Key == "Exit"))
            {
                Console.WriteLine($"Press {navigationControls["Exit"]} to return.");
                Console.ResetColor();
            }
        }
        else
        {
            // Show all controls if no specific actionKey is provided
            // Show dynamic controls first
            foreach (var control in dynamicControls)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Press {control.Value} to {control.Key.ToLower()}.");
                Console.ResetColor();
            }

            // Followed by static controls
            foreach (var control in staticControls)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Press {control.Value} to {control.Key.ToLower()}.");
                Console.ResetColor();
            }
        }
    }

    public static int GetFooterHeight()
    {
        return navigationControls.Count + 4; // Number of controls + header + margin
    }

    private static void ClearFeedbackSpace(int startLine)
    {
        // check if start line is within bounds
        startLine = Math.Max(0, startLine);


        int linesToClear = GetFooterHeight(); // feedback occupies two lines max
        int endLine = Math.Min(Console.WindowHeight, startLine + linesToClear); // feedback occupies two lines max
    
        // clear the feedback space
        for (int i = startLine; i < endLine; i++)
        {
            // safely move the cursor to the start of the line
            Console.SetCursorPosition(0, i);

            // Clear the line by writing spaces across the console width
            Console.Write(new string(' ', Console.WindowWidth));
        }
        // Reset the cursor to the start of the first cleared line
        Console.SetCursorPosition(0, startLine);
    }

    private static void ClearFooterSpace(int startLine, int endLine)
    {
        // Ensure the start and end lines are within bounds
        startLine = Math.Max(0, startLine);
        endLine = Math.Min(Console.WindowHeight, endLine);

        int currentLine = Console.CursorTop;
        for (int i = startLine; i < endLine; i++)
        {
            Console.SetCursorPosition(0, i); // Safeguard to avoid invalid positions
            Console.Write(new string(' ', Console.WindowWidth)); // Clear the line
        }
        Console.SetCursorPosition(0, Math.Min(currentLine, Console.WindowHeight - 1)); // Return cursor to a valid position
    }
}