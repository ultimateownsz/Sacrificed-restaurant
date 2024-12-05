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
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','5',' ','|',' ',' ',' ',' ',' ',' ',' ','C','|',' ','6',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','7',' ',' ','|','ↄ',' ',' ',' ',' ',' ',' ','|',' ','8',' ',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'÷',' ',' ','9',' ','|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','0',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',' ','1','1',' ','÷',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'|',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','|',},
            {'|',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','∩',' ',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ','1','2',' ','|',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','3',' ','|','ↄ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','C','|',' ','1','4',' ','|','ↄ',' ',' ',' ',' ',' ',' ','|',' ','1','5',' ','|',},
            {'|','+','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','-','+',' ',' ',' ',' ',' ',' ',' ','+','-','-','-','+','|',},
            {'|',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ','/',' ',' ','\\',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','u',' ',' ','|',},
            {'+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+',' ',' ',' ',' ','+','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','-','+'}
        };

        private int cursorX = 3, cursorY = 6; // Start at table "1"
        public int SelectedTable { get; private set; }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            Console.Clear();

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1;

                        if (Array.Exists(availableTables, table => table == tableNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = Array.Exists(reservedTables, table => table == tableNumber)
                                ? ConsoleColor.Red
                                : ConsoleColor.Red;
                        }
                        Console.Write(number);
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
            HighlightNumber(availableTables,
            reservedTables);
        }

        private void HighlightNumber(int[] availableTables, int[] reservedTables)
        {
            string number = GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(number))
            {
                int tableNumber = int.Parse(number);

                // Debug: Print the table ID
                Console.SetCursorPosition(0, grid.GetLength(0) + 4); // Position below the grid
                Console.ResetColor();
                Console.WriteLine($"DEBUG: Highlighting Table {tableNumber} at ({cursorX}, {cursorY})");

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
                Console.SetCursorPosition(cursorX, cursorY);
                Console.Write(number);

                if (number.Length == 2)
                {
                    Console.SetCursorPosition(cursorX + 1, cursorY);
                    Console.Write(number[1]);
                }
            }
        }

        private string GetNumberAt(int x, int y)
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


        private (int, int) FindNextNumberInRow(int startX, int startY, int direction)
        {
            int x = startX;

            // If we're currently on a two-digit number, skip the second digit when moving right
            string currentNumber = GetNumberAt(startX, startY);
            if (!string.IsNullOrEmpty(currentNumber) && currentNumber.Length == 2 && direction == 1)
            {
                x += currentNumber.Length; // Move past the current two-digit number
            }

            while (x >= 0 && x < grid.GetLength(1))
            {
                x += direction;

                string nextNumber = GetNumberAt(x, startY);
                if (!string.IsNullOrEmpty(nextNumber))
                {
                    // Always align to the first digit of a double-digit number
                    if (nextNumber.Length == 2 && direction == -1)
                    {
                        x -= 1; // Move back to the first digit
                    }
                    return (x, startY); // Found a valid number
                }
            }

            return (startX, startY); // Return the original position if no number is found
        }


        private (int, int) FindNextNumberInColumn(int startX, int startY, int direction)
        {
            int y = startY;

            while (y >= 0 && y < grid.GetLength(0))
            {
                y += direction;

                if (y < 0 || y >= grid.GetLength(0)) break;

                for (int offset = -1; offset <= 1; offset++)
                {
                    int x = startX + offset;

                    if (x >= 0 && x < grid.GetLength(1) && char.IsDigit(grid[y, x]))
                    {
                        if (x < grid.GetLength(1) - 1 && char.IsDigit(grid[y, x + 1]))
                        {
                            return (x, y);
                        }

                        return (x, y);
                    }
                }
            }

            return (startX, startY);
        }

        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            ShowGrid(availableTables, reservedTables);
            Console.CursorVisible = false;

            int lastX = cursorX, lastY = cursorY;

            while (true)
            {
                Console.SetCursorPosition(0, grid.GetLength(0) + 2);
                Console.ResetColor();
                Console.WriteLine("(B)ack");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                {
                    return -1;
                }

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

                    if (cursorX < 0 || cursorX >= grid.GetLength(1) || cursorY < 0 || cursorY >= grid.GetLength(0) ||
                        string.IsNullOrEmpty(GetNumberAt(cursorX, cursorY)))
                    {
                        cursorX = lastX;
                        cursorY = lastY;
                    }
                    else
                    {
                        lastX = cursorX;
                        lastY = cursorY;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    cursorX = lastX;
                    cursorY = lastY;
                }

                ShowGrid(availableTables, reservedTables);
            }
        }
    }
}