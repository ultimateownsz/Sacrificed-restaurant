namespace App.Logic.Table
{
    public class TableSelectionLogic
    {
        public string[] GenerateClearedRows(int gridHeight, int gridWidth)
        {
            string[] clearedRows = new string[gridHeight];
            for (int i = 0; i < gridHeight; i++)
            {
                clearedRows[i] = new string(' ', gridWidth);
            }

            return clearedRows;
        }

        public (int x, int y) FindTableCoordinates(int tableNumber, string[,] grid)
        {
            // Iterate through the grid to locate the given table number
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y, grid);
                    if (!string.IsNullOrEmpty(number) && int.Parse(number) == tableNumber)
                    {
                        return (x, y); // Return the coordinates of the table
                    }
                }
            }

            throw new Exception($"Table {tableNumber} not found in the grid."); // Error if table not found
        }

        public string GetNumberAt(int x, int y, string[,] grid)
        {
            if (y < 0 || y >= grid.GetLength(0) || x < 0 || x >= grid.GetLength(1)) return null;

            string number = "";
            while (x < grid.GetLength(1) && char.IsDigit(grid[y, x]))
            {
                number += grid[y, x];
                x++;
            }

            return number;
        }

        public Dictionary<int, ConsoleColor> DetermineTableColors(
            int[] activeTables,
            int[] inactiveTables,
            int[] reservedTables,
            int guestCount,
            string[,] grid,
            bool isAdminMode)
        {
            var tableColors = new Dictionary<int, ConsoleColor>();

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y, grid);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);

                        if (isAdminMode)
                        {
                            tableColors[tableNumber] = Array.Exists(activeTables, t => t == tableNumber)
                                ? ConsoleColor.Green
                                : ConsoleColor.Red;
                        }
                        else
                        {
                            if (Array.Exists(reservedTables, t => t == tableNumber))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Reserved
                            }
                            else if (Array.Exists(inactiveTables, t => t == tableNumber))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Inactive
                            }
                            else if (!IsTableValidForGuests(tableNumber, guestCount, activeTables))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Invalid size
                            }
                            else
                            {
                                tableColors[tableNumber] = ConsoleColor.Green; // Available
                            }
                        }
                    }
                }
            }

            return tableColors;
        }

        private bool IsTableValidForGuests(int tableNumber, int guestCount, int[] activeTables)
        {
            if (!Array.Exists(activeTables, t => t == tableNumber))
                return false;

            var table = Access.Places.Read().FirstOrDefault(p => p.ID == tableNumber);
            if (table == null || table.Active == 0) return false;

            return guestCount switch
            {
                1 or 2 => tableNumber == 1 || tableNumber == 4 || tableNumber == 5 || tableNumber == 8 || tableNumber == 9 || tableNumber == 11 || tableNumber == 12 || tableNumber == 15,
                3 or 4 => tableNumber == 6 || tableNumber == 7 || tableNumber == 10 || tableNumber == 13 || tableNumber == 14,
                5 or 6 => tableNumber == 2 || tableNumber == 3,
                _ => false
            };
        }
    }
}