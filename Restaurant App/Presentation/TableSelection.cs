using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation
{
    public class TableSelection
    {
        private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
        private int cursorX, cursorY;
        private Dictionary<int, ConsoleColor> tableColors = new Dictionary<int, ConsoleColor>();

        public int SelectedTable { get; private set; }

        private void ClearErrorMessage()
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 3);
            Console.Write(new string(' ', Console.WindowWidth)); // Clear the error line
        }

        public void ShowGrid(int[] availableTables, int[] reservedTables)
        {
            tableColors.Clear();
            GridPresent.Show(availableTables, reservedTables, tableColors);
            (cursorX, cursorY) = GridPresent.FindTableCoordinates(1); // Set cursor to table 1
            HighlightNumber(availableTables, reservedTables); // Highlight selected table
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
                tableColor = GetTableColor(tableNumber, availableTables, reservedTables);
                Console.ForegroundColor = tableColor;
                Console.Write(tableNumber.ToString().PadRight(2));
            }

            Console.ResetColor();
        }

        private void HighlightNumber(int[] availableTables, int[] reservedTables)
        {
            if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
            {
                flashCancellationTokenSource.Cancel();
                flashCancellationTokenSource.Dispose();
            }

            flashCancellationTokenSource = new CancellationTokenSource();

            string currentNumber = GridPresent.GetNumberAt(cursorX, cursorY);
            if (!string.IsNullOrEmpty(currentNumber))
            {
                int currentTable = int.Parse(currentNumber);
                ConsoleColor tableColor = GetTableColor(currentTable, availableTables, reservedTables);

                _ = FlashHighlightAsync(currentTable, cursorX, cursorY, tableColor, availableTables, reservedTables);
            }

            Console.ResetColor();
        }

        private ConsoleColor GetTableColor(int tableNumber, int[] availableTables, int[] reservedTables)
        {
            if (Array.Exists(reservedTables, table => table == tableNumber))
            {
                return ConsoleColor.Red;
            }
            else if (Array.Exists(availableTables, table => table == tableNumber))
            {
                return ConsoleColor.Green;
            }
            else
            {
                return ConsoleColor.DarkGray;
            }
        }

        private void ResetConsoleToDefault()
        {
            Console.ResetColor();
            Console.Clear();
        }

        private void EnsureConsoleSize()
        {
            const int requiredWidth = 80;
            const int requiredHeight = 30;

            MaximizeConsoleWindow();

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

        public static void MaximizeConsoleWindow()
        {
            const int SW_MAXIMIZE = 3;

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll", SetLastError = true)]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            IntPtr consoleWindow = GetConsoleWindow();
            if (consoleWindow != IntPtr.Zero)
            {
                ShowWindow(consoleWindow, SW_MAXIMIZE);
            }
        }

        private void StopFlashing()
        {
            if (flashCancellationTokenSource != null && !flashCancellationTokenSource.IsCancellationRequested)
            {
                flashCancellationTokenSource.Cancel();
                flashCancellationTokenSource.Dispose();
                flashCancellationTokenSource = null;
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
                    Console.SetCursorPosition(0, Console.WindowHeight - 2);
                    Console.ResetColor();
                    Console.WriteLine("(B)ack");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.B || key.Key == ConsoleKey.Escape)
                    {
                        StopFlashing(); // Ensure flashing stops
                        ResetConsoleToDefault(); // Reset colors and clean up screen
                        return -1; // Return -1 to indicate cancellation
                    }
                    (int nextX, int nextY) = GridPresent.HandleGridNavigation(cursorX, cursorY, key.Key, availableTables, reservedTables);

                    // Ensure valid navigation
                    if (nextX != cursorX || nextY != cursorY)
                    {
                        UpdateTableHighlight(cursorX, cursorY, nextX, nextY, availableTables, reservedTables);
                        cursorX = nextX;
                        cursorY = nextY;
                    }

                    else
                    {
                        ClearErrorMessage();

                        UpdateTableHighlight(lastX, lastY, nextX, nextY, availableTables, reservedTables);
                        lastX = nextX;
                        lastY = nextY;
                        cursorX = nextX;
                        cursorY = nextY;

                        HighlightNumber(availableTables, reservedTables);
                    }

                    if (key.Key == ConsoleKey.Enter)
                    {
                        string selectedNumber = GridPresent.GetNumberAt(cursorX, cursorY);

                        if (string.IsNullOrEmpty(selectedNumber))
                        {
                            Console.SetCursorPosition(0, Console.WindowHeight - 3);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid table. Please select a valid table.");
                            Console.ResetColor();
                            continue;
                        }

                        int tableNumber = int.Parse(selectedNumber);

                        if (!Array.Exists(availableTables, table => table == tableNumber) ||
                            Array.Exists(reservedTables, table => table == tableNumber))
                        {
                            Console.SetCursorPosition(0, Console.WindowHeight - 3);
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
                }
            }
            finally
            {
                StopFlashing();
                ResetConsoleToDefault();
                Console.CursorVisible = true;
            }
        }

        private void UpdateTableHighlight(int prevX, int prevY, int currX, int currY, int[] availableTables, int[] reservedTables)
        {
            if (prevX != -1 && prevY != -1)
            {
                string prevNumber = GridPresent.GetNumberAt(prevX, prevY);
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

            string currNumber = GridPresent.GetNumberAt(currX, currY);
            if (!string.IsNullOrEmpty(currNumber))
            {
                int currTable = int.Parse(currNumber);
                ConsoleColor currColor = GetTableColor(currTable, availableTables, reservedTables);

                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = currColor;
                Console.Write("X ");
            }

            Console.ResetColor();
        }
    }
}
