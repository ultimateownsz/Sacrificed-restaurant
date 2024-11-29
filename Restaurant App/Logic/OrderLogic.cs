using Project;

public class OrderLogic
{

    //Static properties are shared across all instances of the class
    public static RequestModel CurrentOrder { get; private set; } = new(0, 0, 0);

    public OrderLogic()
    {
        // Could do something here

    }

    public void SaveOrder(int? reservationID, int? productID)
    {   
        if (CurrentOrder != null)
        {
            CurrentOrder.ID = GenerateNewOrderID();
            CurrentOrder.ReservationID = reservationID;
            CurrentOrder.ProductID = productID;
            Access.Requests.Write(CurrentOrder);
        }
    }

    public int? GenerateNewOrderID()
    {
        return null;
    }
}