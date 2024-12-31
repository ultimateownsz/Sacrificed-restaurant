using Dapper;

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
        string? feedbackMessage = null)  // pass the recalculated footer start line
    {
        // if (footerStartLine == -1)
        // {
        //     footerStartLine = Console.WindowHeight - GetFooterHeight();  // default position
        // }

        // ClearFooterSpace(footerStartLine, Console.WindowHeight);
        // Console.SetCursorPosition(0, footerStartLine);
        
        // Display feedback if provided
        if (!string.IsNullOrWhiteSpace(feedbackMessage))
        {
            DisplayFeedback(feedbackMessage);
        }

        // Preprocess options to strip out newlines
        if (options != null)
        {
            options = options.Select(option => option.Replace("\n", " ")).ToList();
        }

        // Split navigationControls into dynamic and static controls
        var dynamicControls = navigationControls
        .Where(c => c.Key != "Navigate" && c.Key != "Select" && c.Key != "Exit")
        .OrderBy(c => c.Key)
        .ToList();

        var staticControls = navigationControls
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
            if (navigationControls.TryGetValue("Select", out var selectKey))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Press {selectKey} to select \"{selectedOption}\".");
                Console.ResetColor();
            }
            if (navigationControls.TryGetValue("Navigate", out var navKey))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Press {navKey} to navigate options.");
            }
            if (navigationControls.TryGetValue("Exit", out var exitKey))
            {
                Console.WriteLine($"Press {exitKey} to exit.");
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

    // this method is used to show direct feedback to the user what went wrong, it is shown below in the terminal too
    public static void DisplayFeedback(string message, string position = "bottom", int delayMS = 2000)
    {
        // the message is not null or empty
        if (string.IsNullOrWhiteSpace(message))
            return;

        // calculate the start line based on the position
        int startLine = position.ToLower() == "top" ? 0 : Console.WindowHeight - 2;

        // clear the feedback area
        ClearFeedbackSpace(startLine);

        // Set cursor position and display the message
        Console.SetCursorPosition(0, startLine);
        Console.ForegroundColor = ConsoleColor.Red; // Highlight feedback in red
        Console.WriteLine($"MESSAGE: {message}");
        Console.ResetColor(); // Reset the color to default

        // pause for the specified delay
        if (delayMS > 0)
        {
            Task.Delay(delayMS).Wait();  // blocking delay
        }
    }

    private static void ClearFeedbackSpace(int startLine)
    {
        // check if start line is within bounds
        startLine = Math.Max(0, startLine);
        int endLine = Math.Min(Console.WindowHeight, startLine + 2); // feedback occupies two lines max
    
        // clear the feedback space
        for (int i = startLine; i < endLine; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
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