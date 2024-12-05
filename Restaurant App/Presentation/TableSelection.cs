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
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','5',' ','|',' ',' ',' ',' ',' ',' ','C','|',' ','6',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','7',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ','|',' ','8',' ',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','9',' ','|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','0',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',' ','1','1',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ','1','2',' ','|',' ',' ',' ',' ',' ',' ','C','|',' ','1','3',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','4',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ','|',' ','1','5',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ','/',' ',' ','\\',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',' ',' ',' ',' ','+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+'}
        };

private int cursorX = 3, cursorY = 6; // Start at table "1"
        public int SelectedTable { get; private set; }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            Console.Clear();

            // Display the grid
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    char currentChar = grid[y, x];
                    if (char.IsDigit(currentChar))
                    {
                        int tableNumber = int.Parse(GetNumberAt(x, y));
                        
                        // Determine color based on table availability
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
                            Console.ForegroundColor = ConsoleColor.Red; // Unavailable
                        }

                        Console.Write(tableNumber);

                        // Adjust the x position for multi-digit numbers
                        x += GetNumberAt(x, y).Length - 1;
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(currentChar);
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

                // Determine color for highlighting
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
                    Console.ForegroundColor = ConsoleColor.Red; // Unavailable
                }

                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write("X");
                Console.ResetColor();
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

        private void RemoveHighlight()
        {
            string number = GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(number))
            {
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(number);

                if (number.Length == 2)
                {
                    Console.SetCursorPosition(cursorX + 1, cursorY);
                    Console.Write(number[1]);
                }
            }
        }

        private (int, int) FindNextPosition(int directionX, int directionY)
        {
            int x = cursorX, y = cursorY;

            while (y >= 0 && y < grid.GetLength(0))
            {
                y += directionY;
                x += directionX;

                if (x >= 0 && x < grid.GetLength(1) && char.IsDigit(grid[y, x]))
                {
                    return (x, y);
                }
            }

            return (cursorX, cursorY);
        }

        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables);
            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, grid.GetLength(0) + 2);
                Console.WriteLine("(B)ack");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                {
                    return -1;
                }

                RemoveHighlight();

                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        (cursorX, cursorY) = FindNextPosition(1, 0);
                        break;
                    case ConsoleKey.LeftArrow:
                        (cursorX, cursorY) = FindNextPosition(-1, 0);
                        break;
                    case ConsoleKey.DownArrow:
                        (cursorX, cursorY) = FindNextPosition(0, 1);
                        break;
                    case ConsoleKey.UpArrow:
                        (cursorX, cursorY) = FindNextPosition(0, -1);
                        break;
                    case ConsoleKey.Enter:
                        string selectedNumber = GetNumberAt(cursorX, cursorY);
                        if (!string.IsNullOrEmpty(selectedNumber))
                        {
                            int tableNumber = int.Parse(selectedNumber);

                            if (!Array.Exists(availableTables, table => table == tableNumber) ||
                                Array.Exists(reservedTables, table => table == tableNumber))
                            {
                                Console.SetCursorPosition(0, grid.GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Table {tableNumber} is unavailable. Please select another table.");
                                Console.ResetColor();
                                continue;
                            }

                            SelectedTable = tableNumber;
                            return SelectedTable;
                        }
                        break;
                }

                ShowGrid(availableTables, reservedTables);
            }
        }
    }
}