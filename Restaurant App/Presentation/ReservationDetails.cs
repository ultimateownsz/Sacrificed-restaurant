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
            var orders = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

            if (!orders.Any(r => r.Date.HasValue && r.Date.Value == selectedDate))
            {
                Console.Clear();
                Console.WriteLine("There are no orders for this date.\nPress any key to return...");
                Console.ReadKey();
                return;
            }
        }
    }

    private static string FormatDate(long date)
    {
        // Format the date from ddMMyyyy (e.g., 12122024) to dd/MM/yyyy
        string dateString = date.ToString("D8"); // Ensure it's 8 digits long
        return $"{dateString.Substring(0, 2)}/{dateString.Substring(2, 2)}/{dateString.Substring(4)}";
    }
}
