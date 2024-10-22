using System.Collections.Generic;

public static class ReservationAdminLogic
{
    public static List<ReservationModel> GetAllReservations()
    {
        return ReservationAccess.GetAllReservations();
    }

    public static ReservationModel GetReservationByID(int reservationID)
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

    public static List<ReservationModel> GetFilteredReservations(int? reservationID = null, int? date = null, int? userID = null)
    {
        return ReservationAccess.GetFilteredReservations(reservationID, date, userID);
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
