using Project;
using Project.Presentation;

public static class DeleteReservation
{
    // Method to show the delete reservation interface and process the deletion
    public static void Show(ReservationModel reservation)
    {
        bool isVaild = false;
        while (!isVaild)
        {
            // Confirm with the user if they are sure about deleting the reservation
            Console.Clear();
            var account = Access.Users.GetBy<int?>("ID", reservation.UserID);
            string confirm_info = $"Are you sure you want to delete the reservation for {GetUserFullName(account?.ID)}?\n\n";

            switch(SelectionPresent.Show(["Yes", "No"], confirm_info).text)
            {
                case "Yes":
                    // Attempt to delete the specific reservation
                    Access.Reservations.Delete(reservation.ID);
                    Console.WriteLine("Reservation deleted successfully.");
                    isVaild = true; // Exits the confirmation loop
                    break;

                case "No":
                    Console.WriteLine("Reservation deletion cancelled.");
                    isVaild = true; // Exits the confirmation loop
                    break;
            }
        // Wait for user input before going back to the reservation list
        Console.WriteLine("Press any key to return to the reservation list.");
        }
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
