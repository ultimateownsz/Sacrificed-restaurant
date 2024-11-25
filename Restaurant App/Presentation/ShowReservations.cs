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
            int formattedDate = int.Parse(parsedDate.ToString("ddMMyyyy"));

            var reservations = ReservationAccess.GetReservationsByDate(formattedDate);

            if (reservations.Count == 0)
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
                TableID = r.TableID // Table choice of the reservation
            }).ToList();

            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Reservations for {parsedDate:dd/MM/yyyy}:");

                // Display reservations with highlight for the selected one
                for (int i = 0; i < reservationDetails.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Highlight selected
                        Console.WriteLine($"-> {reservationDetails[i].UserName} - Assigned to Table {reservationDetails[i].TableID}");
                    }
                    else
                    {
                        Console.ResetColor(); // Default color for others
                        Console.WriteLine($"  {reservationDetails[i].UserName} - Assigned to Table {reservationDetails[i].TableID}");
                    }
                }

                Console.ResetColor(); // Reset any lingering color changes
                Console.WriteLine("\nUse arrow keys to navigate. Press Enter to select, or Esc to go back to the Admin Menu.");

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedIndex < reservationDetails.Count - 1)
                            selectedIndex++;
                        break;

                    case ConsoleKey.Enter:
                        // Show options for the selected reservation
                        ShowReservationOptions(reservationDetails[selectedIndex].Reservation);
                        break;

                    case ConsoleKey.Escape:
                        AdminMenu.AdminStart(); // Return to the Admin Menu
                        return;
                }
            }
        }
    }

    private static void ShowReservationOptions(ReservationModel reservation)
    {
        Console.Clear();
        Console.WriteLine($"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.TableChoice}");
        Console.WriteLine("Choose an action:");

        // List of possible actions
        string[] actions = { "View Details", "Update Reservation", "Delete Reservation", "Cancel" };

        int currentActionIndex = 0;

        while (true)
        {
            // Display actions with arrow key navigation and color highlighting
            for (int i = 0; i < actions.Length; i++)
            {
                if (i == currentActionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow; // Highlight the selected option
                    Console.WriteLine($"-> {actions[i]}");
                }
                else
                {
                    Console.ResetColor(); // Reset color for non-selected options
                    Console.WriteLine($"  {actions[i]}");
                }
            }

            // Capture key input for navigation and action selection
            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (currentActionIndex > 0)
                    {
                        currentActionIndex--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (currentActionIndex < actions.Length - 1)
                    {
                        currentActionIndex++;
                    }
                    break;

                case ConsoleKey.Enter:
                    Console.ResetColor();
                    switch (currentActionIndex)
                    {
                        case 0: // View Details
                            ReservationDetails.ShowDetails(reservation);
                            Console.ReadKey();
                            break;

                        case 1: // Update Reservation
                            UpdateReservation.Show(reservation);
                            Console.ReadKey();
                            break;

                        case 2: // Delete Reservation
                            // Call DeleteReservation.Show() with the selected reservation
                            DeleteReservation.Show(reservation);
                            break;

                        case 3: // Cancel
                            return; // Return to the reservation list
                    }
                    break;

                case ConsoleKey.Escape:
                    return; // Exit the options and return to reservation list
            }

            Console.Clear(); // Refresh the options display
            Console.WriteLine($"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.TableChoice}");
            Console.WriteLine("Choose an action:");
        }
    }

    private static string GetUserFullName(long userID)
    {
        var account = AccountsAccess.GetById((int)userID); // Fetch the account details
        if (account != null)
        {
            return $"{account.FirstName} {account.LastName}";
        }
        return "Unknown User"; // Fallback in case no account is found
    }
}
