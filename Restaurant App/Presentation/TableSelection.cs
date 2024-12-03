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

            // Traverse the grid and apply color to tables dynamically
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1; // Skip to the end of the number

                        // Determine the table's color based on availability
                        ConsoleColor color = ConsoleColor.Gray;
                        if (Array.Exists(availableTables, table => table == tableNumber))
                        {
                            color = ConsoleColor.Green; // Available tables in green
                        }
                        else if (Array.Exists(reservedTables, table => table == tableNumber))
                        {
                            color = ConsoleColor.Red; // Reserved or unavailable tables in red
                        }
                        else
                        {
                            color = ConsoleColor.Red; // Unsuitable tables in red
                        }

                        // Apply color to the entire table borders and number
                        ColorTableBordersAndNumber(x, y, tableNumber, color);
                    }
                }
            }

            // Draw the full grid
            for (int y = 0; y < grid.Length; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(grid[y]);
            }

            // Reset color only after the entire grid is rendered
            Console.ResetColor();
            HighlightNumber();
        }

        private void ColorTableBordersAndNumber(int numberX, int numberY, int tableNumber, ConsoleColor color)
        {
            // Locate the table's top-left corner by finding the nearest '+'
            int startX = numberX, startY = numberY;
            while (startX > 0 && grid[startY][startX] != '+') startX--;
            while (startY > 0 && grid[startY][startX] != '+') startY--;

            // Locate the table's bottom-right corner by finding the matching '+'
            int endX = startX, endY = startY;
            while (endY < grid.Length && grid[endY][endX] != '+' && endY < startY + 5) endY++;
            while (endX < grid[endY].Length && grid[endY][endX] != '+' && endX < startX + 10) endX++;

            // Apply color to borders and table ID only
            Console.ForegroundColor = color;
            for (int y = startY; y <= endY && y < Console.BufferHeight; y++)
            {
                for (int x = startX; x <= endX && x < Console.BufferWidth; x++)
                {
                    if (grid[y][x] == '+' || grid[y][x] == '-' || grid[y][x] == '|' || (x == numberX && y == numberY)) // Borders and table ID only
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(grid[y][x]);
                    }
                }
            }
            Console.ResetColor();
        }

        private void HighlightNumber()
        {
            Console.SetCursorPosition(cursorX, cursorY);
            Console.Write("X");
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

                string number = GetNumberAt(startX, y);
                if (!string.IsNullOrEmpty(number))
                {
                    return (startX, y);
                }
            }

            return (startX, startY);
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
                HighlightNumber();
            }
        }
    }
}