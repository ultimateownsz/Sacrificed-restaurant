using Project;

public static class ShowReservations
{
    public static void Show()
    {
        DateTime parsedDate = DateTime.MinValue;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations, or press Escape (Esc) to go back:");

            string dateInput = string.Empty;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true); 

                if (key.Key == ConsoleKey.Escape)
                {
                    return; 
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    if (DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid date format. Press any key to try again or press Escape to go back.");
                        dateInput = string.Empty;
                        continue;
                    }
                }

                if (key.Key == ConsoleKey.Backspace && dateInput.Length > 0)
                {
                    dateInput = dateInput.Substring(0, dateInput.Length - 1);
                    Console.Write("\b \b");
                }
                else if (key.Key != ConsoleKey.Backspace)
                {
                    dateInput += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }

            var reservations = ReservationAdminLogic.GetReservationsByDate(parsedDate);

            if (!reservations.Any())
            {
                Console.WriteLine("\nNo reservations found for this date. Press any key to try another date.");
                Console.ReadKey();
                continue;
            }

            var reservationDetails = reservations.Select(r => new
            {
                Reservation = r,
                UserName = GetUserFullName(r.UserID),
                TableID = r.Place,
                ReservationID = r.ID
            }).ToList();

            var options = reservationDetails.Select(r =>
                $"{r.UserName} - Table {r.TableID} (Reservation ID: {r.ReservationID})"
            ).ToList();

            options.Add("Back");

            var selection = SelectionPresent.Show(
                options,
                $"Reservations for {parsedDate:dd/MM/yyyy}:\nUse arrow keys to navigate and press Enter to select.\n"
            );

            if (selection.text == "Back")
            {
                break;
            }

            var selectedReservation = reservationDetails
                .FirstOrDefault(r => $"{r.UserName} - Table {r.TableID} (Reservation ID: {r.ReservationID})" == selection.text);

            if (selectedReservation != null)
            {
                ShowReservationOptions(selectedReservation.Reservation);
            }
        }
    }

    public static void ShowReservationOptions(ReservationModel reservation)
    {
        var actions = new List<string> { "View Details", "Update Reservation", "Delete Reservation", "Back" };

        var selection = SelectionPresent.Show(
            actions,
            $"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.Place}\nChoose an action:\n"
        );

        switch (selection.text)
        {
            case "View Details":
                ReservationDetails.ShowDetails(reservation);
                Console.ReadKey();
                break;

            case "Update Reservation":
                UpdateReservation.Show(reservation);
                Console.ReadKey();
                break;

            case "Delete Reservation":
                DeleteReservation.Show(reservation);
                Console.ReadKey();
                break;

            case "Back":
                return;
        }
    }

    private static string GetUserFullName(int? userID)
    {
        var account = Access.Users.GetBy<int?>("ID", userID); 
        return account != null ? $"{account.FirstName} {account.LastName}" : "Unknown User";
    }
}

