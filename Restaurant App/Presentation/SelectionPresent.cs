namespace Project;
using System.Dynamic;
using System.Reflection;

internal class SelectionPresent : SelectionLogic
{
    private static void _update(string banner, Dictionary<string, bool> selection, bool oneline, int menuStartLine)
    {
        Console.Clear();
        // clear the menu selection area
        ClearMenuArea(menuStartLine, selection.Count);

        // Move the cursor to the menu start line (top of the terminal)
        Console.SetCursorPosition(0, menuStartLine);

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(banner);  // Display the banner

        foreach ((string text, bool selected) in selection)
        {
            // Clear the current line completely to prevent residual text
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);

            // print the current option
            Console.ForegroundColor = selected ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = selected ? "-> " : "";

            if (oneline && !selected) continue;
            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
        }
        Console.ResetColor();
        // Console.WriteLine("\nControls:\nNavigate : <arrows>\nSelect   : <enter>\nExit     : <escape>");
    }

    private static Tuple<string?, int?>? _read(Dictionary<string, bool> selection)
    {
        var current = ReverseLookup<string, bool>(selection, true);

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
            
            // hmm.. somebody toucha ma code, and destabilized it
            // I won't touch it for now, but I will find you, and I will kill you.
            case ConsoleKey.Escape:
            // case ConsoleKey.B:

                Console.ForegroundColor = ConsoleColor.White;
                return new("", -1);
        }

        return null;
    }

    public static dynamic Show(List<string> options, string banner = "", bool oneline = false)
    {
        Tuple<string?, int?>? selected;
        if (oneline) options.Reverse();
        
        Dictionary<string, bool> selection = ToSelectable(options, oneline);

        int lastWindowHeight = Console.WindowHeight;  // track the initial terminal height
        int reservedLines = ControlsHelperPresent.GetFooterHeight();

        while (true)
        {
             // Always render at the top of the terminal
            int menuStartLine = 0; // Fixed start at the top
            Console.SetCursorPosition(0, menuStartLine);

            // update screen
            _update(banner, selection, oneline, menuStartLine);

            // determine the currently selected index
            int? selectedIndex = null;
            foreach (var item in selection)
            {
                if (item.Value)  // the selected option
                {
                    selectedIndex = options.IndexOf(item.Key);
                    break;
                }
            }

            // Show help section with dynamic feedback for the selected option
            ControlsHelperPresent.ShowHelp(options, selectedIndex);

            if ((selected = _read(selection)) != null)
            {
                if (selected.Item2 == -1)  // escape pressed
                {
                    // Return a dynamic object indicating Escape was pressed
                    dynamic escapeHandle = new ExpandoObject();
                    escapeHandle.text = null;
                    escapeHandle.index = -1;
                    return escapeHandle;
                }

                // Trim the selection text to handle the arrow keys and logic
                string trimmedSelection = selected.Item1?.Trim() ?? "";

                // initialize and return dynamic object for selection
                dynamic dynamicHandle = new ExpandoObject();
                dynamicHandle.text = trimmedSelection;  // return trimmed value for logic
                dynamicHandle.index = selected.Item2;
                return dynamicHandle;
            }
        }
    }

    // clear only the menu area
    private static void ClearMenuArea(int menuStartLine, int menuHeight)
    {
        int endLine = Math.Min(menuStartLine + menuHeight, Console.WindowHeight); // Avoid going out of bounds
        for (int i = menuStartLine; i < endLine; i++)
        {
            // if (i >= Console.WindowHeight) break; // prevent out of bounds
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth)); // clear the line
        }
        Console.SetCursorPosition(0, menuStartLine); // Reset cursor to menu start line
    }
}