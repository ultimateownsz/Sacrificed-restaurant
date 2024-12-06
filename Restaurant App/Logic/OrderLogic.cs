using Project;

public class OrderLogic
{

    //Static properties are shared across all instances of the class
    public static RequestModel CurrentOrder { get; private set; } = new(0, 0, 0);

    public OrderLogic()
    {
        // Could do something here

    }

    public bool SaveOrder(int reservationId, int productId)
    {
        // Validate reservation ID
        if (Access.Reservations.GetBy<int>("ID", reservationId) == null)
        {
            Console.WriteLine($"Reservation ID {reservationId} does not exist.");
            return false;
        }

        // Validate product ID
        if (!ProductManager.DoesProductExist(productId))
        {
            Console.WriteLine($"Product ID {productId} does not exist.");
            return false;
        }

        // Save the order
        return Access.Orders.Write(new OrderModel
        {
            ReservationID = reservationId,
            ProductID = productId
        });
    }


    public int? GenerateNewOrderID()
    {
        return null;
    }
}