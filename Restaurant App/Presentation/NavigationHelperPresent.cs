using Dapper;

public static class NavigationHelperPresent
{   
    // Dictionary for key-to-action mappings (static controls)
    private static readonly Dictionary<string, string> navigationControls = new()
    {
        { "Navigate", "<arrows>" },
        { "Select", "<enter>" },
        { "Exit", "<escape>" },
        { "Previous", "<p>"},
        { "Next", "<n>"},
        { "Reset", "<r>"}
    };

    public static void AddOptions(string action, string key)
    {
        navigationControls[action] = key; // This should add or update the key-value pair
    }

    public static void Reset()
    {
        navigationControls.Clear();
        navigationControls.Add("Navigate", "<arrows>");
        navigationControls.Add("Select", "<enter>");
        navigationControls.Add("Exit", "<escape>");
    }
    
    // public static void SelectionHelp(List<string> options, int? selectedIndex)
    // {
    //     // Calculate the footer start position
    //     int footerHeight = StaticControls.Count + 4;  // show number of controls + margin
    //     int startLine = Console.WindowHeight - footerHeight;

    //     // Clear the space before displaying new controls
    //     ClearFooterSpace(startLine, Console.WindowHeight);
    //     Console.SetCursorPosition(0, startLine);

    //     // add dynamic guidance for the currently selected option
    //     Console.WriteLine("HELP:\n");
    //     if (selectedIndex.HasValue && selectedIndex.Value >= 0 && selectedIndex.Value < options.Count)
    //     {
    //         var selectedOption = options[selectedIndex.Value];

    //         // fetch the "select" action from the dictionary
    //         if (StaticControls.TryGetValue("Select", out var selectKey))
    //         {
    //             Console.WriteLine($"Press {selectKey} to select \"{selectedOption}\".");
    //         }
    //     }
    //     else
    //     {
    //         Console.WriteLine($"No valid option selected.");
    //     }
    // }

    // public static void ShowHelp(string? actionKey = null)
    // {
        
    //     // Calculate the footer start position
    //     int footerHeight = StaticControls.Count + 4; // Number of controls + margin
    //     int startLine = Console.WindowHeight - footerHeight;

    //     // Clear the space before displaying new controls
    //     ClearFooterSpace(startLine, Console.WindowHeight);

    //     // Move the cursor to the start of the footer
    //     Console.SetCursorPosition(0, startLine);

    //     // Calculate dynamic padding for alignment
    //     int maxKeyLength = StaticControls.Keys.Max(key => key.Length) + 2; // Add padding for spacing
    //     int maxValueLength = StaticControls.Values.Max(value => value.Length);

    //     Console.WriteLine("\nHELP:\n");

    //    if (!string.IsNullOrEmpty(actionKey) && StaticControls.TryGetValue(actionKey, out var controlValue))
    //     {
    //         // Show only the specific action if a valid key is provided
    //         Console.WriteLine($"Press {controlValue} to {actionKey.ToLower()}.");
    //     }
    // }

    public static void ShowHelp(
        List<string>? options = null,
        int? selectedIndex = null,
        string? feedbackMessage = null)
    {
        // Display feedback if provided
        if (!string.IsNullOrWhiteSpace(feedbackMessage))
        {
            DisplayFeedback(feedbackMessage);
        }
        
        // Calculate the footer start position
        int footerHeight = GetFooterHeight();
        int startLine = Console.WindowHeight - footerHeight;

        ClearFooterSpace(startLine, Console.WindowHeight);
        Console.SetCursorPosition(0, startLine);

        Console.WriteLine("\nHELP:\n");

        // if (actionKeys != null && actionKeys.Any())
        // {
        //     // Display each actionKey that exists in the dictionary
        //     foreach (var actionKey in actionKeys)
        //     {
        //         if (navigationControls.TryGetValue(actionKey, out var controlValue))
        //         {
        //             Console.WriteLine($"Press {controlValue} to {actionKey.ToLower()}.");
        //         }
        //     }
        // }
        // // Show only a specific action if actionKey is provided
        // if (!string.IsNullOrEmpty(actionKey) && navigationControls.TryGetValue(actionKey, out var controlValue))
        // {
        //     // Console.WriteLine($"Selected option: {actionKey}");
        //     Console.WriteLine($"Press {controlValue} to {actionKey.ToLower()}.");
        // }
        if (selectedIndex.HasValue && options != null && selectedIndex.Value >= 0 && selectedIndex.Value < options.Count)
        {
            // Show only if selection menu is used
            string selectedOption = options[selectedIndex.Value];
            if (navigationControls.TryGetValue("Select", out var selectKey))
            {
                Console.WriteLine($"Press {selectKey} to select {selectedOption}.");
            }
            if (navigationControls.TryGetValue("Navigate", out var navKey))
            {
                Console.WriteLine($"Press {navKey} to navigate options.");
            }
            if (navigationControls.TryGetValue("Exit", out var exitKey))
            {
                Console.WriteLine($"Press {exitKey} to exit.");
            }
        }
        else
        {
            // Show all controls if no specific actionKey is provided
            foreach (var control in navigationControls.OrderBy(c => c.Key))
            {
                Console.WriteLine($"Press {control.Value} to {control.Key.ToLower()}.");
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
        Console.WriteLine($"FEEDBACK: {message}");
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