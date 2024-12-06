namespace Project;
public class OrderModel : IModel
{
    public int? ID { get; set; } // Updated to match IModel
    public int ReservationID { get; set; }
    public int ProductID { get; set; }
}
