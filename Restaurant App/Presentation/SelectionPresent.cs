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

    public static dynamic Show(List<string> options, string banner = "", bool oneline = false)
    {
        Tuple<string?, int?>? selected;
        if (oneline) options.Reverse();
        
        Dictionary<string, bool> selection = ToSelectable(options, oneline);

        while (true)
        {
            // update screen
            _update(banner, selection, oneline);

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

    public static bool EscapeKeyPressedWithConfirmation()
    {
        if (Console.IsInputRedirected)
        {
            int nextChar = Console.In.Peek(); // Controleer of er invoer beschikbaar is
            if (nextChar != -1) // -1 betekent EOF
            {
                var key = (ConsoleKey)Console.Read(); // Lees een char als ConsoleKey

                if (key == ConsoleKey.Escape)
                {
                    var confirmation = SelectionPresent.Show(
                        new List<string> { "yes", "no" },
                        "Are you sure you want to exit?\n\n"
                    );

                    if (confirmation.text == "yes")
                    {
                        Console.WriteLine("\nExiting program...");
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        return false;
                    }
                }
            }
        }
        return false;
    }

    //     var key = Console.ReadKey(intercept: true);
    //     if (key.Key == ConsoleKey.Escape)
    //     {
    //         var confirmation = SelectionPresent.Show(
    //             new List<string> { "yes", "no" }, 
    //             "Are you sure you want to exit?\n\n"
    //         );

    //         if (confirmation.text == "yes")
    //         {
    //             Console.WriteLine("\nExiting program...");
    //             return true;
    //         }
    //         else
    //         {
    //             // Console.WriteLine("\nReturning to menu...");
    //             Console.Clear();
    //             return false;
    //         }
    //     }
    //     return false;
    // }
}
