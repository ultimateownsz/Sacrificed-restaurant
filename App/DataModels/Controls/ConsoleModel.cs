namespace App.DataModels.Controls;

public class ConsoleModel : IConsole
{
    public int WindowHeight => Console.WindowHeight;
    public int WindowWidth => Console.WindowWidth;
    public int CursorTop => Console.CursorTop;

    private readonly List<string> output = new();

    public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
    public void WriteLine(string message) => Console.WriteLine(message);
    public void Write(string message) => Console.Write(message);

    public ConsoleColor ForegroundColor
    {
        get => Console.ForegroundColor;
        set => Console.ForegroundColor = value;
    }

    public List<string> GetOutput() => output;
}