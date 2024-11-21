using Project.Logic;

namespace Project.Presentation;
internal class SelectionMenu: SelectionMenuLogic
{
    public static string? Show(List<string> options, string banner = "")
    {   
        Dictionary<string, bool> selection = ConstructSelection(options);
        while (true)
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
                    return current.Item1 ?? null;
            }
        }
    }
}
