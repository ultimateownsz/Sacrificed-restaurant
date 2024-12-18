using Project;

public class RequestLogic
{
    public static string FormatAccount(ReservationModel reservation)
    {
        return $"Reservation on {reservation.Date:yyyy-MM-dd} at Table {reservation.PlaceID} (ID: {reservation.ID})";
    }

    public static List<string> GenerateMenuOptions(List<ReservationModel> accounts, int currentPage, int totalPages)
    {
        var options = accounts.Select(FormatAccount).ToList();
        if (currentPage > 0) options.Add("Previous Page");
        if (currentPage < totalPages - 1) options.Add("Next Page");
        options.Add("Back");
        return options;
    }
}