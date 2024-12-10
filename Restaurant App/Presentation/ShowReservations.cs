using System.IO.Compression;
using Project;

public static class ShowReservations
{
    public static void Show()
    {
        TryCatchHelper.EscapeKeyException(() =>
        {
            while (true)
            {
                // TODO: Add a Calendar to select dates instead of typing it out
                Console.Clear();
                Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");

                // Use InputHelper to handle Escape and validate the date
                DateTime? parsedDate = InputHelper.GetValidatedInput<DateTime?>(
                "Date (dd/MM/yyyy): ",
                input =>
                {
                    if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var date))
                    {
                        return (date, null);  // valid date
                    }
                    return (null, "Invalid date format. Please try again.");
                });

                if (parsedDate == null)
                {
                    Console.WriteLine("Returning to the previous menu...");
                    return; // Escape key pressed
                }

                // fetch reservations for the given date
                var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate.Value)
                .Where(r => r != null)  // remove null elements
                .Cast<ReservationModel>();  // make sure the type is non-nullable
                
                if (!reservations.Any())
                {
                     Console.WriteLine("No reservations found for this date. Press any key to try another date.");
                    Console.ReadKey();
                    continue;
                }

                // display reservations
                ShowReservationOptions(parsedDate.Value, reservations);
            }

        });
    }

    private static void ShowReservationOptions(DateTime date, IEnumerable<ReservationModel> reservations)
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

            var options = new List<string> { "View Details", "Update Reservation", "Delete Reservation", "Cancel" };

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
