using System;
using System.Collections.Generic;
using System.Linq;
using Project;

namespace Project.Logic
{
    public class AdminTableControlLogic
    {
        public static void ToggleTableState(int tableId, bool activate, DateTime date)
        {
            // Fetch all reservations for the table on the given date
            var reservations = Access.Reservations
                .GetAllBy<int>("PlaceID", tableId)
                .Where(r => r.Date == date)
                .ToList();

            if (!activate)
            {
                foreach (var reservation in reservations)
                {
                    // Find an alternative table for the reservation
                    int guestCount = reservation.GuestCount ?? 1;
                    int? alternativeTable = FindAlternativeTable(guestCount, date, tableId);

                    if (alternativeTable.HasValue)
                    {
                        // Move the reservation to an alternative table
                        reservation.PlaceID = alternativeTable.Value;
                        Access.Reservations.Update(reservation.ID, reservation);
                        Console.WriteLine($"Reservation for table {tableId} moved to table {alternativeTable.Value}.");
                    }
                    else
                    {
                        // Cancel the reservation if no alternative is available
                        Access.Reservations.Delete(reservation.ID);
                        Console.WriteLine($"Reservation for table {tableId} canceled due to no available alternatives.");
                    }
                }
            }

            // Update the table's active state in the database
            Access.Places.Update<int, bool>("ID", tableId, "Active", activate);
            Console.WriteLine($"Table {tableId} is now {(activate ? "active" : "inactive")}.");
        }

        private static int? FindAlternativeTable(int guestCount, DateTime date, int excludedTableId)
        {
            // Get all tables that are active and not reserved
            var activeTables = Access.Places.GetAllBy<int>("Active", 1)
                .Where(t => t.ID != excludedTableId) // Exclude the current table
                .Select(t => t.ID)
                .ToArray();

            var reservedTables = Access.Reservations
                .GetAllBy<DateTime>("Date", date)
                .Select(r => r.PlaceID)
                .Where(rt => rt.HasValue)
                .Select(rt => rt.Value)
                .ToArray();

            var availableTables = activeTables.Except(reservedTables).ToList();

            // Check for a table of the same size first
            foreach (var table in availableTables)
            {
                if (Access.Places.GetById(table).Size == guestCount)
                {
                    return table;
                }
            }

            // As an exception, allow a larger table
            foreach (var table in availableTables)
            {
                if (Access.Places.GetById(table).Size > guestCount)
                {
                    return table;
                }
            }

            // No table is available
            return null;
        }
    }
}
