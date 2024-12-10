using Project;

namespace Project.Logic
{
    public static class AdminTableControlLogic
    {
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
