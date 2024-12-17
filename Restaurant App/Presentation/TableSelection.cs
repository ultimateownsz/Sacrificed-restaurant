using System;
using System.Runtime.InteropServices;
using Project;

namespace Presentation
{
    public class TableSelection
    {        
        private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
        private int cursorX, cursorY;
        private Dictionary<int, ConsoleColor> tableColors = new Dictionary<int, ConsoleColor>();

        public int SelectedTable { get; private set; }

        private void ClearGrid()
        {
            for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', GridPresent.GetGrid().GetLength(1))); // Clear the entire row
            }
            Console.SetCursorPosition(0, 0); // Reset cursor to the top
        }

        private (int x, int y) FindTableCoordinates(int tableNumber)
        {
            // Iterate through the grid to locate the given table number
            for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
            {
                for (int x = 0; x < GridPresent.GetGrid().GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number) && int.Parse(number) == tableNumber)
                    {
                        return (x, y); // Return the coordinates of the table
                    }
                }
            }

            throw new Exception($"Table {tableNumber} not found in the grid."); // Error if table not found
        }

       public void ShowGrid(int[] activeTables, int[] inactiveTables, bool isAdminMode = false)
        {
            tableColors.Clear(); 
            ClearGrid(); 

            for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
            {
                for (int x = 0; x < GridPresent.GetGrid().GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1;

                        // Admin Mode - Highlight active or inactive tables
                        if (isAdminMode)
                        {
                            tableColors[tableNumber] = Array.Exists(activeTables, t => t == tableNumber)
                                ? ConsoleColor.Green
                                : ConsoleColor.Red;
                        }
                        else
                        {
                            // Default behavior for reservations
                            tableColors[tableNumber] = ConsoleColor.Red;
                        }

                        Console.SetCursorPosition(x - (number.Length - 1), y);
                        Console.ForegroundColor = tableColors[tableNumber];
                        Console.Write(number);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.SetCursorPosition(x, y);
                        Console.Write(GridPresent.GetGrid()[y, x]);
                    }
                }
            }

            // Place "X" on the first table
            (cursorX, cursorY) = FindTableCoordinates(1);
            HighlightNumber(activeTables, inactiveTables);
        }



        private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor, int[] availableTables, int[] reservedTables)
        {
            var token = flashCancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
                await Task.Delay(500);

                if (token.IsCancellationRequested) break;

                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write("X ");
                await Task.Delay(500);
            }

            if (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
            }

            Console.ResetColor();
        }

        private void HighlightNumber(int[] activeTables, int[] inactiveTables, bool isAdmin = false, int guestCount = 0)
        {
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

                // Use pre-stored color for the table
                ConsoleColor tableColor = tableColors.ContainsKey(currentTable)
                    ? tableColors[currentTable]
                    : ConsoleColor.Red; // Default to red if no color is found

                // Start flashing the "X" with the table's current color
                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor, activeTables, inactiveTables);
            }
        }

        private bool IsTableValidForGuests(int tableNumber, int guestCount, int[] activeTables)
        {
            // Validate the table size for the given guest count
            if (!Array.Exists(activeTables, t => t == tableNumber))
                return false;

            var table = Access.Places.Read().FirstOrDefault(p => p.ID == tableNumber);
            if (table == null || table.Active == 0) return false;

            return guestCount switch
            {
                1 or 2 => tableNumber == 1 || tableNumber == 4 || tableNumber == 5 || tableNumber == 8 || tableNumber == 9 || tableNumber == 11 || tableNumber == 12 || tableNumber == 15,
                3 or 4 => tableNumber == 6 || tableNumber == 7 || tableNumber == 10 || tableNumber == 13 || tableNumber == 14,
                5 or 6 => tableNumber == 2 || tableNumber == 3,
                _ => false
            };
        }

        private string GetNumberAt(int x, int y) 
        { 
            if (y < 0 || y >= GridPresent.GetGrid().GetLength(0) || x < 0 || x >= GridPresent.GetGrid().GetLength(1)) return null;

            string number = "";
            while (x < GridPresent.GetGrid().GetLength(1) && char.IsDigit(GridPresent.GetGrid()[y, x]))
            {
                number += GridPresent.GetGrid()[y, x];
                x++;
            }

            return number;
        }


        private bool IsDoubleDigit(int x, int y)
        {
            if (x < 0 || x >= GridPresent.GetGrid().GetLength(1) || y < 0 || y >= GridPresent.GetGrid().GetLength(0))
                return false;

            // Check if the current character and its neighbor form a two-digit number
            if (char.IsDigit(GridPresent.GetGrid()[y, x]) && x + 1 < GridPresent.GetGrid().GetLength(1) && char.IsDigit(GridPresent.GetGrid()[y, x + 1]))
                return true;

            // Check if the current character is the second digit of a two-digit number
            if (char.IsDigit(GridPresent.GetGrid()[y, x]) && x - 1 >= 0 && char.IsDigit(GridPresent.GetGrid()[y, x - 1]))
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

            while (x >= 0 && x < GridPresent.GetGrid().GetLength(1))
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

            while (y >= 0 && y < GridPresent.GetGrid().GetLength(0))
            {
                y += direction;

                if (y < 0 || y >= GridPresent.GetGrid().GetLength(0)) break;

                for (int offset = -1; offset <= 1; offset++)
                {
                    int x = startX + offset;

                    if (x >= 0 && x < GridPresent.GetGrid().GetLength(1) && char.IsDigit(GridPresent.GetGrid()[y, x]))
                    {
                        if (x < GridPresent.GetGrid().GetLength(1) - 1 && char.IsDigit(GridPresent.GetGrid()[y, x + 1]))
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
            // Restore the previous table's original color
            if (prevX != -1 && prevY != -1)
            {
                string prevNumber = GetNumberAt(prevX, prevY);
                if (!string.IsNullOrEmpty(prevNumber))
                {
                    int prevTable = int.Parse(prevNumber);
                    if (tableColors.TryGetValue(prevTable, out ConsoleColor prevColor))
                    {
                        Console.SetCursorPosition(prevX, prevY);
                        Console.ForegroundColor = prevColor;
                        Console.Write(prevNumber.PadRight(2));
                    }
                }
            }

            // Highlight the current table with "X"
            string currNumber = GetNumberAt(currX, currY);
            if (!string.IsNullOrEmpty(currNumber))
            {
                int currTable = int.Parse(currNumber);

                // Determine the color for "X" based on table status
                ConsoleColor currColor = ConsoleColor.Red; // Default red
                if (Array.Exists(availableTables, table => table == currTable) && !Array.Exists(reservedTables, table => table == currTable))
                {
                    currColor = ConsoleColor.Green; // Available and not reserved
                }

                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = currColor;
                Console.Write("X ");
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

            // Try to maximize the console window
            // MaximizeConsoleWindow();

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

        // public static void MaximizeConsoleWindow()
        // {
        //     const int SW_MAXIMIZE = 3;

        //     // Import Windows API functions
        //     [DllImport("kernel32.dll", SetLastError = true)]
        //     static extern IntPtr GetConsoleWindow();

        //     [DllImport("user32.dll", SetLastError = true)]
        //     static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //     IntPtr consoleWindow = GetConsoleWindow();
        //     if (consoleWindow != IntPtr.Zero)
        //     {
        //         ShowWindow(consoleWindow, SW_MAXIMIZE);
        //     }
        // }

            private void StopFlashing()
            {
                if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
                {
                    flashCancellationTokenSource.Cancel();
                    flashCancellationTokenSource.Dispose();
                    flashCancellationTokenSource = null; // Prevent further access
                }
            }


            public int SelectTable(int[] activeTables, int[] inactiveTables, bool isAdmin = false)
            {
                EnsureConsoleSize(); // Ensure the console size is adequate
                ShowGrid(activeTables, inactiveTables, isAdminMode: true); // Show tables as active/inactive
                Console.CursorVisible = false; // Hide the cursor

                int lastX = cursorX, lastY = cursorY;

                try
                {
                    while (true)
                    {
                        Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 2);
                        Console.ResetColor();
                        Console.WriteLine("(B)ack");

                        var key = Console.ReadKey(true);

                        if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                        {
                            StopFlashing();
                            ResetConsoleToDefault();
                            return -1; // Exit and return "Back"
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
                                if (string.IsNullOrEmpty(selectedNumber)) continue;

                                int tableNumber = int.Parse(selectedNumber);
                                return tableNumber; // Return the selected table number
                        }

                        // Update the table highlight after moving
                        UpdateTableHighlight(lastX, lastY, nextX, nextY, activeTables, inactiveTables);
                        lastX = nextX;
                        lastY = nextY;
                        cursorX = nextX;
                        cursorY = nextY;

                        HighlightNumber(activeTables, inactiveTables); // Update the highlight dynamically
                    }
                }
                finally
                {
                    StopFlashing();
                    ResetConsoleToDefault();
                    Console.CursorVisible = true;
                }
            }
    }
}