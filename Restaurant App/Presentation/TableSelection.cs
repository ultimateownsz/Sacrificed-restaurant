using System;

namespace Presentation
{
    public class TableSelection
    {
        private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
        private int cursorX, cursorY; // Updated as requested
        private int previousX = -1, previousY = -1;
        private Dictionary<int, ConsoleColor> tableColors = new Dictionary<int, ConsoleColor>();

        public int SelectedTable { get; private set; }

        public int SelectTable(int[] availableTables, int[] reservedTables, bool isAdmin)
        {
            EnsureConsoleSize(); // Ensure the console size is adequate
            Console.CursorVisible = false; // Hide the cursor

            try
            {
                if (!GridPresent.Show(availableTables, reservedTables, isAdmin))
                {
                    return -1; // Exit early if admin
                }

                // Automatically find table 1 and set the cursor position
                (cursorX, cursorY) = FindTableCoordinates(1);

                int lastX = cursorX, lastY = cursorY;

                while (true)
                {
                    Console.SetCursorPosition(0, Console.WindowHeight - 3);
                    Console.ResetColor();
                    Console.WriteLine("(B)ack");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                    {
                        StopFlashing();
                        ResetConsoleToDefault();
                        return -1; // Return -1 to indicate cancellation
                    }

                    int nextX = cursorX, nextY = cursorY;

                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            (nextX, nextY) = GridPresent.FindNextNumberInRow(cursorX, cursorY, 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            (nextX, nextY) = GridPresent.FindNextNumberInRow(cursorX, cursorY, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            (nextX, nextY) = GridPresent.FindNextNumberInColumn(cursorX, cursorY, 1);
                            break;
                        case ConsoleKey.UpArrow:
                            (nextX, nextY) = GridPresent.FindNextNumberInColumn(cursorX, cursorY, -1);
                            break;
                        case ConsoleKey.Enter:
                            string selectedNumber = GridPresent.GetNumberAt(cursorX, cursorY);

                            if (string.IsNullOrEmpty(selectedNumber))
                            {
                                Console.SetCursorPosition(0, Console.WindowHeight - 4);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid table. Please select a valid table.");
                                Console.ResetColor();
                                continue;
                            }

                            int tableNumber = int.Parse(selectedNumber);

                            if (!Array.Exists(availableTables, table => table == tableNumber) ||
                                Array.Exists(reservedTables, table => table == tableNumber))
                            {
                                Console.SetCursorPosition(0, Console.WindowHeight - 4);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Table {tableNumber} is unavailable. Please select another table.");
                                Console.ResetColor();
                                continue;
                            }

                            SelectedTable = tableNumber;

                            StopFlashing();
                            ResetConsoleToDefault();
                            return SelectedTable;
                    }

                    if (nextX < 0 || nextX >= GridPresent.GridWidth || nextY < 0 || nextY >= GridPresent.GridHeight ||
                        string.IsNullOrEmpty(GridPresent.GetNumberAt(nextX, nextY)))
                    {
                        nextX = lastX;
                        nextY = lastY;
                    }
                    else
                    {
                        UpdateTableHighlight(lastX, lastY, nextX, nextY, availableTables, reservedTables);
                        lastX = nextX;
                        lastY = nextY;
                        cursorX = nextX;
                        cursorY = nextY;

                        HighlightNumber(availableTables, reservedTables);
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

        private void HighlightNumber(int[] availableTables, int[] reservedTables)
        {
            StopFlashing();

            string currentNumber = GridPresent.GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(currentNumber))
            {
                int currentTable = int.Parse(currentNumber);

                ConsoleColor tableColor = Array.Exists(reservedTables, table => table == currentTable)
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor, availableTables, reservedTables);
            }
        }

        private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor, int[] availableTables, int[] reservedTables)
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

            if (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
            }

            Console.ResetColor();
        }

        private void UpdateTableHighlight(int prevX, int prevY, int currX, int currY, int[] availableTables, int[] reservedTables)
        {
            GridPresent.RemoveHighlight(prevX, prevY);

            string currNumber = GridPresent.GetNumberAt(currX, currY);
            if (!string.IsNullOrEmpty(currNumber))
            {
                int currTable = int.Parse(currNumber);
                ConsoleColor currColor = Array.Exists(availableTables, table => table == currTable)
                    ? ConsoleColor.Green
                    : ConsoleColor.Red;

                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = currColor;
                Console.Write("X ");
            }

            Console.ResetColor();
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

        private void EnsureConsoleSize()
        {
            const int requiredWidth = 80;
            const int requiredHeight = 30;

            while (Console.WindowWidth < requiredWidth || Console.WindowHeight < requiredHeight)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Your console is too small. Minimum size: {requiredWidth}x{requiredHeight}");
                Console.WriteLine($"Current size: {Console.WindowWidth}x{Console.WindowHeight}");
                Console.ResetColor();
                Console.WriteLine("Resize the console and press Enter to continue...");
                Console.ReadLine();
            }

            Console.Clear();
        }

        private (int x, int y) FindTableCoordinates(int tableNumber)
        {
            // Iterate through the grid to locate the given table number
            for (int y = 0; y < GridPresent.GridHeight; y++)
            {
                for (int x = 0; x < GridPresent.GridWidth; x++)
                {
                    string number = GridPresent.GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number) && int.Parse(number) == tableNumber)
                    {
                        return (x, y); // Return the coordinates of the table
                    }
                }
            }

            throw new Exception($"Table {tableNumber} not found in the grid."); // Error if table not found
        }
    }
}
