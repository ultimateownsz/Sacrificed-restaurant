namespace Project;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks.Dataflow;

internal class SelectionPresent : SelectionLogic
{
    // updates the screen with the given banner and the current selection state.
    // highlights the currently selected item.
    private static void _update(string banner, Dictionary<string, bool> selection, bool oneline)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(banner, Console.ForegroundColor);

        foreach ((string text, bool selected) in selection)
        {
            Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = (selected) ? "-> " : "";

            if (oneline && !selected) continue;  // skip unselected item in one-line mode.
            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
        }
    }

    // reads user input and updates the selection or handles special keys like Enter or Escape
    private static Tuple<string?, int?>? _read(Dictionary<string, bool> selection)
    {
        var current = ReverseLookup<string, bool>(selection, true);

        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine($"Key pressed: {key}");  // Debugging for key press

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.DownArrow:

                selection[current.Item1 ?? ""] = false;
                selection[selection.ElementAt(Next(selection.Count, current.Item2)).Key] = true;
                break;

            case ConsoleKey.UpArrow:

                selection[current.Item1 ?? ""] = false;
                selection[selection.ElementAt(Next(selection.Count, current.Item2, true)).Key] = true;
                break;

            case ConsoleKey.Enter:

                Console.ForegroundColor = ConsoleColor.White;
                return new(current.Item1, current.Item2);
            
            case ConsoleKey.Escape:
                // handle escape functionality
                var escapeResult = HandleEscape(() =>
                {
                    Console.WriteLine("Returning to previous selection menu");
                    return null;
                });

                if (escapeResult == null)
                {
                    return null;
                }
                break;

            default:
                Console.WriteLine("Unhandled key. Ignoring...");
                break;
        }

        return null;
    }

    // displays a selection menu to the user, handles navigation, and captures user choices.
    public static dynamic Show(List<string> options, string banner = "", bool oneline = false)
    {
        Tuple<string?, int?>? selected;
        if (oneline) options.Reverse();  // reverse options for one-line mode.
        
        Dictionary<string, bool> selection = ToSelectable(options, oneline);

        while (true)
        {
            // update screen with the banner and options
            _update(banner, selection, oneline);

            // read user-input
            selected = _read(selection);
            if (selected != null)
            {
                // return the selected item as dynamic object
                dynamic dynamicHandle = new ExpandoObject();
                dynamicHandle.text = selected.Item1;
                dynamicHandle.index = selected.Item2;

                // return
                return dynamicHandle;
            }
        }
    }

    public static string? HandleInputWithEscape()
    {
        StringBuilder inputBuffer = new StringBuilder();

        while (true)
        {
            var keyInfo = Console.ReadKey(intercept: true); // Read a single key without displaying it

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine(); // Move to the next line when Enter is pressed
                return inputBuffer.ToString(); // Return the input string
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && inputBuffer.Length > 0)
            {
                inputBuffer.Remove(inputBuffer.Length - 1, 1); // Remove the last character
                Console.Write("\b \b"); // Simulate backspace
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
            {
                // Handle Escape key
                if (EscapeKeyIsPressed())
                {
                    return null; // Return null to indicate Escape was confirmed
                }
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.DownArrow ||
                    keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.RightArrow)
            {
                // Ignore arrow keys
            }
            else
            {
                // Add valid characters to the input buffer
                inputBuffer.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar); // Display the character
            }
        }
}


    /// <summary>
/// Handles the logic for the Escape key, including optional additional actions.
/// </summary>
/// <param name="action">
/// An optional action provided as a lambda function or delegate that will be executed 
/// if the Escape key is not confirmed by the user. This allows the method to handle 
/// escape-specific behavior while also deferring other logic back to the caller.
/// </param>
/// <returns>
/// Returns null if the Escape key is pressed and confirmed (indicating the user wants to exit or go back). 
/// Otherwise, it executes and returns the result of the provided action (if any).
/// </returns>
/// <remarks>
/// This method centralizes Escape key handling logic, ensuring consistent behavior throughout the application. 
/// It relies on <see cref="EscapeKeyIsPressed"/> to detect the Escape key and confirm the user's intent to exit.
/// If the user cancels the escape prompt, the optional action (provided as a lambda function or delegate) 
/// will be executed instead.
/// </remarks>
    public static dynamic HandleEscape(Func<dynamic>? action)
    {
        // if escape is pressed handle the escape func
        if (EscapeKeyIsPressed())
        {
            Console.WriteLine("Escape pressed, returning to previous menu.");  // debugging message
            return null;
        }

        if (action == null)
        {
            Console.WriteLine("No action provided to HandleEscape");  // debugging message
        }

        return action.Invoke();
    }

    /// <summary>
    /// Detects if the Escape key is pressed and displays a confirmation prompt.
    /// </summary>
    /// <returns>
    /// Returns true if the Escape key is pressed and the user confirms their intent to exit. 
    /// Returns false if the Escape key is not pressed or the user cancels the action.
    /// </returns>
    /// <remarks>
    /// This method is designed to work with menus and workflows where exiting or navigating back 
    /// requires user confirmation to prevent accidental exits.
    /// </remarks>
    private static bool EscapeKeyIsPressed()
    {
        if (!Console.KeyAvailable) return false;

        var keyInfo = Console.ReadKey(intercept: true);
        Console.WriteLine($"Key pressed: {keyInfo.Key}");
        if (keyInfo.Key == ConsoleKey.Escape)
        {
            var confirmation = SelectionPresent.Show(
                new List<string> { "yes", "no" },
                "Are you sure you want to exit?\n]\n"
            );

            Console.WriteLine($"User selected: {confirmation.text}");  // debugging message
            return confirmation.text == "yes";
        }
        return false;
    }
}
