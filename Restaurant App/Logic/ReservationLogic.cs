public class ReservationLogic
{

    //Static properties are shared across all instances of the class
    public static ReservationModel CurrentReservation { get; private set; } = new(0, 0, 0, 0, 0);
    public static Int64 Userid { get; private set; }

    public ReservationLogic()
    {
        // Could do something here

    }
    
    //This function is called throught the presentation layer (MakingReservation.cs)
    //this fucntion will call all the other neccecary functions to make a new ReservationAccess instance
    //with all the info from the user
    public Int64 SaveReservation(string date, string reservationAmount, Int64 userId)
    {   
        if (CurrentReservation != null)
        {
            CurrentReservation.Date = ConvertDate(date);
            CurrentReservation.TableChoice = TableChoice(reservationAmount);
            CurrentReservation.ReservationAmount = ReservationAmount(reservationAmount);
            CurrentReservation.ID = GenerateNewReservationID();
            CurrentReservation.UserID = userId;
            ReservationAccess.Write(CurrentReservation);
            return CurrentReservation.ID;
        }
        return 0;
    }

    //Converts the date from string to Int64 and saves it into CurrentReservation
    public Int64 ConvertDate(string date)
    {
        date = date.Replace("/", "");
        date = date.Replace("-", "");
        date = date.Replace(" ", "");
        Int64 convertedDate = Convert.ToInt64(date);
        return convertedDate;
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

    //Generates a new ID for the reservation and saves it into CurrentReservation
    //The Generated ID is the latest ID + 1
    public Int64 GenerateNewReservationID()
    {
        Int64 GeneratedID = ReservationAccess.GetLatestReservationID() + 1;
        return GeneratedID;
    }

    public Int64 ReservationAmount(string reservationAmount)
    {
        Int64 convertedAmount = Convert.ToInt64(reservationAmount);
        return convertedAmount;
    }

    //This is used to get a specific reservation from the database based on the given ID
    public ReservationModel GetById(int id)
    {
        return ReservationAccess.GetByReservationID(id);
    }

    public bool RemoveReservation(int id)
    {
        if(ReservationAccess.GetByReservationID(id) == null)
        {
            return false;
        }
        else
        {
            ReservationAccess.Delete(id);
            return true;
        }
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
    public static string GetThemeByReservation(int reservationID)
    {
        var menuItems = ReservationAdminLogic.GetMenuItemsForReservation(reservationID);

        if (menuItems != null && menuItems.Count > 0)
        {
            int menuID = (int)menuItems.First().MenuID; // Explicit cast from long to int
            return ReservationAdminLogic.GetThemeByMenuID(menuID);
        }

        return string.Empty;
    }
}
