using System;
using System.Collections.Generic;
using Project;

namespace Project
{
    public class TableAdmin
    {
        private Access.Reservations reservationsAccess;

        public TableAdmin(Access.Reservations access)
        {
            reservationsAccess = access;
        }

        public void ToggleTableState(int tableId, bool activate)
        {
            // Fetch all reservations for the table
            var reservations = reservationsAccess.GetAllBy<int>("PlaceID", tableId);

            if (!activate)
            {
                foreach (var reservation in reservations)
                {
                    // Handle table deactivation by moving or canceling reservations
                    Console.WriteLine($"Notify: Table {tableId} deactivated. Handling reservations...");
                    // Add logic to move/cancel reservations
                }
            }

            // Update table state in the database
            Access.Places.Update<int, bool>("ID", tableId, "Active", activate);
        }
    }
}