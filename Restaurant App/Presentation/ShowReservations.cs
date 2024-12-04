using System.IO.Compression;
using Project;

public static class ShowReservations
{
    public static void Show()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");

            var dateInput = Console.ReadLine();
            if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
            {
                Console.WriteLine("Invalid date format. Press any key to try again.");
                Console.ReadKey();
                continue;
            }

            // Convert date to the required format (ddMMyyyy as integer)
            var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate);

            if (reservations.Count() == 0)
            {
                Console.WriteLine("No reservations found for this date. Press any key to try another date.");
                Console.ReadKey();
                continue;
            }

            // Fetch user names and table choices for reservations
            var reservationDetails = reservations.Select(r => new
            {
                Reservation = r,
                UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
                TableID = r.Place // Table choice of the reservation
            
            }).ToList();

            bool isValid = false;
            var reserv_options = reservationDetails.Select((reservation, index) => 
            $"{reservation.UserName} - Assigned to Table {reservation.TableID} (ID: {index})").ToList();

            while (!isValid)
            {
                Console.Clear();
                string reserv_date = $"Reservations for {parsedDate:dd/MM/yyyy}:\n\n";
                var selected = SelectionPresent.Show(reserv_options, reserv_date);
                string selectedText = (string)selected.text; // Casting dynamic to string
                Console.WriteLine("\nUse arrow keys to navigate. Press Enter to select, or Esc to go back to the Admin Menu.");
                switch (selectedText)
                {
                    case string selectedOption when reserv_options.Contains(selectedOption):
                        int selectedIndex = reserv_options.IndexOf(selectedOption);
                        ShowReservationOptions(reservationDetails[selectedIndex].Reservation);
                        break;
                }
            }
        }
    }

    private static void ShowReservationOptions(ReservationModel reservation)
    {
        bool isValid = false;

        while (!isValid)
        {
            Console.Clear(); // Refresh the options display
            string banner = $"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.Place}\nChoose an action:\n\n"; // Text the user will see

            switch (SelectionPresent.Show(["View Details", "Update Reservation", "Delete Reservation", "Cancel"], banner).text) // Showing all the options
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
                    isValid = true;
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
