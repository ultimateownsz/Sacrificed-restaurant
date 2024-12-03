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
        }
        else
        {
            Console.ResetColor();
        }

        Console.SetCursorPosition(cursorX, cursorY);
        Console.Write("X"); // Draw the highlighted X
        Console.ResetColor();
    }


        private void RemoveHighlight()
        {
            string number = GetNumberAt(cursorX, cursorY);
            if (!string.IsNullOrEmpty(number))
            {
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(number);
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

            // Adjust for double-digit tables when moving left
            if (direction == -1 && x > 0 && char.IsDigit(grid[startY][x]) && char.IsDigit(grid[startY][x - 1]))
            {
                x--; // Move to the first digit of the double-digit table
            }

            string number = GetNumberAt(x, startY);
            if (!string.IsNullOrEmpty(number))
            {
                return (x, startY);
            }
        }

        return (startX, startY);
    }


    private (int, int) FindNextNumberInColumn(int startX, int startY, int direction)
    {
        int y = startY;

        while (y >= 0 && y < grid.Length)
        {
            y += direction;

            // Scan horizontally to find the nearest valid number
            for (int offset = 0; offset <= 2; offset++)
            {
                // Check left and right from the current X position
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

        return (startX, startY); // If no valid number is found, return the current position
    }


        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables); // Ensure the grid is displayed before selection starts
            Console.CursorVisible = false;

            while (true)
            {
                var key = Console.ReadKey(true);
                RemoveHighlight();

                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        (int nextX, int nextY) = FindNextNumberInRow(cursorX + GetNumberAt(cursorX, cursorY).Length, cursorY, 1);
                        cursorX = nextX;
                        cursorY = nextY;
                        break;
                    case ConsoleKey.LeftArrow:
                        (nextX, nextY) = FindNextNumberInRow(cursorX - 1, cursorY, -1);
                        cursorX = nextX;
                        cursorY = nextY;
                        break;
                    case ConsoleKey.DownArrow:
                        (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY + 1, 1);
                        cursorX = nextX;
                        cursorY = nextY;
                        break;
                    case ConsoleKey.UpArrow:
                        (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY - 1, -1);
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

                ShowGrid(availableTables, reservedTables); // Redraw the grid on every key press
            }
        }
    }
}
