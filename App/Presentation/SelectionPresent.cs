namespace Restaurant;
public class SelectionPresent
{

    protected internal struct Palette()
    {
        public ConsoleColor Primary    = ConsoleColor.Yellow;
        public ConsoleColor Secondary  = ConsoleColor.DarkYellow;
        public ConsoleColor Tertiary   = ConsoleColor.DarkYellow;
        public ConsoleColor Base       = ConsoleColor.White;
    }
    protected internal static Palette palette = new Palette();
    private const int TIMEOUT = 0;

    private static void _clear(int menuStartLine, int menuHeight)
    {
        for (int i = menuStartLine; i < menuHeight; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }

    private static void _display(Dictionary<string, SelectionLogic.Selectable> selection,
        string banner, SelectionLogic.Mode mode, int menuStartLine)
    {
        // Clear only the menu area, leaving the footer intact
        _clear(menuStartLine, Console.WindowHeight - ControlHelpPresent.GetFooterHeight());


        // banner & colour initialization
        Console.SetCursorPosition(0, menuStartLine);
        TerminableUtilsPresent.Write(banner + "\n\n");
        Console.ForegroundColor = palette.Base;

        foreach (((string text, SelectionLogic.Selectable selectable), int index) 
            in selection.Select((value, index) => (value, index)))
        {
            // colouring (priority-sensitive)
            Console.ForegroundColor = selectable.selected && selectable.highlighted
                ? palette.Secondary
                : selectable.selected
                ? palette.Primary
                : selectable.highlighted
                ? palette.Tertiary
                : palette.Base;

            // marker
            string prefix = (selectable.selected) ? ">" : "";
            string suffix = (selectable.selected) ? "" : "";
            
            // conditional statements for method-complexity
            if (mode == SelectionLogic.Mode.Scroll && !selectable.selected) continue;
            if ((index == selection.Count() - 1) && mode == SelectionLogic.Mode.Multi) 
                Console.WriteLine();

            // output
            Console.WriteLine($"{prefix} {text} {suffix}");
            Console.ForegroundColor = palette.Base;
        }
    }

    private static SelectionLogic.Interaction _update(Dictionary<string, 
        SelectionLogic.Selectable> selection, SelectionLogic.Mode mode, List<ConsoleKey> keystrokes)
    {
        ConsoleKey capture;
        if (keystrokes.Count > 0)
        {
            Thread.Sleep(TIMEOUT);
            capture = keystrokes[0];
            keystrokes.RemoveAt(0);
        }
        else capture = Console.ReadKey().Key;

        switch (capture)
        {
            // movement
            case ConsoleKey.DownArrow:
            case ConsoleKey.UpArrow:

                // 2D-movement
                SelectionLogic.Iterate(selection,
                    reverse: (capture != ConsoleKey.DownArrow));

                return SelectionLogic.Interaction.Moved;

            // actions
            case ConsoleKey.Escape:
                return SelectionLogic.Interaction.Terminated;

            case ConsoleKey.Spacebar:
            case ConsoleKey.Enter:

                // terminate
                if (mode != SelectionLogic.Mode.Multi)
                    return SelectionLogic.Interaction.Selected;

                // continuous
                SelectionLogic.Mark(selection);
                return SelectionLogic.Interaction.Marked;

            // safeguard
            default:
                return SelectionLogic.Interaction.None;
        }
    }

    public static List<SelectionLogic.Selection> Show(List<string> options, int? _ = null, List<string>? preselected = null, 
        List<ConsoleKey>? keystrokes = null, string banner = "NEW MENU", SelectionLogic.Mode mode = SelectionLogic.Mode.Single)
    {

        // initialization
        Dictionary<string, SelectionLogic.Selectable> selection =
            SelectionLogic.ToSelectables(options, preselected, mode);

        // currently selected
        IEnumerable<KeyValuePair<string, SelectionLogic.Selectable>> selected;

        int lastWindowHeight = Console.WindowHeight;  // track the initial terminal height
        int reservedLines = ControlHelpPresent.GetFooterHeight();

        // loop
        while (true)
        {
            // Always render at the top of the terminal
            int menuStartLine = 0; // Fixed start at the top
            Console.SetCursorPosition(0, menuStartLine);

            // formatting
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

            // Refresh the footer
            ControlHelpPresent.ShowHelp(options, selectedIndex, menuContext: "admin");

            // capture & handle interaction
            switch (_update(selection, mode, keystrokes ?? []))
            {
                case SelectionLogic.Interaction.Marked:

                    // interrupt and prevent nest
                    selected = selection.Where(x => x.Value.selected == true);
                    if (selected.ElementAt(0).Key != "continue")
                        break;

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
                    
                    selected = selection.Where(x => x.Value.selected == true);
                    int index = selected.Select(x => x.Value.index).ElementAt(0);

                    Console.Clear();                    
                    return new()
                    {
                        new SelectionLogic.Selection()
                        {
                            text = selected.Select(x => x.Key).ElementAt(0),
                            index = (mode == SelectionLogic.Mode.Single) 
                                ? index : (options.Count - index) - 1
                        }
                    };

                case SelectionLogic.Interaction.Terminated:

                    Console.Clear();
                    ControlHelpPresent.ShowHelp();
                    Console.SetCursorPosition(0, menuStartLine);
                    Console.ForegroundColor = palette.Base;
                    Console.WriteLine(banner.Trim() + "\n");
                    return new List<SelectionLogic.Selection>
                    {
                        new SelectionLogic.Selection
                        {
                            text = null,
                            index = -1 // Special value indicating escape
                        }
                    };

            }
        }
    }
}
 