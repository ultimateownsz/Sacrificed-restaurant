using Presentation;
using Project;

public static class ShowReservations
{
    public static void Show(UserModel acc)
    {
        FuturePastReservations.Show(acc, true); // using the new method
    }

    public static void ShowReservationOptions(ReservationModel reservation)
    {
        while (true)
        {
            // Use SelectionPresent to display options and capture user selection
            var selectedOption = SelectionPresent.Show(
                new List<string> 
                { 
                    "View reservation (details)", 
                    "Update reservation", 
                    "Delete reservation\n", 
                    "Back" 
                },
                $"Selected reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.PlaceID}\n\n"
            ).text;

            // Handle the chosen action
            switch (selectedOption)
            {
                case "View reservation (details)":
                    ReservationDetails.ShowDetails(reservation);
                    break;

                case "Update reservation":
                    UpdateReservation.Show(reservation, true); // Boolean to check for admin
                    break;

                case "Delete reservation":
                    DeleteReservation.Show(reservation);
                    return; // Return after deleting a reservation to exit this menu
                
                case "Back":
                    return; // Exit the options and return to the reservation list
            }

            // Pause after executing the action
            // EMERGENCY MODIFICATION: 2
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }
    }

    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy<int?>("ID", userID); // Fetch account details
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }
}