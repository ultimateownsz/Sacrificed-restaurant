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
                        else if (Array.Exists(reservedTables, table => table == tableNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Reserved or unavailable tables in red
                        }
                        else
                        {
                            Console.ResetColor(); // Default color for other characters
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
            HighlightNumber();
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