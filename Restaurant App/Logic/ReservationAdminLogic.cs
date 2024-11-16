public static class ReservationAdminLogic
{
    public static List<ReservationModel> GetAllReservations()
    {
        return ReservationAccess.GetAllReservations();
    }

    public static List<ReservationModel> GetReservationsByMonthYear(int month, int year)
    {
        return ReservationAccess.GetReservationsByMonthYear(month, year);
    }

    public static string? GetThemeByMenuID(int menuID)
    {
        return ReservationAccess.GetThemeByMenuID(menuID);
    }

    public static ReservationModel? GetReservationByID(int reservationID)
    {
        return ReservationAccess.GetByReservationID(reservationID);
    }

    public static List<ReservationModel> GetReservationsByDate(int date)
    {
        return ReservationAccess.GetByDate(date);
    }

    public static List<ReservationModel> GetReservationsByUserID(int userID)
    {
        return ReservationAccess.GetByUserID(userID);
    }

    public static List<ProductModel> GetMenuItemsForReservation(int reservationID)
    {
        return ReservationAccess.GetMenuItemsByReservationID(reservationID);
    }

    public static void UpdateReservation(ReservationModel reservation)
    {
        ReservationAccess.Update(reservation);
    }

    public static void DeleteReservation(int reservationID)
    {
        ReservationAccess.Delete(reservationID);
    }
}
