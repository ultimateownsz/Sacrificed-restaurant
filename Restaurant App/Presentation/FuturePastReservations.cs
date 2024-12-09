using System.Formats.Asn1;
using Project;
using Project.Presentation;

namespace Presentation
{
    static class FuturePastResrvations
    {
        public static void Show(UserModel acc, bool admin) // One method that makes use of modularity
        {
            if (admin) // if statement to check wether the user is a admin or not
            {
                ViewAdmin(acc);
            }
            else
            {
                ViewUser(acc);
            }
        }

        private static void ViewAdmin(UserModel acc)
        {
            string banner = "Choose a sort reservation you would like to view\n\n";

            bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
            DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin);

            while (true)
            {
                var calReservations = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

                var calDetails = calReservations.Select(r => new
                {
                    Reservation = r,
                    UserName = GetUserFullName(r.UserID),
                    TableID = r.PlaceID

                }).ToList();

                var calOptions = calDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList();
                var selectedCal = SelectionPresent.Show(calOptions, "RESERVATIONS\n").text;

                if (calOptions.Contains(selectedCal))
                {
                    int calIndex = calOptions.IndexOf(selectedCal);
                    if (calIndex >= 0 && calIndex < calDetails.Count);
                }
            }

            // while (true)
            // {
            //     switch (SelectionPresent.Show(["Past Reservations", "Future Reservations", "Cancel"], banner).text) // displaying three options to the admin
            //     {
            //         case "Past Reservations":
            //             Console.Clear();
            //             Console.WriteLine("Enter a date from the past (dd/MM/yyyy) to view past reservations:");
            //             var dateInput = Console.ReadLine();
            //             if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate)) // check if the input date is in a valid format
            //             {
            //                 Console.WriteLine("Invalid date format.\nPress any key to try again.");
            //                 Console.ReadKey();
            //                 continue;
            //             }

            //             if (parsedDate >= DateTime.Now.Date) // checking if the date is a past reservation
            //             {
            //                 Console.WriteLine("The enterd date must be from the past.\nPress any key to try again.");
            //                 Console.ReadKey();
            //                 continue;
            //             }

            //             var reservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedDate); // getting all reservations

            //             if (!reservations.Any(r => r.Date == parsedDate)) // checking if the DB contains the input date
            //             {
            //                 System.Console.WriteLine("There are no past reservations for this date.\nPress any key to return...");
            //                 Console.ReadKey();
            //                 return;
            //             }

            //             var reservationDetails = reservations.Select(r => new
            //             {
            //                 Reservation = r,
            //                 UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
            //                 TableID = r.PlaceID // Table choice of the reservation
                        
            //             }).ToList();

            //             var pastOptions = reservationDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList(); // selecting the needed info from reservation details
            //             var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text; // displaying the admin all the reservations on the date that has been input

            //             if (pastOptions.Contains(selectedPast)) // getting the index for the showing of reservation options
            //             {
            //                 int pastIndex = pastOptions.IndexOf(selectedPast);
            //                 if (pastIndex >= 0 && pastIndex < reservationDetails.Count)
            //                 {
            //                     ShowReservations.ShowReservationOptions(reservationDetails[pastIndex].Reservation);
            //                 }
            //             }
            //             break;
            //         case "Future Reservations":
            //             Console.Clear();
            //             Console.WriteLine("Enter a specific date (dd/MM/yyyy) to view reservations:");
            //             var futureInput = Console.ReadLine();
            //             if (!DateTime.TryParseExact(futureInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedFuture)) // checking if the input date is of a valid fomrat
            //             {
            //                 Console.WriteLine("Invalid date format.\nPress any key to try again.");
            //                 Console.ReadKey();
            //                 continue;
            //             }

            //             if (parsedFuture < DateTime.Now.Date) // checking if the input date is not from the past
            //             {
            //                 Console.WriteLine("The entered date must be a current date or a future date.\nPress any key to try again.");
            //                 Console.ReadKey();
            //                 continue;
            //             }

            //             var futureReservations = Access.Reservations.GetAllBy<DateTime>("Date", parsedFuture); // getting all the dates

