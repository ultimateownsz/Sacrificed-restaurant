using App.DataAccess.Utils;
using Restaurant;

namespace App.Presentation.Reservation;

public static class ReservationDeletePresent
{
    // Method to show the delete reservation interface and process the deletion
    public static void Show(ReservationModel reservation)
    {
        while (true)
        {
            // Clear the console and display the confirmation menu
            Console.Clear();
            ControlHelpPresent.Clear();
            ControlHelpPresent.ResetToDefault();
            ControlHelpPresent.ShowHelp();
            var account = Access.Users.GetBy("ID", reservation.UserID);
            string userFullName = GetUserFullName(account?.ID);
            Console.WriteLine($"Are you sure?");

            // Use SelectionPresent to show Yes/No options
            var selectedOption = SelectionPresent.Show(
                new List<string> { "Yes", "No" },
                banner: "DELETION\n\nAre you sure?"
            ).ElementAt(0).text;

            if (selectedOption == "Yes")
            {
                // Attempt to delete the specific reservation
                Access.Reservations.Delete(reservation.ID);
                //return; // Exit after successful deletion
                Console.Clear();
                ControlHelpPresent.DisplayFeedback("Reservation deleted successfully.", "bottom", "success");
                ControlHelpPresent.DisplayFeedback("Press any key to return to the reservation list.", "bottom", "tip");
                Console.ReadKey();
            }
            //else if (selectedOption == "No")
            //{
            //    Console.WriteLine("Reservation deletion cancelled.");
            //    Console.WriteLine("Press any key to return to the reservation list.");
            //    Console.ReadKey();
            //    return; // Exit back to the reservation list
            //}
            return;
        }
    }

    // Helper method to get user full name based on UserID
    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy("ID", userID);
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }
}
