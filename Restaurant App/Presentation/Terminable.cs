using System.Runtime.InteropServices;

namespace Project.Presentation;

internal class Terminable
{

    public static void Write(string? text)
    {
        // this seems quite strange doesn't it?
        // for <some> reason, VS' terminal excludes
        // the first character in a STDOUT request
        // after interrupting the <ESC> key, the 
        // simplest yet most effective way to circumvent
        // this issue is by simply writing it twice.        
        for (int i = 0; i < 2; i++)
        {
            Console.Clear();
            Console.Write(text);
        }

    }

    // operates just like Console.ReadLine
    // and -WriteLine, but returns null if terminated
    // also python's input("text") is OP so implemented it.
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
