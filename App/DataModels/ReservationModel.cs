namespace Restaurant;
public class ReservationModel : IModel
{
    public int? ID { get; set; }
    public DateTime? Date { get; set; }
    public int? UserID { get; set; }
    public int? PlaceID { get; set; }

    public ReservationModel() { }

    public ReservationModel(DateTime? date, int? userID, int? placeID, int? id = null)
    {
        ID = id;
        Date = date;
        PlaceID = placeID;
        UserID = userID;
    } 
}
