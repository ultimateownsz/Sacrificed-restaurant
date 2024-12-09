using System.Formats.Asn1;
using Project;
using Project.Logic;
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
            bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1; 
            DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin); // creating the calender (admin calender)

            while (true)
            {
                var reservations = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate); // getting all the dates from the date column

                if (!reservations.Any(r => r.Date.HasValue && r.Date.Value == selectedDate)) // ensuring the selected date exists in the database
                {
                    Console.Clear();
                    Console.WriteLine("There are no reservations for this date.\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                var reservationDetails = reservations.Select(r => new
                {
                    Reservation = r,
                    UserName = GetUserFullName(r.UserID),
                    TableID = r.PlaceID

                }).ToList();  // selecting info from reservation that are needed

                var reservationOptions = reservationDetails.Select(r => $"{r.UserName} - Table {r.TableID} (ID: {r.Reservation.ID})").ToList(); // using this info in a string
                var selectedReservation = SelectionPresent.Show(reservationOptions, "RESERVATIONS\n\n").text; // displaying the info as opions to choose

                if (reservationOptions.Contains(selectedReservation)) // esnuring that after a choice the admin is sent to the correct menu
                {
                    int reservationIndex = reservationOptions.IndexOf(selectedReservation);
                    if (reservationIndex >= 0 && reservationIndex < reservationDetails.Count)
                    {
                        ShowReservations.ShowReservationOptions(reservationDetails[reservationIndex].Reservation);
                    }
                }
            } // TODO: Add a escape key
        }

        private static void ViewUser(UserModel acc)
        {
            string banner = "Choose a sort reservation you would like to view\n\n";

            var testReservation = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                                    .Where(r => r != null)
                                                    .Cast<ReservationModel>()
                                                    .ToList();

            var sortedReservations = testReservation
                        .OrderBy(r => r.Date)
                        .ToList();

            if (sortedReservations == null || sortedReservations.Count == 0)
            {
                Console.WriteLine("You have no reservations.\nPress any key to return");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Here are your reservations, {acc.FirstName}");

            int currentPage = 0;
            int totalPages = (int)Math.Ceiling((double)sortedReservations.Count / 10);

            while (true)
            {
                var reservationOptions = ReservationLogic.GenerateMenuOptions(sortedReservations, currentPage, totalPages);
                var selectedReservations = SelectionPresent.Show(reservationOptions, "RESERVATIONS").text;

                if (selectedReservations == "Back")
                {
                    return;
                }

                if (selectedReservations == "Next Page >>")
                {
                    currentPage = Math.Min(currentPage + 1, totalPages -1);
                    continue;
                }

                if (selectedReservations == "<< Previous page")
                {
                    currentPage = Math.Max(currentPage - 1, 0);
                    continue;
                }

                if (reservationOptions.Contains(selectedReservations))
                {
                    Console.WriteLine($"You selected the reservation {selectedReservations}");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
                    return;
                }
            }

            // while (true)
            // {
            //     switch (SelectionPresent.Show(["Past reservations", "Future reservations", "Cancel"], banner).text) // showing options to the user
            //     {
            //         case "Past reservations":

            //             var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID) // getting the ID from the user
            //                                                     .Where(r => r != null)
            //                                                     .Cast<ReservationModel>()
            //                                                     .ToList();

            //             userReservations = userReservations // making sure that the date is from the past
            //                         .Where(r => r.Date.HasValue && r.Date.Value < DateTime.Now)
            //                         .OrderByDescending(r => r.Date)
            //                         .ToList();

            //             if (userReservations == null || userReservations.Count == 0) // checking if the reservation exists
            //             {
            //                 System.Console.WriteLine("You have no past reservations.\nPress any key to return...");
            //                 Console.ReadKey();
            //                 return;
            //             }

            //             System.Console.Clear();
            //             System.Console.WriteLine($"Here are your past reservations, {acc.FirstName}");

            //             var pastOptions = userReservations.Select(r => $"Reservation: {r.Date} (ID: {r.ID})").ToList(); // selecting info from userReservations
            //             var selectedPast = SelectionPresent.Show(pastOptions, "PAST RESERVATIONS\n\n").text; // displaying the info/options

            //             if (pastOptions.Contains(selectedPast)) // when choosing a date you get this message
            //             {
            //                 Console.Clear();
            //                 Console.WriteLine($"You selected Reservation on: {selectedPast}");
            //                 Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
            //                 Console.ReadKey();
            //             }
            //             break;
            //         case "Future reservations":

            //             var futureReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)// getting the user ID
            //                                                     .Where(r => r != null)
            //                                                     .Cast<ReservationModel>()
            //                                                     .ToList();

            //             futureReservations = futureReservations // making sure the date is a current date or a date from the future
            //                                 .Where(r => r.Date.HasValue && r.Date.Value >= DateTime.Now)
            //                                 .OrderByDescending(r => r.Date)
            //                                 .ToList();

            //             if (futureReservations == null || futureReservations.Count == 0) // checking if the date exists
            //             {
            //                 System.Console.WriteLine("You have no future reservations.\nPress any key to return...");
            //                 Console.ReadKey();
            //                 return;
            //             }

            //             Console.Clear();
            //             System.Console.WriteLine($"Here are your current and future reservations, {acc.FirstName}:");

            //             var futureOptions = futureReservations.Select(r => $"Reservations: {r.Date} (ID: {r.ID})").ToList(); // selecting info from the futureReservations
            //             var selectedFuture = SelectionPresent.Show(futureOptions, "FUTURE/CURRENT RESERVATIONS").text; // displaying the info/options

            //             if (futureOptions.Contains(selectedFuture)) // after choosing a option this is the message the user gets
            //             {
            //                 Console.Clear();
            //                 System.Console.WriteLine($"You selected Reservation on: {selectedFuture}");
            //                 Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
            //                 Console.ReadKey();
            //             }
            //         break;
            //         case "Cancel": // goes back to the previous menu
            //             return;
            //     }
            // }
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