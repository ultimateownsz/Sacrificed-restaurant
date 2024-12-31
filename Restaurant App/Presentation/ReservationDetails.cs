namespace Project;
public static class ReservationDetails
{
    public static void ShowDetails(ReservationModel reservation)
    {
        Console.Clear();
        ControlHelpPresent.Clear();
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        // Fetch the account details for the user
        var account = Access.Reservations.GetBy<int?>("UserID", reservation.UserID);

        // Display feedback if user details cannot be retrieved
        if (account == null)
        {
            ControlHelpPresent.DisplayFeedback("Unable to retrieve user details. Press any key to return...");
            Console.ReadKey();
            return;
        }

        // Display the reservation details
        Console.SetCursorPosition(0, 0); // Ensure content is displayed at the top
        Console.WriteLine("=========== Reservation Details =============================");
        Console.WriteLine($"Reservation Date     : {reservation.Date:yyyy-MM-dd}");
        Console.WriteLine($"Assigned Table Number:  {reservation.PlaceID}");
        Console.WriteLine($"User                 : {GetUserFullName(reservation.UserID)}");
        Console.WriteLine("\nMore details may appear in the future...");
        Console.WriteLine("==============================================================");

        // Show navigation options in the footer
        Console.WriteLine("\nPress any key to return to the reservations list...");
        Console.ReadKey();
    }

    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy<int?>("ID", userID);
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }

        ControlHelpPresent.DisplayFeedback($"User with ID {userID} not found.");
        return "Unknown User";
    }
}
