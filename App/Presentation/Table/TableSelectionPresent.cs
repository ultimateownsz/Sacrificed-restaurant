using App.DataAccess.Utils;
using Restaurant;
using App.Logic.Table;

namespace App.Presentation.Table;

public class TableSelectionPresent
{
    private CancellationTokenSource flashCancellationTokenSource = new CancellationTokenSource();
    private int cursorX, cursorY;
    private Dictionary<int, ConsoleColor> tableColors = new Dictionary<int, ConsoleColor>();

    public int SelectedTable { get; private set; }

    private readonly TableSelectionLogic _logic;
    public TableSelectionPresent(TableSelectionLogic logic)
    {
        _logic = logic;
    }
    public int SelectTable(
        int[] activeTables,
        int[] inactiveTables,
        int[] reservedTables,
        int guestCount,
        bool isAdmin,
        (int cursorX, int cursorY) currentCursor,
        out (int x, int y) nextCursor)
    {
        string[,] grid = GridPresent.GetGrid();
        return _logic.SelectTable(grid, activeTables, inactiveTables, reservedTables, guestCount, isAdmin, currentCursor, out nextCursor);
    }

        public void ClearGrid()
        {
            string[,] grid = GridPresent.GetGrid();
            int gridHeight = grid.GetLength(0);
            int gridWidth = grid.GetLength(1);

            for (int y = 0; y < gridHeight; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', gridWidth));
            }
            Console.SetCursorPosition(0, 0);
        }

        public (int x, int y) FindTableCoordinates(int tableNumber)
        {
            string[,] grid = GridPresent.GetGrid();
            return _logic.FindTableCoordinates(tableNumber, grid);
        }

        public void ShowGrid(
            int[] activeTables,
            int[] inactiveTables,
            int[] reservedTables,
            int guestCount,
            bool isAdminMode = false)
        {
            string[,] grid = GridPresent.GetGrid();
            var tableColors = _logic.DetermineTableColors(
                activeTables, inactiveTables, reservedTables, guestCount, grid, isAdminMode);

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    string number = _logic.GetNumberAt(x, y, grid);
                    if (!string.IsNullOrEmpty(number))
                    {
                        int tableNumber = int.Parse(number);
                        x += number.Length - 1;

                        Console.SetCursorPosition(x - (number.Length - 1), y);
                        Console.ForegroundColor = tableColors[tableNumber];
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
            Console.ResetColor();
        }

        public (int x, int y) FindFirstAvailableCoordinates(
            int[] activeTables,
            int[] inactiveTables,
            int[] reservedTables,
            int guestCount)
        {
            string[,] grid = GridPresent.GetGrid();
            return _logic.FindFirstAvailableCoordinates(
                activeTables, inactiveTables, reservedTables, guestCount, grid);
        }

        public void HighlightNumber(
            int[] activeTables,
            int[] reservedTables,
            int[] inactiveTables,
            int guestCount,
            bool isAdmin,
            int cursorX,
            int cursorY)
        {
            string currentNumber = _logic.GetNumberAt(cursorX, cursorY, GridPresent.GetGrid());

            if (!string.IsNullOrEmpty(currentNumber))
            {
                int currentTable = int.Parse(currentNumber);
                ConsoleColor color;
                string message = _logic.ValidateTableState(
                    currentTable,
                    guestCount,
                    activeTables,
                    reservedTables,
                    inactiveTables,
                    isAdmin,
                    out color);

                if (!isAdmin && !string.IsNullOrEmpty(message))
                {
                    DisplayMessage(message);
                }
                else
                {
                    ClearErrorMessage();
                }
            }
        }

        private void DisplayMessage(string message)
        {
            int messageY = GridPresent.GetGrid().GetLength(0) + 1;
            Console.SetCursorPosition(0, messageY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, messageY);

            if (message.Contains("can be reserved"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private void ClearErrorMessage()
        {
            int messageY = GridPresent.GetGrid().GetLength(0) + 1;
            Console.SetCursorPosition(0, messageY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, messageY);
        }
    }
