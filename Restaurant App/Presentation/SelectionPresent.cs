namespace Project;
using System.Dynamic;
using System.Reflection;

internal class SelectionPresent : SelectionLogic
{
    private static void _update(string banner, Dictionary<string, bool> selection, bool oneline)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(banner, Console.ForegroundColor);

        if (!string.IsNullOrEmpty(banner))
        // Console.WriteLine(banner);

        foreach ((string text, bool selected) in selection)
        {
            Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
            string prefix = (selected) ? "-> " : "";

            if (oneline && !selected) continue;
            Console.WriteLine($"{prefix}{text}", Console.ForegroundColor);
        }
    }

    private static Tuple<string?, int?>? _read(Dictionary<string, bool> selection)
    {
        var current = ReverseLookup<string, bool>(selection, true);

        var key = Console.ReadKey(intercept: true).Key;
        Console.WriteLine($"Key pressed: {key}"); // Debugging

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

                return null;
        }

        return null;
    }

    public static dynamic Show(List<string> options, string banner = "", bool oneline = false)
    {
        Tuple<string?, int?>? selected;
        if (oneline) options.Reverse();
        
        Dictionary<string, bool> selection = ToSelectable(options, oneline);

        while (true)
        {
            // update screen
            _update(banner, selection, oneline);

            if (EscapeKeyIsPressed())
            {
                Console.WriteLine("Escape pressed, exiting...");
                return null;
            }

            // read user-input
            selected = _read(selection);
            if (selected != null)
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

            return confirmation.text == "yes";
        }
        Console.Clear();
        return false;
    }
}
