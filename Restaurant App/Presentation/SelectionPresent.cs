namespace Project;
using System.Dynamic;
using System.Reflection;

internal class SelectionPresent : SelectionLogic
{
    private static void _update(string banner, Dictionary<string, bool> selection)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(banner, Console.ForegroundColor);

        foreach ((string text, bool selected) in selection)
        {
            Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = (selected) ? "-> " : "";

            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
        }
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
        }

        return null;
    }

    public static dynamic Show(List<string> options, string banner = "")
    {
        Tuple<string?, int?>? selected;
        Dictionary<string, bool> selection = ToSelectable(options);

        while (true)
        {
            // update screen
            _update(banner, selection);

            // read user-input

            if ((selected = _read(selection)) != null)
            {
                // iniitialize
                dynamic dynamicHandle = new ExpandoObject();
                dynamicHandle.text = selected.Item1;
                dynamicHandle.index = selected.Item2;

                // return
                return dynamicHandle;
            }
        }
    }
}
