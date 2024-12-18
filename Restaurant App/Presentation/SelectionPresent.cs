namespace Project;
internal class SelectionPresent
{
    private static void _display(Dictionary<string, SelectionLogic.Selectable> selection,
        string banner, SelectionLogic.Mode mode)
    {

        // banner & colour initialization
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(banner + "\n");

        foreach ((string text, SelectionLogic.Selectable selectable) in selection)
        {
            // colouring (priority-sensitive)
            Console.ForegroundColor =
                (selectable.selected && selectable.highlighted)
                // selected and highlighted
                ? ConsoleColor.Cyan : (selectable.selected)
                // only selected
                ? ConsoleColor.Blue : (selectable.highlighted)
                // only highlighted
                ? ConsoleColor.DarkCyan : (ConsoleColor.White);

            // marker
            string prefix = (selectable.selected) ? ">" : "";
            string suffix = (selectable.selected) ? "" : "";
            if (mode == SelectionLogic.Mode.Scroll && !selectable.selected) continue;

            // output
            Console.WriteLine($"{prefix} {text} {suffix}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
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
                    return new();
            }
        }
    }
}
