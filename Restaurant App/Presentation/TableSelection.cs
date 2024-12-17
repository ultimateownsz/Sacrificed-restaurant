using Project;

namespace Presentation;
public class TableSelection
{
    private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
    private int cursorX, cursorY;
    private Dictionary<int, ConsoleColor> tableColors = new();

    public int SelectedTable { get; private set; }

    private void ClearGrid()
    {
        for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write(new string(' ', GridPresent.GetGrid().GetLength(1))); // Clear the entire row
        }
        Console.SetCursorPosition(0, 0); // Reset cursor to the top
    }

    public void ShowGrid(int[] activeTables, int[] reservedTables, bool isAdminMode = false)
    {
        tableColors.Clear();
        ClearGrid();

        for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
        {
            for (int x = 0; x < GridPresent.GetGrid().GetLength(1); x++)
            {
                string number = GetNumberAt(x, y);
                if (!string.IsNullOrEmpty(number))
                {
                    int tableNumber = int.Parse(number);
                    x += number.Length - 1;

                    // Admin Mode: Active/Inactive
                    if (isAdminMode)
                    {
                        tableColors[tableNumber] = Array.Exists(activeTables, t => t == tableNumber)
                            ? ConsoleColor.Green
                            : ConsoleColor.Red;
                    }
                    else
                    {
                        // User Mode: Availability and Reservation Status
                        tableColors[tableNumber] = Array.Exists(reservedTables, t => t == tableNumber)
                            ? ConsoleColor.Red
                            : ConsoleColor.Green;
                    }

                    Console.SetCursorPosition(x - (number.Length - 1), y);
                    Console.ForegroundColor = tableColors[tableNumber];
                    Console.Write(number);
                }
                else
                {
                    Console.ResetColor();
                    Console.SetCursorPosition(x, y);
                    Console.Write(GridPresent.GetGrid()[y, x]);
                }
            }
        }

        // Place "X" on the first table
        (cursorX, cursorY) = FindTableCoordinates(1);
        HighlightNumber(activeTables, reservedTables, isAdminMode);
    }

    private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor)
    {
        var token = flashCancellationTokenSource.Token;

        while (!token.IsCancellationRequested)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = tableColor;
            Console.Write(tableNumber.ToString().PadRight(2));
            await Task.Delay(500);

            if (token.IsCancellationRequested) break;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = tableColor;
            Console.Write("X ");
            await Task.Delay(500);
        }

        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = tableColor;
        Console.Write(tableNumber.ToString().PadRight(2));
        Console.ResetColor();
    }

    private void HighlightNumber(int[] activeTables, int[] reservedTables, bool isAdminMode)
    {
        if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
        {
            flashCancellationTokenSource.Cancel();
            flashCancellationTokenSource.Dispose();
        }

        flashCancellationTokenSource = new CancellationTokenSource();

        string currentNumber = GetNumberAt(cursorX, cursorY);

        if (!string.IsNullOrEmpty(currentNumber))
        {
            int currentTable = int.Parse(currentNumber);

            ConsoleColor tableColor = isAdminMode
                ? (Array.Exists(activeTables, t => t == currentTable) ? ConsoleColor.Green : ConsoleColor.Red)
                : (Array.Exists(reservedTables, t => t == currentTable) ? ConsoleColor.Red : ConsoleColor.Green);

            _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor);
        }
    }

    public int SelectTable(int[] activeTables, int[] reservedTables, bool isAdmin = false)
    {
        ShowGrid(activeTables, reservedTables, isAdminMode: isAdmin);
        Console.CursorVisible = false;

        try
        {
            while (true)
            {
                Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 2);
                Console.ResetColor();
                Console.WriteLine("(B)ack");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                {
                    StopFlashing();
                    ResetConsoleToDefault();
                    return -1; // Back
                }

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        string selectedNumber = GetNumberAt(cursorX, cursorY);
                        if (!string.IsNullOrEmpty(selectedNumber))
                        {
                            int tableNumber = int.Parse(selectedNumber);

                            if (!isAdmin && Array.Exists(reservedTables, t => t == tableNumber))
                            {
                                Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Table is already reserved.");
                                Console.ResetColor();
                                continue;
                            }

                            return tableNumber;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        NavigateToNextNumber(1, 0, activeTables, reservedTables, isAdmin);
                        break;
                    case ConsoleKey.LeftArrow:
                        NavigateToNextNumber(-1, 0, activeTables, reservedTables, isAdmin);
                        break;
                    case ConsoleKey.DownArrow:
                        NavigateToNextNumber(0, 1, activeTables, reservedTables, isAdmin);
                        break;
                    case ConsoleKey.UpArrow:
                        NavigateToNextNumber(0, -1, activeTables, reservedTables, isAdmin);
                        break;
                }
            }
        }
        finally
        {
            StopFlashing();
            ResetConsoleToDefault();
            Console.CursorVisible = true;
        }
    }

    private void NavigateToNextNumber(int dx, int dy, int[] activeTables, int[] reservedTables, bool isAdminMode)
    {
        int nextX = cursorX + dx;
        int nextY = cursorY + dy;

        string nextNumber = GetNumberAt(nextX, nextY);

        if (!string.IsNullOrEmpty(nextNumber))
        {
            cursorX = nextX;
            cursorY = nextY;
            HighlightNumber(activeTables, reservedTables, isAdminMode);
        }
    }

    private void StopFlashing()
    {
        if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
        {
            flashCancellationTokenSource.Cancel();
            flashCancellationTokenSource.Dispose();
            flashCancellationTokenSource = null;
        }
    }

    private void ResetConsoleToDefault()
    {
        Console.ResetColor();
        Console.Clear();
    }

    private string GetNumberAt(int x, int y)
    {
        if (y < 0 || y >= GridPresent.GetGrid().GetLength(0) || x < 0 || x >= GridPresent.GetGrid().GetLength(1))
            return null;

        string number = "";
        while (x < GridPresent.GetGrid().GetLength(1) && char.IsDigit(GridPresent.GetGrid()[y, x]))
        {
            number += GridPresent.GetGrid()[y, x];
            x++;
        }

        return number;
    }
}
