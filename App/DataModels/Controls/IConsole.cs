namespace App.DataModels.Controls;

public interface IConsole
{
    int WindowHeight { get; }
    int WindowWidth { get; }
    int CursorTop { get; }

    void SetCursorPosition(int left, int top);
    void Write(string message);
    void WriteLine(string message);

    
}