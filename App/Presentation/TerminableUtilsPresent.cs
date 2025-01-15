namespace Restaurant;
public static class TerminableUtilsPresent
{

    public static void Write(string? text)
    {      
        for (int i = 0; i < 2; i++)
        {
            Console.Clear();
            Console.Write(text);
        }

    }

    public static string? ReadLine(
        string? text = null, string? load = null,
        ConsoleColor colour = ConsoleColor.Gray)
    {   
        ConsoleKeyInfo capture;
        List<char> buffer = new();

        // load
        if (load != null)
            buffer = load.ToList();

        while (true)
        {
            
            for (int i = 0; i < 2; i++)
            {
                Console.Clear();
                if (text != null)
                    Console.Write(text, colour);
            
                Console.Write(new string(buffer.ToArray()), colour);
            }

            capture = Console.ReadKey();
            
            switch (capture.Key)
            {
                case ConsoleKey.Escape:
                    return null;

                case ConsoleKey.Enter:
                    Console.WriteLine();
                    return new string(buffer.ToArray());

                case ConsoleKey.Backspace:
                    if (buffer.Count > 0)
                        buffer.RemoveAt(buffer.Count - 1);
                    continue;

                default:
                    buffer.Add(capture.KeyChar);
                    break;
            }
        }
    }
}
