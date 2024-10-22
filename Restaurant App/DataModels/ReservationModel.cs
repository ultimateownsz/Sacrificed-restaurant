public class ReservationModel
{
    public Int64 ID { get; set; }
    public Int64 Date { get; set; }
    public Int64 TableChoice { get; set; }
    public Int64 ReservationAmount { get; set; }
    public Int64 UserID { get; set; }

    public ReservationModel() { }
    
    public ReservationModel(Int64 id, Int64 date, Int64 tableChoice, Int64 reservationAmount, Int64 userId)
    {
        ID = id;
        Date = date;
        TableChoice = tableChoice;
        ReservationAmount = reservationAmount;
        UserID = userId;
    }

}
