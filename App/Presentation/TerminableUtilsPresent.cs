using System.Drawing;

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
            ClearInputArea();
            
            // for (int i = 0; i < 2; i++)
            // {
            //     Console.Clear();
            //     if (text != null)
            //         Console.Write(text, colour);
            
            //     Console.Write(new string(buffer.ToArray()), colour);
            // }

            if (text != null)
            {
                Console.ForegroundColor = colour;
                Console.Write(text);
                Console.ResetColor();
            }
            Console.Write(new string(buffer.ToArray()));

            ControlHelpPresent.ShowHelp();

            capture = Console.ReadKey(intercept: true);
            
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

    private static void ClearInputArea()
    {
        int footerHeight = ControlHelpPresent.GetFooterHeight();
        int inputAreaEnd = Console.WindowHeight - footerHeight;

        for (int i = 0; i < inputAreaEnd; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write(new string(' ', Console.WindowWidth));
        }
        Console.SetCursorPosition(0, 0);
    }
}
