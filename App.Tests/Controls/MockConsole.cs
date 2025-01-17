using App.DataModels.Controls;
namespace App.Tests;

public class MockConsole : IConsole
{
    public int WindowHeight { get; set; } = 50; // Simulated console height
    public int WindowWidth { get; set; } = 100; // Simulated console width
    public int CursorTop { get; set; } = 0; // Simulated cursor position
    public ConsoleColor ForegroundColor { get; set; } // Simulated color changes

    private readonly List<string> output = new(); // Captures all output
     private readonly List<int> cursorPositions = new(); // Captures cursor positions for each output

    public void SetCursorPosition(int left, int top)
    {
        CursorTop = top; // Simulate moving the cursor
        cursorPositions.Add(top); // Log the cursor position
    }

    public void WriteLine(string message)
    {
        output.Add(message); // Capture output
    }

    public void Write(string message)
    {
        output.Add(message); // Capture output
    }

    public void ResetColor()
    {
        ForegroundColor = ConsoleColor.White; // Simulate resetting the color
    }

    public List<string> GetOutput()
    {
        return output; // Return captured output
    }

    public List<int> GetCursorPositions() => cursorPositions; // return captured cursor positions
}
