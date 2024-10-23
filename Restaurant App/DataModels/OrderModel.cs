public class OrderModel
{
    public Int64 OrderID { get; set; }
    public Int64 ReservationID { get; set; }
    public Int64 ProductID { get; set; }


    public OrderModel(Int64 orderID, Int64 reservationID, Int64 productID)
    {
        OrderID = orderID;
        ReservationID = reservationID;
        ProductID = productID;
    }
}
