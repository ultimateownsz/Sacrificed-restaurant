using App.Logic.Table;
using App.DataAccess.Utils;
using Restaurant;

namespace App.Presentation.Table
{
    public class TableSelectionPresent
    {
        private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
        private int cursorX, cursorY;

        public int SelectedTable { get; private set; }

        public void ShowGrid(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount, bool isAdminMode = false)
        {
            var grid = GridPresent.GetGrid();
            var tableColors = TableSelectionLogic.DetermineTableColors(grid, activeTables, inactiveTables, reservedTables, guestCount, isAdminMode);

            Console.Clear();

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = TableSelectionLogic.GetNumberAt(grid, x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        Console.SetCursorPosition(x - (number.Length - 1), y);
                        Console.ForegroundColor = tableColors[tableNumber];
                        Console.Write(number);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(x, y);
                        Console.Write(grid[y, x]);
                    }
                }
            }

            (cursorX, cursorY) = TableSelectionLogic.FindFirstAvailableCoordinates(grid, activeTables, inactiveTables, reservedTables, guestCount);
            HighlightNumber(grid, activeTables, reservedTables, inactiveTables, guestCount, isAdminMode);
        }

        private void HighlightNumber(char[,] grid, int[] activeTables, int[] reservedTables, int[] inactiveTables, int guestCount, bool isAdmin)
        {
            if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
            {
                flashCancellationTokenSource.Cancel();
                flashCancellationTokenSource.Dispose();
            }

            flashCancellationTokenSource = new CancellationTokenSource();

            string currentNumber = TableSelectionLogic.GetNumberAt(grid, cursorX, cursorY);

            if (!string.IsNullOrEmpty(currentNumber))
            {
                int currentTable = int.Parse(currentNumber);

                var (isValid, _, message) = TableSelectionLogic.ValidateTable(currentTable, guestCount, activeTables, reservedTables, inactiveTables, isAdmin);

                if (!isAdmin && !string.IsNullOrEmpty(message))
                {
                    DisplayMessage(message);
                }
                else
                {
                    ClearErrorMessage();
                }

                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, ConsoleColor.Yellow);
            }
        }

        private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor)
        {
            var token = flashCancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("X ");
                await Task.Delay(500);

                if (token.IsCancellationRequested) break;

                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
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

        private void DisplayMessage(string message)
        {
            Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 1);

            Console.ForegroundColor = message.Contains("can be reserved") ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void ClearErrorMessage()
        {
            Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 1);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }
}
