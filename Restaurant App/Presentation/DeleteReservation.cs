using Project;
using Project.Presentation;

public static class DeleteReservation
{
    // Method to show the delete reservation interface and process the deletion
    public static void Show(ReservationModel reservation)
    {
        // Confirm with the user if they are sure about deleting the reservation
        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete the reservation for {GetUserFullName(reservation.ID)}? (Y/N)");

        string confirm = Console.ReadLine().ToLower();

        if (confirm == "y")
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

        // Go back to ShowReservations after the action is completed
        ShowReservations.Show();
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
}
