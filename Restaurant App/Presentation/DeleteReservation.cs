using Project;
using Project.Presentation;

public static class DeleteReservation
{
    // Method to show the delete reservation interface and process the deletion
    public static void Show(ReservationModel reservation)
    {
        // Fetch the user details for the reservation
        var account = Access.Users.GetBy<int?>("ID", reservation.UserID);

        // Display confirmation message
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete the reservation for {GetUserFullName(account?.ID)}?");
        
        // Ask for confirmation (Yes/No)
        bool confirmed = ShowConfirmationMenu("Delete Reservation");

        if (confirmed)
        {
            // Attempt to delete the specific reservation
            Access.Reservations.Delete(reservation.ID);
            Console.WriteLine("Reservation deleted successfully.");
        }
        else
        {
            Console.WriteLine("Reservation deletion cancelled.");
        }

        // Wait for user input before going back to the reservation list
        Console.WriteLine("Press any key to return to the reservation list.");
        Console.ReadKey();
    }

    // Helper method to get user full name based on UserID
    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy<int?>("ID", userID);
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }

    // Modular method to display a Yes/No confirmation menu and return the user's choice
    private static bool ShowConfirmationMenu(string action)
    {
        // Convert the string array to List<string>
        var options = new List<string> { "Yes", "No" };

        // Presenting the Yes/No options
        var selection = SelectionPresent.Show(
            options,
            $"Are you sure you want to {action}? (Use arrow keys to select Yes or No and press Enter)\n"
        );

        return selection.text.Equals("Yes", StringComparison.OrdinalIgnoreCase);
    }
}
