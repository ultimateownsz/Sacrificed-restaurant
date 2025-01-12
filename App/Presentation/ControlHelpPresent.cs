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
        string? feedbackMessage = null)  // pass the recalculated footer start line
        
    {
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
            
            if (string.IsNullOrWhiteSpace(selectedOption)) return;  // Skip empty or newline-only options
            
            string cleanOption = selectedOption.Replace("\n", "").Trim();  // Remove newlines and trim spaces

            if (navigationControls.TryGetValue("Select", out var selectKey))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Press {selectKey} to select \"{cleanOption}\".");
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

    /// <summary>
    /// Displays feedback to the user in the console, with different colors for errors, tips, and success messages.
    /// </summary>
    /// <param name="message">The feedback message to display.</param>
    /// <param name="position">Where the feedback should be displayed ("top", "center", or "bottom").</param>
    /// <param name="feedbackType">The type of feedback. Options are "error", "tip", or "success".</param>
    /// <param name="delayMS">Optional delay in milliseconds for how long the message should be displayed.</param>
    public static void DisplayFeedback(
        string message,
        string position = "bottom",
        string feedbackType = "error", // Default is "error"
        int? delayMS = null) // new parameter to differentiate between errors and tips
    {
        // the message is not null or empty
        if (string.IsNullOrWhiteSpace(message)) return;

        int startLine = position.ToLower() switch
        {
            "top" => 0,
            "center" => Console.WindowHeight / 2,
            _ => Console.WindowHeight - 2, // Default to bottom
        };

        // clear the feedback area
        ClearFeedbackSpace(startLine);

        // Determine the color based on feedback type
        ConsoleColor textColor = feedbackType.ToLower() switch
        {
            "tip" => ConsoleColor.Cyan,
            "success" => ConsoleColor.Green,
            _ => ConsoleColor.Red, // Default to error
        };


        // Safely display the message with feedback type
        Console.SetCursorPosition(0, startLine);
        Console.ForegroundColor = textColor; // Set the foreground color

        // Unicode U+200B displays a zero-width space to prevent line wrapping
        Console.WriteLine($"\u200B{feedbackType.ToUpper()}: {message}");
        Console.ResetColor(); // Reset the color to default

        // pause for the specified delay
        delayMS = 2000;
        if (delayMS > 0)
        {
            Task.Delay(delayMS ?? 2000).Wait();  // blocking delay
        }
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