using Presentation;
using Project;

public static class ShowReservations
{
    public static void Show()
    {
        UserModel acc = new UserModel();
        FuturePastResrvations.Show(acc, true);
        // int placeID = 1;
        // string banner = "Choose a sort reservation you would like to view\n\n";

        // while (true)
        // {
        //     Console.Clear();
        //     Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");
        //     // System.Console.WriteLine("Enter the email of the client:");

        //     var dateInput = Console.ReadLine();
        //     if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        //     {
        //         Console.WriteLine("Invalid date format. Press any key to try again.");
        //         Console.ReadKey();
        //         continue;
        //     }

        //     // var accountInput = Console.ReadLine();

        //     // var userAccount = Access.Users.GetBy<string>("Email", accountInput);
        //     // if (userAccount == null)
        //     // {
        //     //     System.Console.WriteLine($"No user found with this email: {accountInput}");
        //     // }

        //     // Convert date to the required format (ddMMyyyy as integer)
        //     var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate);

        //     // var userReservations = Access.Reservations.GetAllBy<int?>("UserID", userAccount.ID)
        //     //                                     .Where(r => r != null)
        //     //                                     .Cast<ReservationModel>()
        //     //                                     .ToList();

        //     // Fetch user names and table choices for reservations
        //     var reservationDetails = reservations.Select(r => new
        //     {
        //         Reservation = r,
        //         UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
        //         TableID = r.Place // Table choice of the reservation
            
        //     }).ToList();

        //     // int selectedIndex = 0;
        //     var pastReservations = reservationDetails.Where(r => r.Reservation.Date.HasValue && r.Reservation.Date.Value < DateTime.Now).ToList();
        //     var futureReservations = reservationDetails.Where(r => r.Reservation.Date.HasValue && r.Reservation.Date.Value >= DateTime.Now).ToList();

        //     switch (SelectionPresent.Show(["Past Reservations", "Future Reservations", "Cancel"], banner).text)
        //     {
        //         case "Past Reservations":
        //             if (pastReservations.Count == 0)
        //             {
        //                 Console.WriteLine("No past reservations found. Press any key to return");
        //                 Console.ReadKey();
        //                 break;
        //             }
        //             var pastOptions = pastReservations.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
        //             var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text;

        //             if (pastOptions.Contains(selectedPast))
        //             {
        //                 int pastIndex = pastOptions.IndexOf(selectedPast);
        //                 if (pastIndex >= 0 && pastIndex > pastReservations.Count)
        //                 {
        //                     ShowReservationOptions(pastReservations[pastIndex].Reservation);
        //                     break;
        //                 }
        //             }
        //             break;
        //         case "Future Reservations":
        //             if (futureReservations.Count == 0)
        //             {
        //                 Console.WriteLine("No current reservations found. Press any key to return");
        //                 Console.ReadKey();
        //                 break;
        //             }
        //             var futureOptions = futureReservations.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
        //             var selectedFuture = SelectionPresent.Show(futureOptions, "CURRENT RESERVATIONS\n\n").text;

        //             if (futureOptions.Contains(selectedFuture))
        //             {
        //                 int futureIndex = futureOptions.IndexOf(selectedFuture);
        //                 if (futureIndex >= 0 && futureIndex > futureReservations.Count)
        //                 {
        //                     ShowReservationOptions(futureReservations[futureIndex].Reservation);
        //                     break;
        //                 }
        //             }
        //             break;
        //         case "Cancel":
        //             return;
        //     }

            // while (true)
            // {
            //     Console.Clear();
            //     Console.WriteLine($"Reservations for {parsedDate:dd/MM/yyyy}:");

            //     // Display reservations with highlight for the selected one
            //     for (int i = 0; i < reservationDetails.Count(); i++)
            //     {
            //         if (i == selectedIndex)
            //         {
            //             Console.ForegroundColor = ConsoleColor.Yellow; // Highlight selected
            //             Console.WriteLine($"-> {reservationDetails[i].UserName} - Assigned to Table {reservationDetails[i].TableID}");
            //         }
            //         else
            //         {
            //             Console.ResetColor(); // Default color for others
            //             Console.WriteLine($"  {reservationDetails[i].UserName} - Assigned to Table {reservationDetails[i].TableID}");
            //         }
            //     }

            //     Console.ResetColor(); // Reset any lingering color changes
            //     Console.WriteLine("\nUse arrow keys to navigate. Press Enter to select, or Esc to go back to the Admin Menu.");

            //     var key = Console.ReadKey(true);

            //     switch (key.Key)
            //     {
            //         case ConsoleKey.UpArrow:
            //             if (selectedIndex > 0)
            //                 selectedIndex--;
            //             break;

            //         case ConsoleKey.DownArrow:
            //             if (selectedIndex < reservationDetails.Count() - 1)
            //                 selectedIndex++;
            //             break;

            //         case ConsoleKey.Enter:
            //             // Show options for the selected reservation
            //             ShowReservationOptions(reservationDetails[selectedIndex].Reservation);
            //             break;

            //         case ConsoleKey.Escape:
            //             return;
            //     }
            // }
        }
    // }

    private static void ShowReservationOptions(ReservationModel reservation)
    {
        // List of possible actions
        string[] actions = { "View Details", "Update Reservation", "Delete Reservation", "Cancel" };

        int currentActionIndex = 0;

        while (true)
        {
            Console.Clear(); // Refresh the options display
            Console.WriteLine($"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.Place}");
            Console.WriteLine("Choose an action:");

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
                            Console.ReadKey();
                            break;

                        case 3: // Cancel
                            return; // Return to the reservation list
                    }
                    break;

                case ConsoleKey.Escape:
                    return; // Exit the options and return to reservation list
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
