namespace Project;
using System.Dynamic;

internal class SelectionPresent
{
    private static void _update(string banner, Dictionary<string, bool> selection, 
        ref Tuple<List<string?>, List<int?>> multiselect, SelectionLogic.Mode mode)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(banner, Console.ForegroundColor);

        foreach ((string text, bool selected) in selection)
        {
            Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = (selected) ? "-> " : "";

            if (mode == SelectionLogic.Mode.Multi)
                Console.ForegroundColor = (multiselect.Item1.Contains(text)) ? ConsoleColor.Yellow : ConsoleColor.White;
            
            if (mode == SelectionLogic.Mode.Narrow && !selected) continue;
            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
        }
    }

    private static Tuple<string?, int?>? _read(Dictionary<string, bool> selection, 
        ref Tuple<List<string?>, List<int?>> multiselect, SelectionLogic.Mode mode)
    {
        var current = SelectionLogic.ReverseLookup<string, bool>(selection, true);
        

        switch (Console.ReadKey().Key)
        {
            case ConsoleKey.DownArrow:

                selection[current.Item1 ?? ""] = false;
                selection[selection.ElementAt(SelectionLogic.Next(selection.Count, current.Item2)).Key] = true;
                break;

            case ConsoleKey.UpArrow:

                selection[current.Item1 ?? ""] = false;
                selection[selection.ElementAt(SelectionLogic.Next(selection.Count, current.Item2, true)).Key] = true;
                break;

            case ConsoleKey.Enter:
                
                Console.ForegroundColor = ConsoleColor.White;
                if (mode == SelectionLogic.Mode.Multi)
                {
                    if (current.Item1 == "continue")
                    {
                        return new("", -1);
                    }

                    if (multiselect.Item1.Contains(current.Item1))
                    {
                        multiselect.Item1.Remove(current.Item1);
                        multiselect.Item2.Remove(current.Item2);
                    }
                    else
                    {
                        multiselect.Item1.Add(current.Item1);
                        multiselect.Item2.Add(current.Item2);
                    }

                    return new("", 0);
                }

                return new(current.Item1, current.Item2);

            // hmm.. somebody toucha ma code, and destabilized it
            // I won't touch it for now, but I will find you, and I will kill you.
            case ConsoleKey.Escape:
            case ConsoleKey.B:

                Console.ForegroundColor = ConsoleColor.White;
                return new("", -1);
        }

        return null;
    }

    public static dynamic Show(List<string> options, string banner = "", 
        SelectionLogic.Mode mode = SelectionLogic.Mode.Single)
    {
        // if you don't understand it, don't touch it..
        Tuple<string?, int?>? selected;
        Tuple<List<string?>, List<int?>> multiselect = new(new(), new());
        dynamic dynamicHandle = new ExpandoObject();

        if (mode == SelectionLogic.Mode.Narrow) options.Reverse();
        Dictionary<string, bool> selection = SelectionLogic.ToSelectable(options, mode);

        while (true)
        {
            // update screen
            _update(banner, selection, ref multiselect, mode);

            if ((selected = _read(selection, ref multiselect, mode)) != null)
            {
                if (mode == SelectionLogic.Mode.Multi)
                {
                    if (selected.Item2 == -1)
                    {
                        dynamicHandle.text = multiselect.Item1;
                        dynamicHandle.index = multiselect.Item2;
                        return dynamicHandle;
                    }
                    
                    continue;
                }
                
                dynamicHandle.text = selected.Item1;
                dynamicHandle.index = selected.Item2;
                return dynamicHandle;
            }
        }
    }
}
