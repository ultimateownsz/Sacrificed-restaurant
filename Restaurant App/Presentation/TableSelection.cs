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

        public void ShowGrid()
        {
            Console.Clear();
            foreach (var line in grid)
            {
                Console.WriteLine(line);
            }
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
            Console.SetCursorPosition(cursorX, cursorY);
            Console.Write(number);
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

                if (x >= 0 && x < grid[startY].Length && char.IsDigit(grid[startY][x]))
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

                if (y >= 0 && y < grid.Length && char.IsDigit(grid[y][startX]))
                {
                    return (startX, y);
                }
            }

            return (startX, startY);
        }

        public int SelectTable()
        {
            Console.CursorVisible = false;
            ShowGrid();

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
                        SelectedTable = int.Parse(GetNumberAt(cursorX, cursorY));
                        return SelectedTable;
                    case ConsoleKey.Escape:
                        return -1;
                }

                HighlightNumber();
            }
        }
    }
}