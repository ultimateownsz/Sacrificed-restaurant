using Project.Presentation;
using System.Net.Http.Headers;

namespace Project;
internal class SelectionPresent
{

    public struct Palette()
    {
        public ConsoleColor Primary    = ConsoleColor.Yellow;
        public ConsoleColor Secondary  = ConsoleColor.DarkYellow;
        public ConsoleColor Tertiary   = ConsoleColor.DarkYellow;
        public ConsoleColor Base       = ConsoleColor.White;
    }
    private static Palette palette = new Palette();

    private static void _display(Dictionary<string, SelectionLogic.Selectable> selection,
        string banner, SelectionLogic.Mode mode)
    {

        Terminable.Write(banner + "\n\n");
        Console.ForegroundColor = palette.Base;

        foreach (((string text, SelectionLogic.Selectable selectable), int index) 
            in selection.Select((value, index) => (value, index)))
        {
            // colouring (priority-sensitive)
            Console.ForegroundColor =
                (selectable.selected && selectable.highlighted)
                // selected and highlighted
                ? palette.Secondary : (selectable.selected)
                // only selected
                ? palette.Primary   : (selectable.highlighted)
                // only highlighted
                ? palette.Tertiary  : (palette.Base);

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

    public static void _controls()
    {
        string output = "";
        output += "CONTROLS OVERVIEW (click 'C' for shortcut):\n\n";

        output += "- General:\n";
        output += "  - Navigate: Arrow Keys (↑, ↓, ←, →)\n";
        output += "  - Select/Submit: ENTER (or SPACE where allowed)\n";
        output += "  - Go Back: ESC\n";
        output += "  - Input Text: Type and press ENTER\n\n";

        output += "- Menu Modes:\n";
        output += "  1. Single Mode:\n";
        output += "     - Navigate through a list with ↑ and ↓\n";
        output += "     - Select an item with ENTER\n";
        output += "  2. Multi Mode:\n";
        output += "     - Navigate through a list with ↑ and ↓\n";
        output += "     - Select multiple items with ENTER (or SPACE where allowed)\n";
        output += "     - Submit selection by clicking \"Continue\"\n";
        output += "  3. Scroll Mode:\n";
        output += "     - Scroll one line at a time with ↑ and ↓\n";
        output += "     - Submit selection with ENTER\n";
        output += "  4. Calendar Mode:\n";
        output += "     - Navigate the 2D calendar with ↑, ↓, ←, →\n";
        output += "     - Select a date with ENTER\n";
        output += "  5. Table Selection Mode:\n";
        output += "     - Navigate the 2D table with ↑, ↓, ←, →\n";
        output += "     - Select a cell with ENTER\n";
        output += "\nPress any key to continue...";

        // display
        Console.Clear();
        Console.WriteLine(output);
        Console.ReadKey();
    }

    private static SelectionLogic.Interaction _update(
        Dictionary<string, SelectionLogic.Selectable> selection, SelectionLogic.Mode mode)
    {
        ConsoleKey capture;
        switch (capture = Console.ReadKey().Key)
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

            case ConsoleKey.C:
                _controls();
                return SelectionLogic.Interaction.None;

            // safeguard
            default:
                return SelectionLogic.Interaction.None;
        }
    }

    public static List<SelectionLogic.Selection> Show(List<string> options, List<string>? preselected = null,
        string banner = "NEW MENU", SelectionLogic.Mode mode = SelectionLogic.Mode.Single)
    {

        // initialization
        Dictionary<string, SelectionLogic.Selectable> selection =
            SelectionLogic.ToSelectables(options, preselected, mode);

        // currently selected
        IEnumerable<KeyValuePair<string, SelectionLogic.Selectable>> selected;

        // loop
        while (true)
        {
            // formatting
            _display(selection, banner, mode);

            // capture & handle interaction
            switch (_update(selection, mode))
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

                    Console.Clear();
                    selected = selection.Where(x => x.Value.selected == true);
                    return new()
                    {
                        new SelectionLogic.Selection()
                        {
                            text = selected.Select(x => x.Key).ElementAt(0),
                            index = selected.Select(x => x.Value.index).ElementAt(0)
                        }
                    };

                case SelectionLogic.Interaction.Terminated:

                    Console.Clear();
                    return new()
                    {
                        new()
                        {
                            text = "",
                            index = -1
                        }
                    };

            }
        }
    }
}
