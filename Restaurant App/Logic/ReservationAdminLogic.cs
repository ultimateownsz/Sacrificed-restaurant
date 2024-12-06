using Project;

public static class ReservationAdminLogic
{
    public static IEnumerable<ReservationModel> GetAllReservations()
    {
        return Access.Reservations.Read();
    }

    //public static List<ReservationModel> GetReservationsByMonthYear(int month, int year)
    //{
    //    return ReservationAccess.GetReservationsByMonthYear(month, year);
    //}

    //public static string GetThemeByMenuID(int menuID)
    //{
    //    return ReservationAccess.GetThemeByMenuID(menuID);
    //}

    public static ReservationModel? GetReservationByID(int reservationID)
    {
        return Access.Reservations.GetBy<int>("ID", reservationID);
    }

    public static IEnumerable<ReservationModel?> GetReservationsByDate(int date)
    {
        return Access.Reservations.GetAllBy<int>("Date", date);
    }

    public static IEnumerable<ReservationModel?> GetReservationsByUserID(int userID)
    {
        return Access.Reservations.GetAllBy("UserID", userID);
    }

    //public static List<ProductModel> GetMenuItemsForReservation(int reservationID)
    //{
    //    return ReservationAccess.GetMenuItemsByReservationID(reservationID);
    //} 

    public static IEnumerable<ReservationModel?> GetReservationsByTableID(int tableID)
    {
        return Access.Reservations.GetAllBy<int>("Place", tableID);
    }

    public static List<ReservationModel> GetReservationsByDate(DateTime date)
    {
        var potentialReservations = Access.Reservations.GetAll();
        if (potentialReservations == null || !potentialReservations.Any())
        {
            return new List<ReservationModel>();
        }

        return potentialReservations
            .Where(r => r.Date.HasValue && r.Date.Value.Date == date.Date)
            .ToList();
    }
}
