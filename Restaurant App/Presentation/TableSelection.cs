using System;

namespace Presentation
{
    public class TableSelection
    {
        private string[] grid = {
            "+---------------------------------------------------+",
            "|     +---------------------------------------+     |",
            "|     |                  BAR                  |     |",
            "|     +---------------------------------------+     |",
            "|  ∩          ∩   ∩               ∩   ∩          ∩  |",
            "|+---+      +---+---+           +---+---+      +---+|",
            "|  1 |     C|   2   |ↄ         C|   3   |ↄ     | 4  |",
            "|+---+      +---+---+           +---+---+      +---+|",
            "|  u          u   u               u   u          u  |",
            "|                                                   |",
            "|  ∩           ∩                   ∩             ∩  |",
            "|+---+       +----+              +----+        +---+|",
            "÷  5 |      C| 6  |ↄ            C| 7  |ↄ       | 8  ÷",
            "|+---+       +----+              +----+        +---+|",
            "|  u           u                   u             u  |",
            "|                        ∩                          |",
            "|  ∩                   +----+                    ∩  |",
            "|+---+                C| 10 |ↄ                 +---+|",
            "÷  9 |                 +----+                  | 11 ÷",
            "|+---+                   u                     +---+|",
            "|  u                                             u  |",
            "|                                                   |",
            "|  ∩           ∩                   ∩             ∩  |",
            "|+---+       +----+              +----+        +---+|",
            "| 12 |      C| 13 |ↄ            C| 14 |ↄ       | 15 |",
            "|+---+       +----+              +----+        +---+|",
            "|  u           u        /  \\      u              u  |",
            "+----------------------+    +-----------------------+"
        };

        private int cursorX = 3, cursorY = 6; // Start at table "1"
        public int SelectedTable { get; private set; }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            Console.Clear();
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1; // Skip to the end of the number

                        // Determine color based on table availability
                        if (Array.Exists(availableTables, table => table == tableNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Green; // Available tables in green
                        }
                        else
                        {
                            // Tables not available for this guest count, or reserved
                            Console.ForegroundColor = Array.Exists(reservedTables, table => table == tableNumber) 
                                ? ConsoleColor.Red 
                                : ConsoleColor.Red; // Unsuitable or reserved in red
                        }
                        Console.Write(number);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(grid[y][x]);
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            HighlightNumber(availableTables, reservedTables);
        }

        private void HighlightNumber(int[] availableTables, int[] reservedTables)
        {
            string number = GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(number))
            {
                int tableNumber = int.Parse(number);

                // Determine the color of the X based on the table's availability
                if (Array.Exists(availableTables, table => table == tableNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Available
                }
                else if (Array.Exists(reservedTables, table => table == tableNumber))
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Reserved
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Unsuitable
                }

                // Highlight the first digit with "X"
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write("X");

                // Replace the second digit with a space if it's a two-digit number
                if (number.Length == 2)
                {
                    Console.SetCursorPosition(cursorX + 1, cursorY);
                    Console.Write(" ");
                }
            }
            Console.ResetColor();
        }

        private void RemoveHighlight()
        {
            string number = GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(number))
            {
                // Restore the original table number
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(number);

                // Restore the second digit if it's a two-digit number
                if (number.Length == 2)
                {
                    Console.SetCursorPosition(cursorX + 1, cursorY);
                    Console.Write(number[1]);
                }
            }
        }

        private string GetNumberAt(int x, int y)
        {
            string number = "";
            while (x < grid[y].Length && char.IsDigit(grid[y][x]))
            {
                number += grid[y][x];
                x++;
            }
            return number;
        }
        private (int, int) FindNextNumberInRow(int startX, int startY, int direction)
        {
            int x = startX;

            while (x >= 0 && x < grid[startY].Length)
            {
                x += direction;

                // Moving right: Skip to the first digit of the next table
                if (direction == 1 && x > 0 && x < grid[startY].Length &&
                    char.IsDigit(grid[startY][x]) && char.IsDigit(grid[startY][x - 1]))
                {
                    continue; // Skip the second digit
                }

                // Moving left: Skip to the first digit of the previous table
                if (direction == -1 && x + 1 < grid[startY].Length &&
                    char.IsDigit(grid[startY][x]) && char.IsDigit(grid[startY][x + 1]))
                {
                    x--; // Move to the first digit
                }

                // Ensure the new position is valid
                if (x >= 0 && x < grid[startY].Length && char.IsDigit(grid[startY][x]))
                {
                    return (x, startY); // Found a valid digit
                }
            }

            // If no valid position is found, return the current position
            return (startX, startY);
        }



        private (int, int) FindNextNumberInColumn(int startX, int startY, int direction)
        {
            int y = startY;

            while (y >= 0 && y < grid.Length)
            {
                y += direction;

                // Prevent out-of-bound access
                if (y < 0 || y >= grid.Length)
                {
                    break;
                }

                // Scan horizontally to find the nearest valid digit
                for (int offset = 0; offset <= 2; offset++)
                {
                    if (startX - offset >= 0 && char.IsDigit(grid[y][startX - offset]))
                    {
                        return (startX - offset, y);
                    }
                    if (startX + offset < grid[y].Length && char.IsDigit(grid[y][startX + offset]))
                    {
                        return (startX + offset, y);
                    }
                }
            }

            // If no valid position is found, return the current position
            return (startX, startY);
        }



        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables); // Display the grid
            Console.CursorVisible = false;

            // Track the last valid position
            int lastX = cursorX, lastY = cursorY;

            while (true)
            {
                var key = Console.ReadKey(true);
                RemoveHighlight();

                try
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            (int nextX, int nextY) = FindNextNumberInRow(cursorX, cursorY, 1);
                            cursorX = nextX;
                            cursorY = nextY;
                            break;
                        case ConsoleKey.LeftArrow:
                            (nextX, nextY) = FindNextNumberInRow(cursorX, cursorY, -1);
                            cursorX = nextX;
                            cursorY = nextY;
                            break;
                        case ConsoleKey.DownArrow:
                            (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY, 1);
                            cursorX = nextX;
                            cursorY = nextY;
                            break;
                        case ConsoleKey.UpArrow:
                            (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY, -1);
                            cursorX = nextX;
                            cursorY = nextY;
                            break;
                        case ConsoleKey.Enter:
                            string selectedNumber = GetNumberAt(cursorX, cursorY);
                            if (!string.IsNullOrEmpty(selectedNumber))
                            {
                                SelectedTable = int.Parse(selectedNumber);
                                return SelectedTable;
                            }
                            break;
                        case ConsoleKey.Escape:
                            return -1;
                    }

                    // Reset to the last valid position if out of bounds
                    if (cursorX < 0 || cursorX >= grid[0].Length || cursorY < 0 || cursorY >= grid.Length || string.IsNullOrEmpty(GetNumberAt(cursorX, cursorY)))
                    {
                        cursorX = lastX;
                        cursorY = lastY;
                    }
                    else
                    {
                        lastX = cursorX;
                        lastY = cursorY; // Update the last valid position
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    // Handle any edge-case errors gracefully
                    cursorX = lastX;
                    cursorY = lastY;
                }

                ShowGrid(availableTables, reservedTables); // Redraw the grid
            }
        }

    }
}
