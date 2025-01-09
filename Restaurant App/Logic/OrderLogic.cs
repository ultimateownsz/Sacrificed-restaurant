using Project;

public class OrderLogic
{

    //Static properties are shared across all instances of the class
    public static RequestModel CurrentOrder { get; private set; } = new(0, 0, 0);

    public OrderLogic()
    {
        // Could do something here

    }

    // public bool SaveOrder(int reservationId, int productId)
    // {
    //     // Validate reservation ID
    //     var reservation = Access.Reservations.GetBy<int>("ID", reservationId);
    //     if (reservation == null)
    //     {
    //         Console.WriteLine($"Reservation ID {reservationId} does not exist.");
    //         return false;
    //     }

    //     // Validate product ID
    //     if (!ProductManager.DoesProductExist(productId))
    //     {
    //         Console.WriteLine($"Product ID {productId} does not exist.");
    //         return false;
    //     }

    //     // Create a new RequestModel for the order
    //     var request = new RequestModel
    //     {
    //         ReservationID = reservationId,
    //         ProductID = productId
    //     };

    //     // Instantiate RequestAccess to save the request
    //     var requestAccess = new RequestAccess();

    //     // Save the order using RequestAccess
    //     if (!requestAccess.Write(request))
    //     {
    //         Console.WriteLine("Failed to save the order.");
    //         return false;
    //     }

    //     Console.WriteLine("Order saved successfully.");
    //     return true;
    // }

    public (bool isValid, string message) SaveOrder(int reservationId, int productId)
    {
        // Validate reservation ID
        var reservation = Access.Reservations.GetBy<int>("ID", reservationId);
        if (reservation == null)
        {
            return (false, $"Reservation ID {reservationId} does not exist.");
        }

        // Validate product ID
        if (!ProductManager.DoesProductExist(productId))
        {
            return (false, $"Product ID {productId} does not exist.");
        }

        // Create a new RequestModel for the order
        var request = new RequestModel
        {
            ReservationID = reservationId,
            ProductID = productId
        };

        // Instantiate RequestAccess to save the request
        var requestAccess = new RequestAccess();

        // Save the order using RequestAccess
        if (!requestAccess.Write(request))
        {
            return (false, "Failed to save the order.");
        }

        return (true, "Order saved successfully.");
    }

    public int? GenerateNewOrderID()
    {
        return null;
    }
}