using Project;

namespace Project.Logic
{
    public static class AdminTableControlLogic
    {
        public static void HandleDeactivatedTable(int tableID)
        {
            // Fetch all reservations for the deactivated table
            var reservations = Access.Reservations.Read()
                .Where(r => r.PlaceID == tableID && r.Date.HasValue)
                .ToList();

            if (!reservations.Any())
            {
                Console.WriteLine($"No reservations found for table {tableID}.");
                Console.WriteLine("Press Enter to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                return;
            }

            Console.WriteLine($"Handling reservations for deactivated table {tableID}:");

            foreach (var reservation in reservations)
            {
                // Attempt to find another table with the same capacity
                var currentTable = Access.Places.Read().FirstOrDefault(p => p.ID == tableID);
                if (currentTable == null) continue;

                var availableTables = Access.Places.Read()
                    .Where(p => p.Active == 1 && p.ID != tableID) // Active and not the current table
                    .ToList();

                var replacementTable = availableTables
                    .Where(p => p.Capacity == currentTable.Capacity &&
                                !Access.Reservations.Read().Any(r => r.PlaceID == p.ID && r.Date == reservation.Date))
                    .FirstOrDefault();

                if (replacementTable == null)
                {

                    // If no table with the same capacity is available, try larger capacity
                    replacementTable = availableTables
                        .Where(p => p.Capacity > currentTable.Capacity &&
                                    !Access.Reservations.Read().Any(r => r.PlaceID == p.ID && r.Date == reservation.Date))
                        .OrderBy(p => p.Capacity) // Prefer the smallest larger table
                        .FirstOrDefault();
                }

                if (replacementTable != null)
                {
                    // Update the reservation to use the new table
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Reservation ID {reservation.ID} has been moved from table {tableID} to table {replacementTable.ID}.");
                    Console.ResetColor();
                    reservation.PlaceID = replacementTable.ID;
                    Access.Reservations.Update(reservation);
                }
                else
                {
                    // If no replacement table is available, cancel the reservation
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Reservation ID {reservation.ID} for table {tableID} has been canceled (no replacement available).");
                    Console.ResetColor();
                    Access.Reservations.Delete(reservation.ID);
                }

                // Wait for the admin to press Enter before proceeding
                Console.WriteLine("Press Enter to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        }
        public static bool ToggleTableActiveState(int tableID)
        {
            var table = Access.Places.Read().FirstOrDefault(p => p.ID == tableID);
            if (table == null)
                return false;

            table.Active = table.Active == 1 ? 0 : 1;
            return Access.Places.Update(table);
        }

        public static List<int> GetReservationsForTable(int tableID)
        {
            return Access.Reservations.Read()
                .Where(r => r.PlaceID == tableID)
                .Select(r => r.ID.Value)
                .ToList();
        }

        public static int GetAvailableTable(int capacity, int excludeTableID = -1)
        {
            return Access.Places.Read()
                .Where(p => p.Capacity >= capacity && p.Active == 1 && p.ID != excludeTableID)
                .OrderBy(p => p.Capacity)
                .Select(p => p.ID.Value)
                .FirstOrDefault();
        }
    }
}
