using System;

namespace Presentation
{
    public class TableSelection
    {        

        private char[,] grid = {
            {'+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',},
            {'|',' ',' ',' ',' ',' ','+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ','|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','B','A','R',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ','+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ','+','-','-','-','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','-','-','-','+',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','1',' ','|',' ',' ',' ',' ',' ','C','|',' ',' ',' ','2',' ',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ',' ',' ','3',' ',' ',' ','|','ↄ',' ',' ',' ',' ',' ','|',' ','4',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ','+','-','-','-','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','-','-','-','+',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','5',' ','|',' ',' ',' ',' ',' ',' ',' ','C','|',' ','6',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','7',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ','|',' ','8',' ',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','9',' ','|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','0',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',' ','1','1',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ','1','2',' ','|',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','3',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','4',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ','|',' ','1','5',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ','/',' ',' ','\\',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',' ',' ',' ',' ','+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+'}
        };

        private Dictionary<int, (int x, int y)> tableLocations = new();
        private int currentTableId = 1; // Always start on table 1
        public int SelectedTable { get; private set; }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            // Dynamically find table locations before rendering the grid
            FindTableLocations();

            Console.Clear();

            // Display the grid
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);

                        // Highlight the current table with "X "
                        if (tableNumber == currentTableId)
                        {
                            Console.ForegroundColor = GetTableColor(availableTables, reservedTables, tableNumber);
                            Console.Write("X ");
                            x += number.Length - 1; // Skip over multi-digit numbers
                        }
                        else
                        {
                            Console.ForegroundColor = GetTableColor(availableTables, reservedTables, tableNumber);
                            Console.Write(number);
                            x += number.Length - 1; // Skip over multi-digit numbers
                        }
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(grid[y, x]);
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }

        private void FindTableLocations()
        {
            tableLocations.Clear(); // Clear previous locations

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (char.IsDigit(grid[y, x]))
                    {
                        string number = GetNumberAt(x, y);
                        if (int.TryParse(number, out int tableNumber))
                        {
                            tableLocations[tableNumber] = (x, y);
                            x += number.Length - 1; // Skip to the end of the number
                        }
                    }
                }
            }
        }

        private string GetNumberAt(int x, int y)
        {
            string number = "";
            while (x < grid.GetLength(1) && char.IsDigit(grid[y, x]))
            {
                number += grid[y, x];
                x++;
            }
            return number;
        }

        private ConsoleColor GetTableColor(int[] availableTables, int[] reservedTables, int tableNumber)
        {
            if (Array.Exists(availableTables, table => table == tableNumber))
                return ConsoleColor.Green; // Available
            if (Array.Exists(reservedTables, table => table == tableNumber))
                return ConsoleColor.Red; // Reserved
            return ConsoleColor.Red; // Unavailable
        }

        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables);

            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, grid.GetLength(0) + 2);
                Console.ResetColor();
                Console.WriteLine("(B)ack");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                {
                    return -1; // Return to the previous menu
                }

                int previousTableId = currentTableId;

                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        currentTableId = FindNextTable(previousTableId, 1, 0);
                        break;
                    case ConsoleKey.LeftArrow:
                        currentTableId = FindNextTable(previousTableId, -1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        currentTableId = FindNextTable(previousTableId, 0, 1);
                        break;
                    case ConsoleKey.UpArrow:
                        currentTableId = FindNextTable(previousTableId, 0, -1);
                        break;
                    case ConsoleKey.Enter:
                        if (Array.Exists(availableTables, table => table == currentTableId) &&
                            !Array.Exists(reservedTables, table => table == currentTableId))
                        {
                            SelectedTable = currentTableId;
                            return SelectedTable;
                        }
                        Console.SetCursorPosition(0, grid.GetLength(0) + 3);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Table {currentTableId} is unavailable. Please select another table.");
                        Console.ResetColor();
                        continue;
                }

                ShowGrid(availableTables, reservedTables);
            }
        }

        private int FindNextTable(int currentTableId, int directionX, int directionY)
        {
            if (!tableLocations.ContainsKey(currentTableId))
            {
                return currentTableId;
            }

            (int currentX, int currentY) = tableLocations[currentTableId];

            while (true)
            {
                int newX = currentX + directionX;
                int newY = currentY + directionY;

                if (newX < 0 || newY < 0 || newY >= grid.GetLength(0) || newX >= grid.GetLength(1))
                {
                    return currentTableId; // Out of bounds, return the current table ID
                }

                string number = GetNumberAt(newX, newY);

                // Check if it's a double-digit number
                if (int.TryParse(number, out int nextTableId))
                {
                    // If double-digit, ensure both digits are part of the same number
                    if (number.Length == 2 && newX + 1 < grid.GetLength(1) && grid[newY, newX + 1] == number[1])
                    {
                        return nextTableId;
                    }
                    return nextTableId;
                }

                currentX = newX;
                currentY = newY;
            }
        }
    }
}