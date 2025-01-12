using App.DataAccess.Utils;
using Restaurant;

namespace App.Logic.Table
{
    public class TableSelectionLogic
    {
        public (int x, int y) FindTableCoordinates(int tableNumber, string[,] grid)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y, grid);
                    if (!string.IsNullOrEmpty(number) && int.Parse(number) == tableNumber)
                    {
                        return (x, y);
                    }
                }
            }
            throw new Exception($"Table {tableNumber} not found in the grid.");
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
                                tableColors[tableNumber] = ConsoleColor.Red;
                            }
                            else if (Array.Exists(inactiveTables, t => t == tableNumber))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red;
                            }
                            else if (!IsTableValidForGuests(tableNumber, guestCount, activeTables))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red;
                            }
                            else
                            {
                                tableColors[tableNumber] = ConsoleColor.Green;
                            }
                        }
                    }
                }
            }
            return tableColors;
        }

        public (int x, int y) FindFirstAvailableCoordinates(
            int[] activeTables,
            int[] inactiveTables,
            int[] reservedTables,
            int guestCount,
            string[,] grid)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y, grid);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        if (!Array.Exists(reservedTables, t => t == tableNumber) &&
                            !Array.Exists(inactiveTables, t => t == tableNumber) &&
                            IsTableValidForGuests(tableNumber, guestCount, activeTables))
                        {
                            return (x, y);
                        }
                    }
                }
            }
            return FindTableCoordinates(1, grid);
        }

        public bool IsTableValidForGuests(int tableNumber, int guestCount, int[] activeTables)
        {
            if (!Array.Exists(activeTables, t => t == tableNumber)) return false;

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

        public string ValidateTableState(
            int tableNumber,
            int guestCount,
            int[] activeTables,
            int[] reservedTables,
            int[] inactiveTables,
            bool isAdmin,
            out ConsoleColor tableColor)
        {
            bool isReserved = Array.Exists(reservedTables, t => t == tableNumber);
            bool isInactive = Array.Exists(inactiveTables, t => t == tableNumber);
            bool isActive = Array.Exists(activeTables, t => t == tableNumber);

            // Determine if the table size is valid
            bool isTooSmall = guestCount > GetMaxGuestsForTable(tableNumber);
            bool isTooBig = guestCount < GetMinGuestsForTable(tableNumber);

            string message = "";

            if (isReserved && isActive)
            {
                message = $"Table {tableNumber} is already reserved.\n";
                tableColor = ConsoleColor.Red;
                return message;
            }

            if (isInactive)
            {
                if (isTooSmall)
                {
                    message = $"Table {tableNumber} is too small for {guestCount} guests and inactive.\n";
                }
                else if (isTooBig)
                {
                    message = $"Table {tableNumber} is too big for {guestCount} guests and inactive.\n";
                }
                else
                {
                    message = $"Table {tableNumber} is inactive.\n";
                }
                tableColor = ConsoleColor.Red;
                return message;
            }

            if (isTooSmall)
            {
                message = $"Table {tableNumber} is too small for {guestCount} guests.\n";
                tableColor = ConsoleColor.Red;
                return message;
            }

            if (isTooBig)
            {
                message = $"Table {tableNumber} is too big for {guestCount} guests.\n";
                tableColor = ConsoleColor.Red;
                return message;
            }

            if (isActive && !isReserved)
            {
                tableColor = ConsoleColor.Green;
                return $"Table {tableNumber} can be reserved.\n";
            }

            tableColor = ConsoleColor.Red;
            return $"Table {tableNumber} is not available for selection.\n";
        }

        public int GetMaxGuestsForTable(int tableNumber)
        {
            return tableNumber switch
            {
                1 or 4 or 5 or 8 or 9 or 11 or 12 or 15 => 2,
                6 or 7 or 10 or 13 or 14 => 4,
                2 or 3 => 6,
                _ => 0
            };
        }

        public int GetMinGuestsForTable(int tableNumber)
        {
            return tableNumber switch
            {
                1 or 4 or 5 or 8 or 9 or 11 or 12 or 15 => 1,
                6 or 7 or 10 or 13 or 14 => 3,
                2 or 3 => 5,
                _ => int.MaxValue
            };
        }
    }
}
