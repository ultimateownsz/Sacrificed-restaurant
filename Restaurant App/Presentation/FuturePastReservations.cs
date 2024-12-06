using Project;
using Project.Presentation;

namespace Presentation
{
    static class FuturePastResrvations
    {
        public static void Show(UserModel acc, bool admin)
        {
            if (admin)
            {
                ViewAdmin();
            }
            else
            {
                ViewUser(acc);
            }
        }

        private static void ViewAdmin()
        {
            string banner = "Choose a sort reservation you would like to view\n\n";

            while (true)
            {
                switch (SelectionPresent.Show(["Past Reservations", "Future Reservations", "Cancel"], banner).text)
                {
                    case "Past Reservations":
                        Console.Clear();
                        Console.WriteLine("Enter a date from the past (dd/MM/yyyy) to view past reservations:");
                        var dateInput = Console.ReadLine();
                        if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                        {
                            Console.WriteLine("Invalid date format. Press any key to try again.");
                            Console.ReadKey();
                            continue;
                        }

                        if (parsedDate >= DateTime.Now.Date)
                        {
                            Console.WriteLine("The enterd date must be from the past. Press any key to try again.");
                            Console.ReadKey();
                            continue;
                        }

                        var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate);
                        var reservationDetails = reservations.Select(r => new
                        {
                            Reservation = r,
                            UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
                            TableID = r.Place // Table choice of the reservation
                        
                        }).ToList();

                        var pastOptions = reservationDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
                        var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text;

                        if (pastOptions.Contains(selectedPast))
                        {
                            int pastIndex = pastOptions.IndexOf(selectedPast);
                            if (pastIndex >= 0 && pastIndex > reservationDetails.Count)
                            {
                                ShowReservations.ShowReservationOptions(reservationDetails[pastIndex].Reservation);
                                break;
                            }
                        }

                        break;
                    case "Future Reservations":
                        Console.Clear();
                        Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");
                        var futureInput = Console.ReadLine();
                        if (!DateTime.TryParseExact(futureInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedFuture))
                        {
                            Console.WriteLine("Invalid date format. Press any key to try again.");
                            Console.ReadKey();
                            continue;
                        }

                        if (parsedFuture < DateTime.Now.Date)
                        {
                            Console.WriteLine("The entered date must be a current date or a future date. Press any key to try again.");
                            Console.ReadKey();
                            continue;
                        }

                        var futureReservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedFuture);
                        var futureDetails = futureReservations.Select(r => new
                        {
                            Reservation = r,
                            UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
                            TableID = r.Place // Table choice of the reservation
                        
                        }).ToList();

                        var futureOptions = futureDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
                        var selectedFuture = SelectionPresent.Show(futureOptions, "FUTURE/CURRENT RESERVATIONS").text;

                        if (futureOptions.Contains(selectedFuture))
                        {
                            int futureIndex = futureOptions.IndexOf(selectedFuture);
                            if (futureIndex >= 0 && futureIndex > futureDetails.Count)
                            {
                                ShowReservations.ShowReservationOptions(futureDetails[futureIndex].Reservation);
                                break;
                            }
                        }

                        break;
                    case "Cancel":
                        return;
                }
            }
        }

        public static void ViewUser(UserModel acc)
        {
            string banner = "Choose a sort reservation you would like to view\n\n";
            while (true)
            {
                switch (SelectionPresent.Show(["Past reservations", "Future reservations", "Cancel"], banner).text)
                {
                    case "Past reservations":
                        int reservationIndex = 0;
                        bool inResMenu = true;

                        var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                                                .Where(r => r != null)
                                                                .Cast<ReservationModel>()
                                                                .ToList();

                        userReservations = userReservations
                                    .Where(r => r.Date.HasValue && r.Date.Value < DateTime.Now)
                                    .OrderByDescending(r => r.Date)
                                    .ToList();

                        if (userReservations == null || userReservations.Count == 0)
                        {
                            System.Console.WriteLine("You have no past reservations. Press any key to return...");
                            Console.ReadKey();
                            return;
                        }

                        while (inResMenu)
                        {
                            System.Console.Clear();
                            System.Console.WriteLine($"Here are your past reservations, {acc.FirstName}");

                            for (int j = 0; j < userReservations.Count; j++)
                            {
                                if (j == reservationIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"> Reservation: {userReservations[j].Date}"); // Highlight selected item
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"  Reservation: {userReservations[j].Date}");
                                }
                            }

                            var key = Console.ReadKey(intercept: true);
                            switch (key.Key)
                            {
                                case ConsoleKey.UpArrow:
                                    if (reservationIndex > 0) reservationIndex--; // Move up
                                    break;

                                case ConsoleKey.DownArrow:
                                    if (reservationIndex < userReservations.Count - 1) reservationIndex++; // Move down
                                    break;

                                case ConsoleKey.Enter:
                                    // Process the selected reservation
                                    Console.Clear();
                                    Console.WriteLine($"You selected Reservation on: {userReservations[reservationIndex].Date}");
                                    Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
                                    var key2 = Console.ReadKey();

                                    if (key2.Key == ConsoleKey.Escape)
                                    {
                                        inResMenu = false;
                                        break; // Exit loop
                                    }
                                    else
                                    {
                                        break;
                                    }

                                case ConsoleKey.Escape: // Exit without selection
                                    inResMenu = false;
                                    break;
                            }
                        }
                        break;
                    case "Future reservations":
                        bool isValid = true;
                        int futureIndex = 0;

                        var futureReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                                                .Where(r => r != null)
                                                                .Cast<ReservationModel>()
                                                                .ToList();

                        futureReservations = futureReservations
                                            .Where(r => r.Date.HasValue && r.Date.Value >= DateTime.Now)
                                            .OrderByDescending(r => r.Date)
                                            .ToList();

                        if (futureReservations == null || futureReservations.Count == 0)
                        {
                            System.Console.WriteLine("You have no future reservations. Press any key to return...");
                            Console.ReadKey();
                            return;
                        }

                        while (isValid)
                        {
                            Console.Clear();
                            System.Console.WriteLine($"Here are your current and future reservations, {acc.FirstName}:");

                            for (int j = 0; j < futureReservations.Count; j++)
                            {
                                if (j == futureIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"> Reservation: {futureReservations[j].Date}"); // Highlight selected item
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"  Reservation: {futureReservations[j].Date}");
                                }
                            }

                            var key = Console.ReadKey(intercept: true);
                            switch (key.Key)
                            {
                                case ConsoleKey.UpArrow:
                                    if (futureIndex > 0) futureIndex--; // Move up
                                    break;

                                case ConsoleKey.DownArrow:
                                    if (futureIndex < futureReservations.Count - 1) futureIndex++; // Move down
                                    break;

                                case ConsoleKey.Enter:
                                    // Process the selected reservation
                                    Console.Clear();
                                    Console.WriteLine($"You selected Reservation on: {futureReservations[futureIndex].Date}");
                                    Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
                                    var key2 = Console.ReadKey();

                                    if (key2.Key == ConsoleKey.Escape)
                                    {
                                        inResMenu = false;
                                        break; // Exit loop
                                    }
                                    else
                                    {
                                        break;
                                    }

                                case ConsoleKey.Escape: // Exit without selection
                                    inResMenu = false;
                                    break;
                            }
                        }
                    break;
                    case "Cancel":
                        return;
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
}