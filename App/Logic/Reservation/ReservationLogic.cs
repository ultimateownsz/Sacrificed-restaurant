using App.DataAccess.Utils;
using Restaurant;

namespace App.Logic.Reservation;

public class ReservationLogic
{

    //Static properties are shared across all instances of the class
    public static ReservationModel CurrentReservation { get; private set; } = new();
    public static long Userid { get; private set; }

    public int SaveReservation(DateTime date, int userId, int placeId)
    {
        // Validate UserID
        var user = Access.Users.GetBy("ID", userId);
        if (user == null)
        {
            Console.WriteLine($"ERROR: User ID {userId} does not exist.");
            return 0;
        }

        // Validate PlaceID
        var place = Access.Places.GetBy("ID", placeId);
        if (place == null)
        {
            Console.WriteLine($"ERROR: Place ID {placeId} does not exist.");
            return 0;
        }

        // Check for existing reservation conflict
        var existingReservation = Access.Reservations
            .GetAllBy("Date", date)
            .FirstOrDefault(r => r?.PlaceID == placeId);
        if (existingReservation != null)
        {
            Console.WriteLine($"ERROR: Conflict. Reservation already exists for PlaceID {placeId} on {date.ToShortDateString()}.");
            return 0;
        }

        // Create and save the reservation
        var reservation = new ReservationModel
        {
            Date = date,
            PlaceID = placeId,
            UserID = userId
        };
        if (!Access.Reservations.Write(reservation))
        {
            Console.WriteLine("ERROR: Failed to save the reservation.");
            return 0;
        }

        // Retrieve and log the newly created reservation ID
        var newReservation = Access.Reservations
            .Read()
            .OrderByDescending(r => r.ID)
            .FirstOrDefault();
        if (newReservation != null)
        {
            Console.WriteLine($"DEBUG: Reservation saved successfully with ID: {newReservation.ID}");
            return newReservation.ID.Value;
        }

        Console.WriteLine("ERROR: Failed to fetch the newly created reservation.");
        return 0;
    }


    //Converts the date from string to Int64 and saves it into CurrentReservation
    public long ConvertDate(DateTime date)
    {
        return DateTime.Parse(date.ToString()).ToFileTimeUtc();
    }

    //Converts the tableChoice from string to Int64 and saves it into CurrentReservation
    public long TableChoice(string tableChoice)
    {
        switch (tableChoice.ToLower())
        {
            case "1" or "2" or "one" or "two":
                return 2;
            case "3" or "4" or "three" or "four":
                return 4;
            case "5" or "6" or "five" or "six":
                return 6;
            default:
                return 0;
        };
    }

    public long ReservationAmount(string reservationAmount)
    {
        switch (reservationAmount.ToLower())
        {
            case "1" or "2" or "one" or "two":
                return 2;
            case "3" or "4" or "three" or "four":
                return 4;
            case "5" or "6" or "five" or "six":
                return 6;
            default:
                return 0;
        };
    }

    //This is used to get a specific reservation from the database based on the given ID
    // public ReservationModel GetById(int id)
    // {
    //     return Access.Reservations.GetBy<int>("ID", id);
    // }


    public bool RemoveReservation(int id)
    {

        if (Access.Reservations.GetBy("ID", id) == null)
        {
            return false;
        }
        else
        {
            Access.Reservations.Delete(id);
            return true;
        }
    }

    public IEnumerable<ReservationModel?> GetUserReservations(int? userID)
    {
        return Access.Reservations.GetAllBy("UserID", userID);
    }

    public static bool IsValidMonthYear(string monthInput, string yearInput, out int month, out int year)
    {
        month = 0;
        year = 0;

        return monthInput.Length == 2 && yearInput.Length == 4
            && int.TryParse(monthInput, out month) && int.TryParse(yearInput, out year)
            && month >= 1 && month <= 12
            && year >= 2024 && year <= DateTime.Now.Year;
    }
    public static string? GetThemeByReservation(int reservationID)
    {
        var productID = Access.Requests.GetBy<int?>("ReservationID", reservationID)?.ProductID;
        var themeID = Access.Products.GetBy("ID", productID)?.ThemeID;
        return Access.Themes.GetBy("ID", themeID)?.Name;
    }

    public static string FormatAccount(ReservationModel reservation)
    {
        return $"Reservation on {reservation.Date:yyyy-MM-dd} at Table {reservation.PlaceID} (ID: {reservation.ID})";
    }

    public static List<string> GenerateMenuOptions(List<ReservationModel> accounts, int currentPage, int totalPages)
    {
        var options = accounts.Select(FormatAccount).ToList();
        if (currentPage > 0) options.Add("Previous Page");
        if (currentPage < totalPages - 1) options.Add("Next Page");
        return options;
    }
}
