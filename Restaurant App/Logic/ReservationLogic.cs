using Project;

public class ReservationLogic
{

    //Static properties are shared across all instances of the class
    public static ReservationModel CurrentReservation { get; private set; } = new();
    public static Int64 Userid { get; private set; }


    //This function is called throught the presentation layer (MakingReservation.cs)
    //this function will call all the other neccecary functions to make a new ReservationAccess instance
            //with all the info from the user
    public int SaveReservation(DateTime date, int userId, int placeId)
    {

        // Validate UserID (check if the user exists)
        var user = Access.Users.GetBy<int>("ID", userId);
        if (user == null)
        {
            Console.WriteLine($"ERROR: User ID {userId} does not exist.");
            return 0; // Return 0 if the user does not exist
        }

        // Validate PlaceID (check if the place exists)
        var place = Access.Places.GetBy<int>("ID", placeId); // Assuming Access.Places is valid
        if (place == null)
        {
            Console.WriteLine($"ERROR: Place ID {placeId} does not exist.");
            return 0; // Return 0 if the place does not exist
        }

        // Check if there is already an existing reservation on the same date and place
        var existingReservation = Access.Reservations.GetAllBy<DateTime>("Date", date)
            .FirstOrDefault(r => r?.PlaceID == placeId && r?.Date == date);

        if (existingReservation != null)
        {
            Console.WriteLine($"ERROR: There is already a reservation for PlaceID {placeId} on Date {date.ToShortDateString()}.");
            return 0; // Return 0 if there's a conflict
        }

        // Create the reservation
        var reservation = new ReservationModel
        {
            Date = date,
            PlaceID = placeId,
            UserID = userId
        };

        // Debug log the SQL query to make sure the data is correct

        // Insert reservation into database
        if (Access.Reservations.Write(reservation))
        {
            // Now get the last inserted reservation ID (assuming your database auto-increments the ID)
            var lastReservation = Access.Reservations.GetAllBy<int>("UserID", userId)
                                                    .OrderByDescending(r => r?.Date)
                                                    .FirstOrDefault(); // Assuming the latest reservation is by date or similar

            if (lastReservation != null)
            {
                Console.WriteLine($"DEBUG: Reservation saved with ID={lastReservation.ID}");
                return lastReservation.ID ?? 0; // Return the generated ID
            }
        }

        Console.WriteLine("ERROR: Reservation save failed");
        return 0; // Return 0 if the reservation failed
    }


    //Converts the date from string to Int64 and saves it into CurrentReservation
    public Int64 ConvertDate(DateTime date)
    {
        return DateTime.Parse(date.ToString()).ToFileTimeUtc();
    }

    //Converts the tableChoice from string to Int64 and saves it into CurrentReservation
    public Int64 TableChoice(string tableChoice)
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

    // THIS IS AN AUTOMATED PROCESS
    //Generates a new ID for the reservation and saves it into CurrentReservation
    //The Generated ID is the latest ID + 1,
    //public int? GenerateNewReservationID()
    //{
    //    int? GeneratedID = ReservationAccess.GetLatestReservationID() + 1;
    //    return GeneratedID;
    //}

    public Int64 ReservationAmount(string reservationAmount)
    {
        Int64 convertedAmount = Convert.ToInt64(reservationAmount);
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
        var reservation = Access.Reservations.GetBy<int>("ID", id);
        if (reservation == null)
        {
            return false;
        }

        var success = Access.Reservations.Delete(id);
        if (!success)
        {
            return false;
        }

        return true;
    }


    public IEnumerable<ReservationModel?> GetUserReservations(int? userID)
    {
        var reservations = Access.Reservations.GetAllBy<int?>("UserID", userID);
        if (reservations == null)
        {
            return Enumerable.Empty<ReservationModel?>();
        }
        return reservations;
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
        if (productID == null) return "No product associated with this reservation";

        var themeID = Access.Products.GetBy<int?>("ID", productID)?.ThemeID;
        if (themeID == null) return "No theme associated with this product";

        return Access.Themes.GetBy<int?>("ID", themeID)?.Name ?? "Theme not found";
    }


    public static string FormatAccount(ReservationModel reservation)
    {
        if (reservation == null) return "Reservation data is missing.";

        if (reservation.Date == null || reservation.PlaceID == null || reservation.ID == null)
        {
            return "Incomplete reservation details.";
        }
        
        return $"Date: {reservation.Date:dd/MM/yyyy} at table: {reservation.PlaceID}, reservation number: {reservation.ID}\n";
    }

    public static List<string> GenerateMenuOptions(List<ReservationModel> accounts, int currentPage, int totalPages)
    {
        if (accounts == null || !accounts.Any())
        {
            return new List<string> { "No reservations available", "Back" };
        }

        if (currentPage < 0 || currentPage >= totalPages)
        {
            throw new ArgumentOutOfRangeException("Invalid page number");
        }

        var options = accounts.Select(FormatAccount).ToList();
        if (currentPage > 0) options.Add("Previous Page");
        if (currentPage < totalPages - 1) options.Add("Next Page");
        options.Add("Back");
        return options;
    }

}
