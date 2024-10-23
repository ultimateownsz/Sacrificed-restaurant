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
    public Int64 SaveReservation(string date, string tableChoice, string reservationAmount, Int64 userId)
    {   
        if (CurrentReservation != null)
        {
            CurrentReservation.Date = ConvertDate(date);
            CurrentReservation.TableChoice = ConvertTableChoice(tableChoice);
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
        Int64 convertedDate = Convert.ToInt64(date);
        return convertedDate;
    }

    //Converts the tableChoice from string to Int64 and saves it into CurrentReservation
    public Int64 ConvertTableChoice(string tableChoice)
    {
        switch (tableChoice.ToLower())
        {
            case "1" or "two" or "table for two":
                return 2;
            case "2" or "four" or "table for four":
                return 4;   
            case "3" or "six" or "table for six":
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
}
