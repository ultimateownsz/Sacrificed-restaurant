using Presentation;
using Project;

public static class ShowReservations
{
    public static void Show()
    {
        UserModel acc = new UserModel();
        FuturePastResrvations.Show(acc, true); // using the new method
    }

    public static void ShowReservationOptions(ReservationModel reservation)
    {
        // List of possible actions
        string[] actions = { "View Details", "Update Reservation", "Delete Reservation", "Cancel" };

        int currentActionIndex = 0;

        while (true)
        {
            Console.Clear(); // Refresh the options display
            Console.WriteLine($"Selected Reservation for: {GetUserFullName(reservation.UserID)} - Table {reservation.PlaceID}");
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