            //             if (!futureReservations.Any(r => r.Date.HasValue && r.Date.Value.Date == parsedFuture)) // checking if the DB contains the input date
            //             {
            //                 System.Console.WriteLine("There are no current or future reservations for this date.\nPress any key to return...");
            //                 Console.ReadKey();
            //                 return;
            //             }

            //             var futureDetails = futureReservations.Select(r => new
            //             {
            //                 Reservation = r,
            //                 UserName = GetUserFullName(r.UserID), // Helper method to get the user's name
            //                 TableID = r.PlaceID // Table choice of the reservation
                        
            //             }).ToList();

            //             var futureOptions = futureDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList(); // selecting info from futureDetails
            //             var selectedFuture = SelectionPresent.Show(futureOptions, "FUTURE/CURRENT RESERVATIONS").text; // displaying the admin all the reservations on the date that has been input

            //             if (futureOptions.Contains(selectedFuture)) // getting the index for showing reservation options
            //             {
            //                 int futureIndex = futureOptions.IndexOf(selectedFuture);
            //                 if (futureIndex >= 0 && futureIndex < futureDetails.Count)
            //                 {
            //                     ShowReservations.ShowReservationOptions(futureDetails[futureIndex].Reservation);
            //                 }
            //             }
            //             break;
            //         case "Cancel": // goes back to the previous menu
            //             return;
            //     }
            // }
        }

        private static void ViewUser(UserModel acc)
        {
            string banner = "Choose a sort reservation you would like to view\n\n";
            while (true)
            {
                switch (SelectionPresent.Show(["Past reservations", "Future reservations", "Cancel"], banner).text) // showing options to the user
                {
                    case "Past reservations":

                        var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID) // getting the ID from the user
                                                                .Where(r => r != null)
                                                                .Cast<ReservationModel>()
                                                                .ToList();

                        userReservations = userReservations // making sure that the date is from the past
                                    .Where(r => r.Date.HasValue && r.Date.Value < DateTime.Now)
                                    .OrderByDescending(r => r.Date)
                                    .ToList();

                        if (userReservations == null || userReservations.Count == 0) // checking if the reservation exists
                        {
                            System.Console.WriteLine("You have no past reservations.\nPress any key to return...");
                            Console.ReadKey();
                            return;
                        }

                        System.Console.Clear();
                        System.Console.WriteLine($"Here are your past reservations, {acc.FirstName}");

                        var pastOptions = userReservations.Select(r => $"Reservation: {r.Date} (ID: {r.ID})").ToList(); // selecting info from userReservations
                        var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text; // displaying the info/options

                        if (pastOptions.Contains(selectedPast)) // when choosing a date you get this message
                        {
                            Console.Clear();
                            Console.WriteLine($"You selected Reservation on: {selectedPast}");
                            Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
                            Console.ReadKey();
                        }
                        break;
                    case "Future reservations":

                        var futureReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)// getting the user ID
                                                                .Where(r => r != null)
                                                                .Cast<ReservationModel>()
                                                                .ToList();

                        futureReservations = futureReservations // making sure the date is a current date or a date from the future
                                            .Where(r => r.Date.HasValue && r.Date.Value >= DateTime.Now)
                                            .OrderByDescending(r => r.Date)
                                            .ToList();

                        if (futureReservations == null || futureReservations.Count == 0) // checking if the date exists
                        {
                            System.Console.WriteLine("You have no future reservations.\nPress any key to return...");
                            Console.ReadKey();
                            return;
                        }

                        Console.Clear();
                        System.Console.WriteLine($"Here are your current and future reservations, {acc.FirstName}:");

                        var futureOptions = futureReservations.Select(r => $"Reservations: {r.Date} (ID: {r.ID})").ToList(); // selecting info from the futureReservations
                        var selectedFuture = SelectionPresent.Show(futureOptions, "FUTURE/CURRENT RESERVATIONS").text; // displaying the info/options

                        if (futureOptions.Contains(selectedFuture)) // after choosing a option this is the message the user gets
                        {
                            Console.Clear();
                            System.Console.WriteLine($"You selected Reservation on: {selectedFuture}");
                            Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
                            Console.ReadKey();
                        }
                    break;
                    case "Cancel": // goes back to the previous menu
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