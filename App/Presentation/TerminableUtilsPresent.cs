namespace Restaurant;
internal class TerminableUtilsPresent
{

    // I'm very sorry I had to refactor this method because of I couldn't add controls showing up in the footer if the console would be cleared      
    public static void Write(string? text)
    {
        // this seems quite strange doesn't it?
        // for <some> reason, VS' terminal excludes
        // the first character in a STDOUT request
        // after interrupting the <ESC> key, the 
        // simplest yet most effective way to circumvent
        // this issue is by simply writing it twice.  

        // for (int i = 0; i < 2; i++)
        // {
        //     Console.Clear();
        //     Console.Write(text);
        // }

        if (string.IsNullOrEmpty(text)) return;
        Console.Write(text);

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

        // calculate dynamic positions
        int footerHeight = ControlHelpPresent.GetFooterHeight();  // returns the height of the help footer
        int promptLine = 0; // always keep input prompt at the top (line 0)
        int controlsStartline = Console.WindowHeight - footerHeight;
        
        while (true)
        {
            // clear only the relevant areas: input area and controls footer
            ClearLine(promptLine);
            ClearLine(controlsStartline);

            // position cursor and render the input prompt
            Console.SetCursorPosition(0, promptLine);
            if (!string.IsNullOrEmpty(text))
            {
                Console.ForegroundColor = colour;
                Write(text);
                Console.ResetColor();
            }

            // for (int i = 0; i < 2; i++)
            // {
            //     Console.Clear();
            //     if (text != null)
            //         Console.Write(text, colour);
            
            //     Console.Write(new string(buffer.ToArray()), colour);
            // }

            Write(new string(buffer.ToArray()));

            Console.SetCursorPosition(0, controlsStartline);
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

    // Clears a specific line in the console
    private static void ClearLine(int line)
    {
        Console.SetCursorPosition(0, line);
        Console.Write(new string(' ', Console.WindowWidth)); // Clear entire line
        Console.SetCursorPosition(0, line); // Reset cursor to start of cleared line
    }
}