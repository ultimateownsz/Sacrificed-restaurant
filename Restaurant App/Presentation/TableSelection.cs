using System;

namespace Presentation
{
    public class TableSelection
    {        
        private int previousX = -1;
        private int previousY = -1;

        private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
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

        private void ClearGrid()
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', grid.GetLength(1))); // Clear the entire row
            }
            Console.SetCursorPosition(0, 0); // Reset cursor to the top
        }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            ClearGrid(); // Clear the grid area

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1;

                        // Set table colors
                        if (Array.Exists(reservedTables, table => table == tableNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Reserved tables
                        }
                        else if (Array.Exists(availableTables, table => table == tableNumber))
                        {
                            Console.ForegroundColor = ConsoleColor.Green; // Available tables
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Unusable tables
                        }

                        Console.SetCursorPosition(x - (number.Length - 1), y);
                        Console.Write(number);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(x, y);
                        Console.Write(grid[y, x]);
                    }
                }
            }

            HighlightNumber(availableTables, reservedTables); // Highlight the selected table
        }





        private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor)
        {
            var token = flashCancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write("X ");
                await Task.Delay(500);

                if (token.IsCancellationRequested) break;

                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
                await Task.Delay(500);
            }

            if (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write("X ");
            }
            Console.ResetColor();
        }


        private void HighlightNumber(int[] availableTables, int[] reservedTables)
        {
            // Cancel the previous flashing task
            if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
            {
                flashCancellationTokenSource.Cancel();
                flashCancellationTokenSource.Dispose();
            }

            flashCancellationTokenSource = new CancellationTokenSource();

            string currentNumber = GetNumberAt(cursorX, cursorY);

            if (!string.IsNullOrEmpty(currentNumber))
            {
                int currentTable = int.Parse(currentNumber);

                ConsoleColor tableColor = ConsoleColor.Red; // Default to red
                if (Array.Exists(availableTables, table => table == currentTable))
                {
                    tableColor = ConsoleColor.Green; // Available table
                }

                // Start a flashing task for the current table
                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor);
            }

            Console.ResetColor();
        }






        private void RemoveHighlight(int x, int y)
        {
            string oldNumber = GetNumberAt(x, y);

            if (!string.IsNullOrEmpty(oldNumber))
            {
                int oldTable = int.Parse(oldNumber);

                // Restore the table number
                Console.SetCursorPosition(x, y);
                Console.ResetColor();
                Console.Write(oldTable.ToString().PadRight(2)); // Properly clear for double digits
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


        private bool IsDoubleDigit(int x, int y)
        {
            if (x < 0 || x >= grid.GetLength(1) || y < 0 || y >= grid.GetLength(0))
                return false;

            // Check if the current character and its neighbor form a two-digit number
            if (char.IsDigit(grid[y, x]) && x + 1 < grid.GetLength(1) && char.IsDigit(grid[y, x + 1]))
                return true;

            // Check if the current character is the second digit of a two-digit number
            if (char.IsDigit(grid[y, x]) && x - 1 >= 0 && char.IsDigit(grid[y, x - 1]))
                return true;

            return false;
        }



        private (int, int) FindNextNumberInRow(int startX, int startY, int direction)
        {
            int x = startX;

            // Adjust when moving left from a double-digit number
            if (direction == -1 && IsDoubleDigit(startX, startY))
            {
                string currentNumber = GetNumberAt(startX, startY);
                if (!string.IsNullOrEmpty(currentNumber) && currentNumber.Length == 2)
                {
                    x -= 1; // Align to the first digit of the current number
                }
            }
            // Adjust when moving right from a double-digit number
            else if (direction == 1 && IsDoubleDigit(startX, startY))
            {
                string currentNumber = GetNumberAt(startX, startY);
                if (!string.IsNullOrEmpty(currentNumber) && currentNumber.Length == 2)
                {
                    x += currentNumber.Length; // Move past the current number
                }
            }

            while (x >= 0 && x < grid.GetLength(1))
            {
                x += direction;

                string nextNumber = GetNumberAt(x, startY);
                if (!string.IsNullOrEmpty(nextNumber))
                {
                    if (direction == -1 && IsDoubleDigit(x, startY))
                    {
                        // Align to the first digit if moving left
                        x -= 1;
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

        private void UpdateTableHighlight(int prevX, int prevY, int currX, int currY, int[] availableTables, int[] reservedTables)
        {
            // Remove "X" from the previous position and restore the table number
            if (prevX != -1 && prevY != -1)
            {
                string prevNumber = GetNumberAt(prevX, prevY);
                if (!string.IsNullOrEmpty(prevNumber))
                {
                    int prevTable = int.Parse(prevNumber);

                    // Determine the color of the previous table
                    ConsoleColor prevColor = ConsoleColor.Red; // Default to red
                    if (Array.Exists(availableTables, table => table == prevTable))
                    {
                        prevColor = ConsoleColor.Green;
                    }

                    // Restore the previous table number
                    Console.SetCursorPosition(prevX, prevY);
                    Console.ForegroundColor = prevColor;
                    Console.Write(prevNumber.PadRight(2)); // Properly handle double digits
                }
            }

            // Highlight the current table with "X"
            string currNumber = GetNumberAt(currX, currY);
            if (!string.IsNullOrEmpty(currNumber))
            {
                int currTable = int.Parse(currNumber);

                // Determine the color of the "X" based on the current table's availability
                ConsoleColor currColor = ConsoleColor.Red; // Default to red
                if (Array.Exists(availableTables, table => table == currTable))
                {
                    currColor = ConsoleColor.Green;
                }

                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = currColor;
                Console.Write("X "); // Highlight with "X"
            }

            Console.ResetColor();
        }

        private void ResetConsoleToDefault()
        {
            Console.ResetColor();
            Console.Clear();
        }


        private void EnsureConsoleSize()
        {
            const int requiredWidth = 80; // Example width
            const int requiredHeight = 30; // Example height

            while (Console.WindowWidth < requiredWidth || Console.WindowHeight < requiredHeight)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Your console is too small to display the table blueprint.");
                Console.WriteLine($"Minimum required size: {requiredWidth}x{requiredHeight}");
                Console.WriteLine($"Current size: {Console.WindowWidth}x{Console.WindowHeight}");
                Console.ResetColor();
                Console.WriteLine("Please resize your console window and press Enter to continue...");
                Console.ReadLine();
            }

            Console.Clear();
        }

        private void StopFlashing()
        {
            if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
            {
                flashCancellationTokenSource.Cancel();
                flashCancellationTokenSource.Dispose();
                flashCancellationTokenSource = null; // Prevent further access
            }
        }


        public int SelectTable(int[] availableTables, int[] reservedTables)
        {
            EnsureConsoleSize(); // Ensure the console size is adequate
            ShowGrid(availableTables, reservedTables);
            Console.CursorVisible = false; // Hide the cursor

            int lastX = cursorX, lastY = cursorY;

            try
            {
                while (true)
                {
                    Console.SetCursorPosition(0, grid.GetLength(0) + 2);
                    Console.ResetColor();
                    Console.WriteLine("(B)ack");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                    {
                        StopFlashing(); // Ensure flashing stops
                        ResetConsoleToDefault(); // Reset colors and clean up screen
                        return -1; // Return -1 to indicate cancellation
                    }

                    int nextX = cursorX, nextY = cursorY;

                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            (nextX, nextY) = FindNextNumberInRow(cursorX, cursorY, 1);
                            break;
                        case ConsoleKey.LeftArrow:
                            (nextX, nextY) = FindNextNumberInRow(cursorX, cursorY, -1);
                            break;
                        case ConsoleKey.DownArrow:
                            (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY, 1);
                            break;
                        case ConsoleKey.UpArrow:
                            (nextX, nextY) = FindNextNumberInColumn(cursorX, cursorY, -1);
                            break;
                        case ConsoleKey.Enter:
                            string selectedNumber = GetNumberAt(cursorX, cursorY);

                            if (string.IsNullOrEmpty(selectedNumber))
                            {
                                Console.SetCursorPosition(0, grid.GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid table. Please select a valid table.");
                                Console.ResetColor();
                                continue; // Retry selection
                            }

                            int tableNumber = int.Parse(selectedNumber);

                            if (!Array.Exists(availableTables, table => table == tableNumber) ||
                                Array.Exists(reservedTables, table => table == tableNumber))
                            {
                                // Display the error message
                                Console.SetCursorPosition(0, grid.GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Table {tableNumber} is unavailable. Please select another table.");
                                Console.ResetColor();
                                continue;
                            }

                            SelectedTable = tableNumber;

                            StopFlashing(); // Stop flashing task
                            ResetConsoleToDefault(); // Reset colors and clean up screen
                            return SelectedTable; // Return the valid table number
                    }

                    // Ensure valid cursor movement
                    if (nextX < 0 || nextX >= grid.GetLength(1) || nextY < 0 || nextY >= grid.GetLength(0) ||
                        string.IsNullOrEmpty(GetNumberAt(nextX, nextY)))
                    {
                        nextX = lastX;
                        nextY = lastY;
                    }
                    else
                    {
                        // Clear error message when navigating
                        Console.SetCursorPosition(0, grid.GetLength(0) + 3);
                        Console.Write(new string(' ', Console.WindowWidth)); // Clear the error line

                        UpdateTableHighlight(lastX, lastY, nextX, nextY, availableTables, reservedTables); // Partial update
                        lastX = nextX;
                        lastY = nextY;
                        cursorX = nextX;
                        cursorY = nextY;

                        HighlightNumber(availableTables, reservedTables); // Dynamically update flashing
                    }
                }
            }
            finally
            {
                StopFlashing(); // Stop flashing and reset tasks
                ResetConsoleToDefault(); // Reset colors and clean up screen
                Console.CursorVisible = true; // Restore the cursor visibility
            }
        }

    }
}