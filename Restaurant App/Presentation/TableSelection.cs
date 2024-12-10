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

        public void ShowGrid(int[] activeTables, int[] inactiveTables)
        {
            tableColors.Clear(); // Clear the previous color mappings
            ClearGrid(); // Clear the grid area

            // Fetch reservations for today
            var reservations = Access.Reservations.Read()
                .Where(r => r.Date.HasValue && r.Date.Value.Date == DateTime.Now.Date)
                .Select(r => r.PlaceID)
                .ToHashSet(); // Store reserved table IDs for today in a HashSet

            // Fetch deactivated tables
            var deactivatedTables = Access.Places.Read()
                .Where(p => p.Active == 0)
                .Select(p => p.ID.Value)
                .ToHashSet();

            for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
            {
                for (int x = 0; x < GridPresent.GetGrid().GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1;

                        // Coloring Logic
                        if (deactivatedTables.Contains(tableNumber))
                        {
                            tableColors[tableNumber] = ConsoleColor.Red; // Deactivated tables
                        }
                        else if (reservations.Contains(tableNumber))
                        {
                            tableColors[tableNumber] = ConsoleColor.Red; // Reserved tables
                        }
                        else if (Array.Exists(activeTables, table => table == tableNumber) &&
                                !reservations.Contains(tableNumber)) // Ensure table is active and not reserved
                        {
                            tableColors[tableNumber] = ConsoleColor.Green; // Available for the day
                        }
                        else
                        {
                            tableColors[tableNumber] = ConsoleColor.Red; // Default to red for unfit or unavailable tables
                        }

                        // Draw table with assigned color
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

            // Automatically find table 1 and place the "X" on it
            (cursorX, cursorY) = FindTableCoordinates(1); // Dynamically set the cursor to table 1's coordinates
            HighlightNumber(activeTables, inactiveTables); // Highlight the selected table
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




        private void HighlightNumber(int[] activeTables, int[] reservedTables)
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

                // Check database for table status
                var table = Access.Places.Read().FirstOrDefault(p => p.ID == currentTable);

                if (table == null || table.Active == 0 || Array.Exists(reservedTables, table => table == currentTable))
                {
                    // Mark deactivated or reserved tables as red
                    ConsoleColor tableColor = ConsoleColor.Red;
                    _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor, activeTables, reservedTables);
                    return;
                }

                // Otherwise, highlight active and available tables
                ConsoleColor color = Array.Exists(activeTables, table => table == currentTable) ? ConsoleColor.Green : ConsoleColor.Red;
                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, color, activeTables, reservedTables);
            }

            Console.ResetColor();
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
                    Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 2);
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
                                Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid table. Please select a valid table.");
                                Console.ResetColor();
                                continue; // Retry selection
                            }

                            int tableNumber = int.Parse(selectedNumber);

                            // Check if the table is deactivated
                            var table = Access.Places.Read().FirstOrDefault(p => p.ID == tableNumber);
                            if (table != null && table.Active == 0)
                            {
                                Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 3);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Table {tableNumber} is unavailable. It is deactivated.");
                                Console.ResetColor();
                                continue; // Retry selection
                            }

                            if (!Array.Exists(availableTables, t => t == tableNumber) ||
                                Array.Exists(reservedTables, t => t == tableNumber))
                            {
                                // Display the error message for unavailable or reserved tables
                                Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 3);
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
                    if (nextX < 0 || nextX >= GridPresent.GetGrid().GetLength(1) || nextY < 0 || nextY >= GridPresent.GetGrid().GetLength(0) ||
                        string.IsNullOrEmpty(GetNumberAt(nextX, nextY)))
                    {
                        nextX = lastX;
                        nextY = lastY;
                    }
                    else
                    {
                        // Clear error message when navigating
                        Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 3);
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