using Project;
using Project.Presentation;

namespace Presentation
{
    static class FuturePastResrvations
    {
        public static void Show()
        {
            ViewAdmin();
            ViewUser();
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
                        break;
                    case "Cancel":
                        return;
                }
                // Console.Clear();
                // Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");
                // // System.Console.WriteLine("Enter the email of the client:");

                // var dateInput = Console.ReadLine();
                // if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                // {
                //     Console.WriteLine("Invalid date format. Press any key to try again.");
                //     Console.ReadKey();
                //     continue;
                // }

                // var accountInput = Console.ReadLine();

                // var userAccount = Access.Users.GetBy<string>("Email", accountInput);
                // if (userAccount == null)
                // {
                //     System.Console.WriteLine($"No user found with this email: {accountInput}");
                // }

                // Convert date to the required format (ddMMyyyy as integer)
                // var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate);

                // var userReservations = Access.Reservations.GetAllBy<int?>("UserID", userAccount.ID)
                //                                     .Where(r => r != null)
                //                                     .Cast<ReservationModel>()
                //                                     .ToList();

                // Fetch user names and table choices for reservations
                // var reservationDetails = reservations.Select(r => new
                // {
                //     Reservation = r,
                //     UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
                //     TableID = r.Place // Table choice of the reservation
                
                // }).ToList();

                // int selectedIndex = 0;
                // var pastReservations = reservationDetails.Where(r => r.Reservation.Date.HasValue && r.Reservation.Date.Value < DateTime.Now).ToList();
                // var futureReservations = reservationDetails.Where(r => r.Reservation.Date.HasValue && r.Reservation.Date.Value >= DateTime.Now).ToList();

                // switch (SelectionPresent.Show(["Past Reservations", "Future Reservations", "Cancel"], banner).text)
                // {
                //     case "Past Reservations":
                //         if (pastReservations.Count == 0)
                //         {
                //             Console.WriteLine("No past reservations found. Press any key to return");
                //             Console.ReadKey();
                //             break;
                //         }
                //         var pastOptions = pastReservations.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
                //         var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text;

                //         if (pastOptions.Contains(selectedPast))
                //         {
                //             int pastIndex = pastOptions.IndexOf(selectedPast);
                //             if (pastIndex >= 0 && pastIndex > pastReservations.Count)
                //             {
                //                 // ShowReservationOptions(pastReservations[pastIndex].Reservation);
                //                 break;
                //             }
                //         }
                //         break;
                //     case "Future Reservations":
                //         if (futureReservations.Count == 0)
                //         {
                //             Console.WriteLine("No current reservations found. Press any key to return");
                //             Console.ReadKey();
                //             break;
                //         }
                //         var futureOptions = futureReservations.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
                //         var selectedFuture = SelectionPresent.Show(futureOptions, "CURRENT RESERVATIONS\n\n").text;

                //         if (futureOptions.Contains(selectedFuture))
                //         {
                //             int futureIndex = futureOptions.IndexOf(selectedFuture);
                //             if (futureIndex >= 0 && futureIndex > futureReservations.Count)
                //             {
                //                 // ShowReservationOptions(futureReservations[futureIndex].Reservation);
                //                 break;
                //             }
                //         }
                //         break;
                //     case "Cancel":
                //         return;
                // }
            }
        }

        private static void ViewUser()
        {

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