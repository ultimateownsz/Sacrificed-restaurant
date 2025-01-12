namespace App.Logic.Table
{
    public static class TableSelectionLogic
    {
        public static async Task FlashHighlightAsyncLogic(
            int tableNumber, int x, int y, ConsoleColor tableColor, CancellationToken token,
            Action<int, int, string> setCursor, Func<int, int, string> restoreNumber)
        {
            while (!token.IsCancellationRequested)
            {
                setCursor(x, y, "X ");
                await Task.Delay(500);

                if (token.IsCancellationRequested) break;

                string originalNumber = restoreNumber(x, y);
                setCursor(x, y, originalNumber);
                await Task.Delay(500);
            }
        }

        public static void ClearGridLogic(char[,] grid, Action<int, int, string> clearLine)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                clearLine(0, y, new string(' ', grid.GetLength(1)));
            }
        }

        public static (int x, int y) FindTableCoordinatesLogic(char[,] grid, int tableNumber, Func<int, int, string> getNumberAt)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = getNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number) && int.Parse(number) == tableNumber)
                    {
                        return (x, y);
                    }
                }
            }
            throw new Exception($"Table {tableNumber} not found in the grid.");
        }

        public static string GetNumberAtLogic(char[,] grid, int x, int y)
        {
            if (x < 0 || x >= grid.GetLength(1) || y < 0 || y >= grid.GetLength(0))
                return string.Empty;

            string number = "";
            while (x < grid.GetLength(1) && char.IsDigit(grid[y, x]))
            {
                number += grid[y, x];
                x++;
            }
            return number;
        }

        public static (bool isValid, string message) ValidateTableLogic(
            int tableNumber, int guestCount, int[] activeTables, int[] reservedTables, int[] inactiveTables)
        {
            if (reservedTables.Contains(tableNumber))
                return (false, $"Table {tableNumber} is reserved.");
            if (inactiveTables.Contains(tableNumber))
                return (false, $"Table {tableNumber} is inactive.");
            if (!activeTables.Contains(tableNumber))
                return (false, $"Table {tableNumber} is not active.");
            if (guestCount > 6)
                return (false, $"Table {tableNumber} cannot accommodate more than 6 guests.");

            return (true, string.Empty);
        }

        public static (int x, int y) FindFirstAvailableCoordinatesLogic(
            char[,] grid, int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount,
            Func<int, int, string> getNumberAt, Func<int, int, int[], bool> isTableValidForGuests)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = getNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        if (!reservedTables.Contains(tableNumber) && !inactiveTables.Contains(tableNumber)
                            && isTableValidForGuests(tableNumber, guestCount, activeTables))
                        {
                            return (x, y);
                        }
                    }
                }
            }
            return FindTableCoordinatesLogic(grid, 1, getNumberAt);
        }

        public static void ClearCursorAreaLogic(int x, int y, string content, ConsoleColor color, Action<int, int, string, ConsoleColor> render)
        {
            render(x, y, content, color);
        }

        public static int SelectTableLogic(
            char[,] grid, int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount,
            Func<int, int, string> getNumberAt, Func<int, int, int[], bool> isTableValidForGuests,
            Func<int[], int[], int[], int, (int, int)> findFirstAvailableCoordinates,
            Func<int, int[], int[], int[], bool, (bool, string)> validateTable,
            Action<int, int, ConsoleColor, string> updateHighlight,
            Func<ConsoleKey> readKey)
        {
            var (cursorX, cursorY) = findFirstAvailableCoordinates(activeTables, inactiveTables, reservedTables, guestCount);
            updateHighlight(cursorX, cursorY, ConsoleColor.Yellow, "X");

            while (true)
            {
                ConsoleKey key = readKey();
                if (key == ConsoleKey.Enter)
                {
                    string selectedNumber = getNumberAt(cursorX, cursorY);
                    if (string.IsNullOrEmpty(selectedNumber)) continue;

                    int tableNumber = int.Parse(selectedNumber);
                    var (isValid, message) = validateTable(
                        tableNumber, guestCount, activeTables, reservedTables, inactiveTables, false);

                    if (!isValid)
                    {
                        Console.WriteLine(message);
                        continue;
                    }

                    return tableNumber;
                }
            }
        }
    }
}
