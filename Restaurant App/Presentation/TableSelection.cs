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

        public void ShowGrid(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount, bool isAdminMode = false)
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

                        // Determine color based on the table's state
                        if (isAdminMode)
                        {
                            tableColors[tableNumber] = Array.Exists(activeTables, t => t == tableNumber)
                                ? ConsoleColor.Green
                                : ConsoleColor.Red;
                        }
                        else
                        {
                            if (Array.Exists(reservedTables, t => t == tableNumber))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Reserved
                            }
                            else if (Array.Exists(inactiveTables, t => t == tableNumber))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Inactive
                            }
                            else if (!IsTableValidForGuests(tableNumber, guestCount, activeTables))
                            {
                                tableColors[tableNumber] = ConsoleColor.Red; // Invalid size
                            }
                            else
                            {
                                tableColors[tableNumber] = ConsoleColor.Green; // Available
                            }
                        }

                        // Display the table
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

            // Automatically place "X" on the first available table
            (cursorX, cursorY) = FindFirstAvailableCoordinates(activeTables, inactiveTables, reservedTables, guestCount);
            HighlightNumber(activeTables, reservedTables, inactiveTables, guestCount, isAdminMode);
        }


        private (int, int) FindFirstAvailableCoordinates(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount)
        {
            for (int y = 0; y < GridPresent.GetGrid().GetLength(0); y++)
            {
                for (int x = 0; x < GridPresent.GetGrid().GetLength(1); x++)
                {
                    string number = GetNumberAt(x, y);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        if (!Array.Exists(reservedTables, t => t == tableNumber) &&
                            !Array.Exists(inactiveTables, t => t == tableNumber) &&
                            IsTableValidForGuests(tableNumber, guestCount, activeTables))
                        {
                            return (x, y);
                        }
                    }
                }
            }

            return FindTableCoordinates(1); // Fallback to table 1 if no available tables
        }


        private void HighlightNumber(int[] activeTables, int[] reservedTables, int[] inactiveTables, int guestCount, bool isAdmin)
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

                // Validate the table and get its properties
                var (isValid, _, message) = ValidateTable(
                    currentTable,
                    guestCount,
                    activeTables,
                    reservedTables,
                    inactiveTables,
                    isAdmin
                );

                if (!isAdmin && !string.IsNullOrEmpty(message))
                {
                    DisplayMessage(message); // Show the message above controls
                    Console.WriteLine("\n");
                }
                else
                {
                    ClearErrorMessage(); // Clear the message area for admins or valid tables
                }


                // Flash the "X" and the table number in yellow
                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, ConsoleColor.Yellow);
            }
        }


        private async Task FlashHighlightAsync(int tableNumber, int x, int y, ConsoleColor tableColor)
        {
            var token = flashCancellationTokenSource.Token;

            while (!token.IsCancellationRequested)
            {
                // Flash the "X" in yellow
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("X ");
                await Task.Delay(500);

                if (token.IsCancellationRequested) break;

                // Restore the table ID
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
                await Task.Delay(500);
            }

            if (!token.IsCancellationRequested)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2)); // Always restore the table ID
            }

            Console.ResetColor();
        }


        private void DisplayMessage(string message)
        {
            int messageY = GridPresent.GetGrid().GetLength(0) + 1; // Position above controls
            Console.SetCursorPosition(0, messageY);

            // Clear previous message completely
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, messageY);

            // Check the message content to determine the color
            if (message.Contains("can be reserved", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Green; // Green for available tables
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // Red for errors or other messages
            }

            // Display the message
            Console.WriteLine(message);
            Console.ResetColor();

            // Print a newline
            Console.WriteLine();
        }



        private void ShowErrorMessage(string message)
        {
            int messageY = GridPresent.GetGrid().GetLength(0) + 3;
            Console.SetCursorPosition(0, messageY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }


        private void ClearErrorMessage()
        {
            int messageY = GridPresent.GetGrid().GetLength(0) + 1; // Same line as the message
            Console.SetCursorPosition(0, messageY);

            // Clear the message line
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(cursorX, cursorY); // Return cursor to its original position
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
            // Restore the previous table's original color and ID
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
                        Console.Write(prevNumber.PadRight(2)); // Restore the table number
                    }
                }
            }

            // Highlight the current table with "X" and set to yellow
            string currNumber = GetNumberAt(currX, currY);
            if (!string.IsNullOrEmpty(currNumber))
            {
                int currTable = int.Parse(currNumber);

                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = ConsoleColor.Yellow; // Highlight as yellow
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
            const int requiredWidth = 140;
            const int requiredHeight = 45;
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
                flashCancellationTokenSource.Cancel(); // Cancel the token
                flashCancellationTokenSource.Dispose();
                flashCancellationTokenSource = new CancellationTokenSource(); // Reset for future use
            }

            // Clear any lingering "X" or number at the cursor's location
            ClearCursorArea();
        }

        private void ClearCursorArea()
        {
            if (!string.IsNullOrEmpty(GetNumberAt(cursorX, cursorY)))
            {
                Console.SetCursorPosition(cursorX, cursorY);
                string tableNumber = GetNumberAt(cursorX, cursorY);
                if (!string.IsNullOrEmpty(tableNumber))
                {
                    if (tableColors.TryGetValue(int.Parse(tableNumber), out ConsoleColor originalColor))
                    {
                        Console.ForegroundColor = originalColor;
                        Console.Write(tableNumber.PadRight(2)); // Restore the table ID
                    }
                }
                Console.ResetColor();
            }
        }



        private (bool isValid, ConsoleColor tableColor, string message) ValidateTable(
            int tableNumber,
            int guestCount,
            int[] activeTables,
            int[] reservedTables,
            int[] inactiveTables,
            bool isAdmin)
        {
            bool isReserved = Array.Exists(reservedTables, t => t == tableNumber);
            bool isInactive = Array.Exists(inactiveTables, t => t == tableNumber);
            bool isActive = Array.Exists(activeTables, t => t == tableNumber);

            // Determine if the table size is valid
            bool isTooSmall = guestCount > GetMaxGuestsForTable(tableNumber);
            bool isTooBig = guestCount < GetMinGuestsForTable(tableNumber);

            string message = "";
            ConsoleColor tableColor;

            if (isReserved && isActive)
            {
                message = $"Table {tableNumber} is already reserved.\n";
                tableColor = ConsoleColor.Red;
                return (false, tableColor, message);
            }

            if (isInactive)
            {
                if (isTooSmall)
                {
                    message = $"Table {tableNumber} is too small for {guestCount} guests and inactive.\n";
                }
                else if (isTooBig)
                {
                    message = $"Table {tableNumber} is too big for {guestCount} guests and inactive.\n";
                }
                else
                {
                    message = $"Table {tableNumber} is inactive.\n";
                }
                tableColor = ConsoleColor.Red;
                return (false, tableColor, message);
            }

            if (isTooSmall)
            {
                message = $"Table {tableNumber} is too small for {guestCount} guests.\n";
                tableColor = ConsoleColor.Red;
                return (false, tableColor, message);
            }

            if (isTooBig)
            {
                message = $"Table {tableNumber} is too big for {guestCount} guests.\n";
                tableColor = ConsoleColor.Red;
                return (false, tableColor, message);
            }

            // If the table passes all checks
            if (isActive && !isReserved)
            {
                tableColor = ConsoleColor.Green; // Available tables are colorless
                message = $"Table {tableNumber} can be reserved.\n"; // Add positive message
                return (true, tableColor, message);
            }

            // Default case: invalid table
            message = $"Table {tableNumber} is not available for selection.\n";
            tableColor = ConsoleColor.Red;
            return (false, tableColor, message);
        }


        private int GetMaxGuestsForTable(int tableNumber)
        {
            return tableNumber switch
            {
                1 or 4 or 5 or 8 or 9 or 11 or 12 or 15 => 2, // Tables for 1-2 guests
                6 or 7 or 10 or 13 or 14 => 4, // Tables for 3-4 guests
                2 or 3 => 6, // Tables for 5-6 guests
                _ => 0 // Invalid table number
            };
        }

        private int GetMinGuestsForTable(int tableNumber)
        {
            return tableNumber switch
            {
                1 or 4 or 5 or 8 or 9 or 11 or 12 or 15 => 1, // Tables for 1-2 guests
                6 or 7 or 10 or 13 or 14 => 3, // Tables for 3-4 guests
                2 or 3 => 5, // Tables for 5-6 guests
                _ => int.MaxValue // Invalid table number
            };
        }


        public int SelectTable(int[] activeTables, int[] inactiveTables, int[] reservedTables, int guestCount = 0, bool isAdmin = false)
        {
            EnsureConsoleSize();
            ShowGrid(activeTables, inactiveTables, reservedTables, guestCount, isAdmin);
            Console.CursorVisible = false;

            int lastX = cursorX, lastY = cursorY;

            try
            {
                while (true)
                {
                    Console.SetCursorPosition(0, GridPresent.GetGrid().GetLength(0) + 2);
                    Console.ResetColor();
                    // Console.WriteLine("Controls:\nNavigate : <arrows>\nSelect : <enter>\nExit : <escape>");
                    // Console.WriteLine("\nControls:\nNavigate : <arrows>\nSelect   : <enter>\nExit     : <escape>");
                    MenuHelperPresent.Show();

                    // \n is not possible with this, perhaps we should use the spectre console to make these functionalities more modulair since I could not integrated any modularity
                    // Console.WriteLine("Controls: Navigate: <arrows>, select: <enter>, exit: <escape>".PadRight(Console.WindowWidth - 1));

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)  // removed the b to make the menus more consistent
                    {
                        StopFlashing(); // Cancel all flashing tasks
                        ResetConsoleToDefault(); // Clear and reset the screen
                        return -1;
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

                            var (isValid, _, message) = ValidateTable(
                                tableNumber,
                                guestCount,
                                activeTables,
                                reservedTables,
                                inactiveTables,
                                isAdmin
                            );

                            if (!isValid && !isAdmin)
                            {
                                DisplayMessage(message); // Always display above controls
                                Console.WriteLine("\n");
                                continue;
                            }

                            StopFlashing();
                            ResetConsoleToDefault();
                            return tableNumber;

                    }

                    ClearErrorMessage();
                    UpdateTableHighlight(lastX, lastY, nextX, nextY, activeTables, reservedTables);
                    lastX = nextX;
                    lastY = nextY;
                    cursorX = nextX;
                    cursorY = nextY;

                    HighlightNumber(activeTables, reservedTables, inactiveTables, guestCount, isAdmin);
                }
            }
            finally
            {
                StopFlashing(); // Stop flashing on exit
                ResetConsoleToDefault();
                Console.CursorVisible = true;
            }
        }
    }
}