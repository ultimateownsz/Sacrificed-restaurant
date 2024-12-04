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
            "|                                                   |",
            "|  ∩                     ∩                       ∩  |",
            "|+---+                 +----+                  +---+|",
            "÷  9 |                C| 10 |ↄ                 | 11 ÷",
            "|+---+                 +----+                  +---+|",
            "|  u                     u                       u  |",
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

            // Display the grid
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

            // Highlight the selected table
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
                    Console.SetCursorPosition(cursorX + 1, cursorY); // Move to the second digit
                    Console.Write(" "); // Replace the second digit with a space
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

                // Restore the second digit if it's a double-digit number
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

        //AFBLIJVEN! IK HEB 4 UUR GEDEBUGGED EN DIT WAS DE ENIGE OPLOSSING.
        // A.F.B.L.I.J.V.E.N.
        private (int, int) FindNextNumberInRowLeft(int startX, int startY)
        {
            int x = startX;

            while (x >= 0)
            {
                x--; // Move left

                if (x >= 0 && char.IsDigit(grid[startY][x]))
                {
                    // If we encounter the second digit of a two-digit number, move back to the first digit
                    if (x > 0 && char.IsDigit(grid[startY][x - 1]))
                    {
                        x--; // Align to the first digit
                    }
                    return (x, startY); // Found a valid number
                }
            }

            return (startX, startY); // No valid number, stay in place
        }

        private (int, int) FindNextNumberInRow(int startX, int startY, int direction)
        {
            int x = startX;
            string currentNumber = GetNumberAt(startX, startY);

            // If we're currently on a two-digit number, skip the second digit
            if (!string.IsNullOrEmpty(currentNumber) && currentNumber.Length == 2)
            {
                // Moving right: Skip past the two digits
                if (direction == 1)
                {
                    x += currentNumber.Length; // Move past the two digits
                }
                // Moving left: Reset to the first digit of the current number
                else if (direction == -1)
                {
                    x -= 1; // Ensure we're at the first digit
                }
            }

            while (x >= 0 && x < grid[startY].Length)
            {
                x += direction;

                // Ensure the new position is valid
                string nextNumber = GetNumberAt(x, startY);
                if (!string.IsNullOrEmpty(nextNumber))
                {
                    // Moving left: Ensure we land on the first digit of a two-digit number
                    if (direction == -1 && nextNumber.Length == 2)
                    {
                        x -= 1; // Align cursor to the first digit
                    }
                    return (x, startY); // Found a valid digit
                }
            }

            return (startX, startY); // No valid position found, stay in place
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
                for (int offset = -1; offset <= 1; offset++) // Check adjacent horizontal offsets
                {
                    if (startX + offset >= 0 && startX + offset < grid[y].Length &&
                        char.IsDigit(grid[y][startX + offset]))
                    {
                        int targetX = startX + offset;

                        // If landing on a two-digit number, align to the first digit
                        if (targetX < grid[y].Length - 1 && char.IsDigit(grid[y][targetX + 1]))
                        {
                            return (targetX, y); // Align to the first digit
                        }

                        return (targetX, y); // Found a valid digit
                    }
                }
            }

            return (startX, startY); // No valid table found, stay in place
        }


        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables); // Display the grid
            Console.CursorVisible = false;

            int lastX = cursorX, lastY = cursorY;

            while (true)
            {
                Console.SetCursorPosition(0, grid.Length + 2); // Position below the grid
                Console.ResetColor();
                Console.WriteLine("(B)ack");

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
                            (nextX, nextY) = FindNextNumberInRowLeft(cursorX, cursorY);
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
                                return SelectedTable; // Table selected
                            }
                            break;
                        case ConsoleKey.B: // Handle Back option
                            return -1; // Special code for "Back"
                        case ConsoleKey.Escape: // Optional escape handling
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
                    cursorX = lastX;
                    cursorY = lastY; // Recover from any exceptions
                }

                ShowGrid(availableTables, reservedTables); // Redraw the grid
            }
        }


    }
}
