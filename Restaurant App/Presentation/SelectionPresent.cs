namespace Project;
using System.Dynamic;
using System.Reflection;

internal class SelectionPresent : SelectionLogic
{
<<<<<<< HEAD
<<<<<<< HEAD
    public struct Palette()
    {
        public ConsoleColor Primary    = ConsoleColor.Yellow;
        public ConsoleColor Secondary  = ConsoleColor.DarkYellow;
        public ConsoleColor Tertiary   = ConsoleColor.DarkYellow;
        public ConsoleColor Base       = ConsoleColor.White;
    }
    private static Palette palette = new Palette();

    private static void _display(Dictionary<string, SelectionLogic.Selectable> selection,
        string banner, SelectionLogic.Mode mode, int menuStartLine)
    {

        // banner & colour initialization
=======
    private static void _update(string banner, Dictionary<string, bool> selection, bool oneline, int menuStartLine)
    {
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)
=======
    private static void _update(string banner, Dictionary<string, bool> selection, bool oneline, int menuStartLine)
    {
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)
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
<<<<<<< HEAD
<<<<<<< HEAD
            
            // conditional statements for method-complexity
            if (mode == SelectionLogic.Mode.Scroll && !selectable.selected) continue;
            if ((index == selection.Count() - 1) && mode == SelectionLogic.Mode.Multi) 
                Console.WriteLine();
=======
=======
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)

            // print the current option
            Console.ForegroundColor = selected ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = selected ? "-> " : "";

            if (oneline && !selected) continue;
            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
<<<<<<< HEAD
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)
=======
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)
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
                return new(null, -1); // Return null text and -1 index for Escape
        }

        return null;
    }

    public static dynamic Show(List<string> options, string banner = "", bool oneline = false)
    {
        Tuple<string?, int?>? selected;
        if (oneline) options.Reverse();
        
        Dictionary<string, bool> selection = ToSelectable(options, oneline);

        int lastWindowHeight = Console.WindowHeight;  // track the initial terminal height
        int reservedLines = ControlHelpPresent.GetFooterHeight();

        while (true)
        {
             // Always render at the top of the terminal
            int menuStartLine = 0; // Fixed start at the top
            Console.SetCursorPosition(0, menuStartLine);

            // update screen
            _display(selection, banner, mode, menuStartLine);

            // determine the currently selected index
            int? selectedIndex = null;
            foreach (var item in selection)
            {
                if (item.Value.selected)  // the selected option
                {
                    selectedIndex = options.IndexOf(item.Key);
                    break;
                }
            }

            // Show help section with dynamic feedback for the selected option
            ControlHelpPresent.ShowHelp(options, selectedIndex);

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

<<<<<<< HEAD
<<<<<<< HEAD
                    // interrupt and prevent nest
                    selection.Where(x => x.Value.selected == true);
                    // if (.ElementAt(0).Key != "continue")
                    //     break;

                    // prepare reading-phase
                    var list = new List<SelectionLogic.Selection>();
                    selection.Remove("continue");

                    // read highlighted input
                    foreach (var selectable in selection.Where(x => x.Value.highlighted == true))
                    {
                        list.Add(new SelectionLogic.Selection()
                        {
                            text = selectable.Key,
                            index = selectable.Value.index
                        });
                    }

                    Console.Clear();
                    return list;

                case SelectionLogic.Interaction.Moved:
                    break;

                case SelectionLogic.Interaction.Selected:

                    Console.Clear();
                    selection.Where(x => x.Value.selected == true);
                    return new()
                    {
                        new SelectionLogic.Selection()
                        {
                            text = selection.Select(x => x.Key).ElementAt(0),
                            index = selection.Select(x => x.Value.index).ElementAt(0)
                        }
                    };

                case SelectionLogic.Interaction.Terminated:

                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("WARNING\n");

                    Console.ForegroundColor = palette.Base;
                    Console.WriteLine(
                        "You are about to attempt a menu termination,\n"+
                        "however this functionality has been rather \n"+
                        "buggy due to our retarded ahh approach in\n"+
                        "making the most non-modular code imaginable.\n\n"
                        );

                    Console.Write("Would you like to proceed? [might cause a crash] (y/N)");
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'y':
                            return new();

                        default:
                            continue;
                    }
=======
                // Trim the selection text to handle the arrow keys and logic
                string trimmedSelection = selected.Item1?.Trim() ?? "";
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)
=======
                // Trim the selection text to handle the arrow keys and logic
                string trimmedSelection = selected.Item1?.Trim() ?? "";
>>>>>>> parent of 35a7e76 (Merge branch 'main' into making-menu's-consistent)

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