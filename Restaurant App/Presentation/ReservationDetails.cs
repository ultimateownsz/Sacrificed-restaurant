namespace Project;
public static class ReservationDetails
{
    public static void ShowDetails(ReservationModel reservation)
    {
        Console.Clear();
        ControlHelpPresent.Clear();
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();
        _ = ControlHelpPresent.GetFooterHeight(); // Dynamically determine footer height
        int reservationStartLine = 0; // Start drawing the receipt from the top

        Console.SetCursorPosition(0, reservationStartLine);

        // Fetch the account details for the user
        var account = Access.Reservations.GetBy<int?>("UserID", reservation.UserID);

        // Display feedback if user details cannot be retrieved
        if (account == null)
        {
            ControlHelpPresent.DisplayFeedback("Unable to retrieve user details. Press any key to return...", "bottom", "tip");
            Console.ReadKey();
            return;
        }

        // Display the reservation details
        Console.WriteLine("=========== Reservation Details =============================");
        Console.WriteLine($"Reservation date     : {reservation.Date:dd/MM/yyyy}");
        Console.WriteLine($"Assigned table number:  {reservation.PlaceID}");
        Console.WriteLine($"User                 : {GetUserFullName(reservation.UserID)}");
        Console.WriteLine("\nMore details may appear in the future...");
        Console.WriteLine("==============================================================");

        // Show navigation options in the footer
        ControlHelpPresent.DisplayFeedback("Press any key to return to the reservations list...", "bottom", "tip");
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
