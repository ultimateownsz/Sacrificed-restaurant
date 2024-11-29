using Project;

public class ReservationLogic
{

    //Static properties are shared across all instances of the class
    public static ReservationModel CurrentReservation { get; private set; } = new();
    public static Int64 Userid { get; private set; }


    //This function is called throught the presentation layer (MakingReservation.cs)
    //this fucntion will call all the other neccecary functions to make a new ReservationAccess instance
    //with all the info from the user
    public int? SaveReservation(DateTime date, string reservationAmount, int? userId)
    {
        if (CurrentReservation != null)
        {
            CurrentReservation.Date = date;
            //CurrentReservation.Place = TableChoice(reservationAmount);
            //CurrentReservation.ReservationAmount = ReservationAmount(reservationAmount);
            CurrentReservation.ID = null;
            CurrentReservation.UserID = userId;
            Access.Reservations.Write(CurrentReservation);
            return CurrentReservation.ID;
        }
        return 0;
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
    public ReservationModel GetById(int id)
    {
        return Access.Reservations.GetBy<int>("ID", id);
    }

    public bool RemoveReservation(int id)
    {
        
        if (Access.Reservations.GetBy<int>("ID", id) == null)
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
        return Access.Reservations.GetAllBy<int?>("UserID", userID);
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
        var themeID = Access.Products.GetBy<int?>("ID", productID)?.ThemeID;
        return Access.Themes.GetBy<int?>("ID", themeID)?.Name;
    }
}
