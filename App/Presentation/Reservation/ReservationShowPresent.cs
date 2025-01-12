using App.DataAccess.Utils;
using Restaurant;

namespace App.Presentation.Reservation;
public static class ReservationShowPresent
{
    public static void Show(UserModel acc)
    {
        FuturePastResrvations.Show(acc, true); // using the new method
    }

    public static void ShowReservationOptions(ReservationModel reservation, UserModel acc)
    {
        while (true)
        {
            // Use SelectionPresent to display options and capture user selection
            var selectedOption = SelectionPresent.Show(
                new List<string>
                {
                    "View    Details",
                    "Update  Reservation",
                    "Delete  Reservation"
                },
                banner: $"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.PlaceID}\n\nChoose an action:"
            ).ElementAt(0).text;

            if (selectedOption == null) return;

            // Handle the chosen action
            switch (selectedOption)
            {
                case "View    Details":
                    ReservationDetailsPresent.ShowDetails(reservation);
                    break;

                case "Update  Reservation":
                    ReservationUpdatePresent.Show(reservation, acc); // Boolean to check for admin
                    break;

                case "Delete  Reservation":
                    ReservationDeletePresent.Show(reservation);
                    return; // Return after deleting a reservation to exit this menu

                case "":
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
        var account = Access.Users.GetBy("ID", userID); // Fetch account details
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }
}
