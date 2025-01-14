using App.DataAccess.Utils;
using Restaurant;

namespace App.Presentation.Table;

public static class TableControlPresent
{
    public static void Show()
    {
        Console.Clear();
        Console.WriteLine("Navigate the grid with arrow keys. Press Enter to toggle table state.");
        Console.WriteLine("Press Esc to return to the admin menu.");

        var tableSelection = new TableSelectionLogic();
        Console.CursorVisible = false;

        // Get active and inactive tables
        var activeTables = Access.Places.Read()
            .Where(p => p.Active == 1)
            .Select(p => p.ID.Value)
            .ToArray();

        var inactiveTables = Access.Places.Read()
            .Where(p => p.Active == 0)
            .Select(p => p.ID.Value)
            .ToArray();

        try
        {
            while (true)
            {
                var emptyReservedTables = Array.Empty<int>();

                // Call SelectTable with isAdmin set to true
                int selectedTable = tableSelection.SelectTable(
                    activeTables,
                    inactiveTables,
                    emptyReservedTables,
                    guestCount: 0, // No guest count needed for admin
                    isAdmin: true
                );

                if (selectedTable == -1)
                {
                    // Exit on "Back" or Esc key
                    break;
                }

                // Toggle the table's active state
                bool isSuccess = TableControlLogic.ToggleTableActiveState(selectedTable);

                if (isSuccess)
                {
                    Console.Clear();
                    Console.WriteLine($"Table {selectedTable} state toggled successfully.");

                    if (Access.Places.Read().FirstOrDefault(p => p.ID == selectedTable)?.Active == 0)
                    {
                        // Handle reservations when a table is deactivated
                        TableControlLogic.HandleDeactivatedTable(selectedTable);
                    }

                    // Update active and inactive tables
                    activeTables = Access.Places.Read()
                        .Where(p => p.Active == 1)
                        .Select(p => p.ID.Value)
                        .ToArray();

                    inactiveTables = Access.Places.Read()
                        .Where(p => p.Active == 0)
                        .Select(p => p.ID.Value)
                        .ToArray();
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Failed to toggle table {selectedTable}'s state. Please try again.");
                    Console.ResetColor();
                }
            }
        }
        finally
        {
            Console.CursorVisible = true;
            Console.Clear();
        }
    }
}
