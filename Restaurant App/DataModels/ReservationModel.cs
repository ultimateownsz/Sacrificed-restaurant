namespace Project;
public class ReservationModel: IModel
{
    public int? ID { get; set; }
    public DateTime? Date { get; set; }
    public int? UserID { get; set; }
    public int? Place { get; set; }

    public ReservationModel() { }

    public ReservationModel(DateTime? date, int? userID, int? place, int? id = null)
    {
        ID = id;
        Date = date;
        Place = place;
        UserID = userID;
    } 
}
