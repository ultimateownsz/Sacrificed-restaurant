
namespace Project;
public static class ReservationDetails
{
    public static void ShowDetails(ReservationModel reservation)
    {
        Console.Clear();

        // Fetch the account details for the user
        var account = Access.Reservations.GetBy<int?>("UserID", reservation.UserID);

        // Ensure data exists
        if (account == null)
        {
            Console.WriteLine("Unable to retrieve user details.");
            Console.WriteLine("Press any key to return to the reservations list.");
            Console.ReadKey();
            return;
        }

        // Display the details
        Console.WriteLine("Reservation Details");
        Console.WriteLine("-------------------");
        // Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
        Console.WriteLine();
        Console.WriteLine($"Reservation Date: {reservation.Date.ToString()}");
        Console.WriteLine($"Assigned Table number: {reservation.PlaceID}");
        Console.WriteLine("More details may appear in the future...");

        Console.WriteLine("\nPress any key to return to the reservations list.");
        Console.ReadKey();
    }

    public static void ShowOrders(UserModel acc)
    {
        int guests = 1;
        bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
        DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);

        while (true)
        {
            // Fetch all reservations for the selected date
            var reservations = Access.Reservations
                .GetAllBy<DateTime>("Date", selectedDate)
                .Where(r => r.Date == selectedDate)
                .ToList();

            if (reservations.Count == 0)
            {
                Console.Clear();
                Console.WriteLine($"There are no orders for the date {selectedDate:dd/MM/yyyy}.");
                Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
                ConsoleKeyInfo emptyOrders = Console.ReadKey();
                switch (emptyOrders.Key)
                {
                    case ConsoleKey.B:
                        return;
                    case ConsoleKey.P:
                        selectedDate = selectedDate.AddDays(-1);
                        continue;
                    case ConsoleKey.N:
                        selectedDate = selectedDate.AddDays(1);
                        continue;
                }
            }
            else
            {
                DisplayOrdersGrid(reservations, selectedDate);

                Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.B:
                        return;
                    case ConsoleKey.P:
                        selectedDate = selectedDate.AddDays(-1);
                        break;
                    case ConsoleKey.N:
                        selectedDate = selectedDate.AddDays(1);
                        break;
                }
            }
        }
    }

    private static void DisplayOrdersGrid(List<ReservationModel?> reservations, DateTime selectedDate)
    {
        // Dictionary to count products
        Dictionary<string, int> productCount = new Dictionary<string, int>();
        Dictionary<string, decimal> productPrices = new Dictionary<string, decimal>();

        foreach (var reservation in reservations)
        {
            // Fetch all requests linked to the reservation
            var requests = Access.Requests.GetAllBy<int?>("ReservationID", reservation.ID).ToList();

            foreach (var request in requests)
            {
                // Fetch the product associated with the request
                var product = Access.Products.GetBy<int?>("ID", request.ProductID);
                if (product != null)
                {
                    // Count the product occurrences
                    if (productCount.ContainsKey(product.Name))
                    {
                        productCount[product.Name]++;
                    }
                    else
                    {
                        productCount[product.Name] = 1;
                        productPrices[product.Name] = product.Price ?? 0; // Store product price
                    }
                }
            }
        }

        // Display the orders in a grid format
        decimal grandTotalPrice = 0;

        Console.Clear();
        Console.WriteLine($"Orders for {selectedDate:dd/MM/yyyy}\n");

        Console.WriteLine("{0,-30}{1,-20}{2,-20}", "Product", "Quantity", "Total Price");

        foreach (var product in productCount)
        {
            decimal totalPrice = product.Value * productPrices[product.Key];
            grandTotalPrice += totalPrice;

            Console.WriteLine("{0,-30}{1,-20}{2,-20:C}", product.Key, product.Value, totalPrice);
        }

        Console.WriteLine("\nGrand Total: {0:C}", grandTotalPrice);
    }


}
