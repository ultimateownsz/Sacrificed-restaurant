public class OrderLogic
{

    //Static properties are shared across all instances of the class
    public static OrderModel CurrentOrder { get; private set; } = new(0, 0, 0);

    public OrderLogic()
    {
        // Could do something here

    }

    public void SaveOrder(Int64 reservationID, Int64 productID)
    {   
        if (CurrentOrder != null)
        {
            CurrentOrder.OrderID = GenerateNewOrderID();
            CurrentOrder.ReservationID = reservationID;
            CurrentOrder.ProductID = productID;
            OrderAccess.Write(CurrentOrder);
        }
    }

    public Int64 GenerateNewOrderID()
    {
        Int64 GeneratedID = OrderAccess.GetLatestOrderID() + 1;
        return GeneratedID;
    }
}