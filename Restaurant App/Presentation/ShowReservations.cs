using Presentation;
using Project;

public static class ShowReservations
{
    public static void Show(UserModel acc)
    {
        FuturePastResrvations.Show(acc, true); // using the new method
    }

    public static void ShowReservationOptions(ReservationModel reservation)
    {
        var reservationDetails = reservations.Select(r => new
        {
            Reservation = r,
            UserName = GetUserFullName(r.UserID),
            TableID = r.PlaceID
        }).ToList();

        var options = reservationDetails.Select((r, i) => $"{r.UserName} - Table {r.TableID}").ToList();
        options.Add("Cancel");  // add a cancel option

        while (true)
        {
            Console.Clear(); // Refresh the options display
            string banner = $"Reservations for {date:dd/MM/yyyy}\nChoose a reservation:\n\n";

            var selectedOption = SelectionPresent.Show(options, banner);
            if (selectedOption == null)
            {
                Console.WriteLine("Returning to the previous menu...");
                return;
            }

            string selectedText = selectedOption.text;
            if (selectedText == "Cancel") return;

            int selectedIndex = options.IndexOf(selectedText);
            if (selectedIndex >= 0 && selectedIndex < reservationDetails.Count)
            {
                ShowReservationActions(reservationDetails[selectedIndex].Reservation);
            }
        }
    }

    private static void ShowReservationActions(ReservationModel reservation)
    {
        bool isValid = false;

        while (!isValid)
        {
            Console.Clear();
            string banner = $"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.PlaceID}\nChoose an action:\n\n";

                        case 1: // Update Reservation
                            UpdateReservation.Show(reservation, true); // Boolean to check for admin
                            Console.ReadKey();
                            break;

            var selectedOption = SelectionPresent.Show(options, banner);

            if (selectedOption == null) // Escape key pressed
            {
                Console.WriteLine("Returning to the reservations list...");
                return;
            }

            switch (selectedOption.text)
            {
                case "View Details":
                    ReservationDetails.ShowDetails(reservation);
                    break;
                case "Update Reservation":
                    UpdateReservation.Show(reservation);
                    break;
                case "Delete Reservation":
                    DeleteReservation.Show(reservation);
                    break;
                case "Cancel":
                    isValid = true; // Exit to the reservations list
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy<int?>("ID", userID); // Fetch the account details
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }
}
