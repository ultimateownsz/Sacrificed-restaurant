namespace Project;
public class RequestModel: IModel
{
    public int? ID { get; set; }
    public int? ReservationID { get; set; }
    public int? ProductID { get; set; }

    public RequestModel() { }

    public RequestModel(int? reservationID, int? productID, int? id = null)
    {
        ID = id;
        ProductID = productID;
        ReservationID = reservationID;
    } 
}
