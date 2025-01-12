namespace App.Presentation.Table
{
    public class TableSelectionPresent
    {
        private readonly char[,] grid;
        private readonly Dictionary<int, ConsoleColor> tableColors = new();
        private CancellationTokenSource flashCancellationTokenSource = new();
        private int cursorX, cursorY;

        public TableSelectionPresent(char[,] grid)
        {
            this.grid = grid;
        }

        public async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor)
        {
            var token = flashCancellationTokenSource.Token;
            await App.Logic.Table.TableSelectionLogic.FlashHighlightAsyncLogic(
                tableNumber, x, y, tableColor, token,
                (cx, cy, str) =>
                {
                    Console.SetCursorPosition(cx, cy);
                    Console.Write(str);
                },
                (cx, cy) => GetNumberAt(cx, cy));
        }

        public void ClearGrid()
        {
            App.Logic.Table.TableSelectionLogic.ClearGridLogic(grid, (x, y, str) =>
            {
                Console.SetCursorPosition(x, y);
                Console.Write(str);
            });
        }

        public (int x, int y) FindTableCoordinates(int tableNumber)
        {
            return App.Logic.Table.TableSelectionLogic.FindTableCoordinatesLogic(grid, tableNumber, GetNumberAt);
        }

        public (int x, int y) FindFirstAvailableCoordinates(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount)
        {
            return App.Logic.Table.TableSelectionLogic.FindFirstAvailableCoordinatesLogic(
                grid, activeTables, inactiveTables, reservedTables, guestCount, GetNumberAt, IsTableValidForGuests);
        }

        public void ClearCursorArea(int x, int y, string content, ConsoleColor color)
        {
            App.Logic.Table.TableSelectionLogic.ClearCursorAreaLogic(x, y, content, color, (cx, cy, str, clr) =>
            {
                Console.SetCursorPosition(cx, cy);
                Console.ForegroundColor = clr;
                Console.Write(str);
                Console.ResetColor();
            });
        }

        public int SelectTable(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount, char[,] grid)
        {
            return App.Logic.Table.TableSelectionLogic.SelectTableLogic(
                grid, activeTables, inactiveTables, reservedTables, guestCount,
                GetNumberAt, IsTableValidForGuests,
                FindFirstAvailableCoordinates,
                ValidateTable,
                UpdateTableHighlight,
                () => Console.ReadKey(true).Key);
        }

        private string GetNumberAt(int x, int y)
        {
            return App.Logic.Table.TableSelectionLogic.GetNumberAtLogic(grid, x, y);
        }

        private bool IsTableValidForGuests(int tableNumber, int guestCount, int[] activeTables)
        {
            return activeTables.Contains(tableNumber) && guestCount <= 6;
        }

        private void UpdateTableHighlight(int x, int y, ConsoleColor color, string content)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(content);
            Console.ResetColor();
        }

        private (bool isValid, string message) ValidateTable(int tableNumber, int[] activeTables, int[] reservedTables, int[] inactiveTables, int guestCount)
        {
            return App.Logic.Table.TableSelectionLogic.ValidateTableLogic(
                tableNumber, guestCount, activeTables, reservedTables, inactiveTables);
        }
    }
}