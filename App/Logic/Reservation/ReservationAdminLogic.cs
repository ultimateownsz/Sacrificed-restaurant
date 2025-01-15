using App.DataAccess.Utils;
using Restaurant;

namespace App.Logic.Reservation;

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

    public static ReservationModel? GetReservationByID(int reservationID)
    {
        return Access.Reservations.GetBy("ID", reservationID);
    }

    public static IEnumerable<ReservationModel?> GetReservationsByDate(int date)
    {
        return Access.Reservations.GetAllBy("Date", date);
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
        return Access.Reservations.GetAllBy("Place", tableID);
    }
}
