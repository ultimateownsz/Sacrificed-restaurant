using System.Collections;
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
                    continue;
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
            var testReservation = Access.Reservations.GetAllBy<int?>("UserID", acc.ID) // getting the user id's
                                                    .Where(r => r != null)
                                                    .Cast<ReservationModel>()
                                                    .ToList();

            var sortedReservations = testReservation
                        .OrderBy(r => r.Date)
                        .ToList(); // sorting the dates 

            if (sortedReservations == null || sortedReservations.Count == 0) // checking if the reservations exists
            {
                Console.WriteLine("You have no reservations.\nPress any key to return");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"Here are your reservations, {acc.FirstName}");

            int currentPage = 0;
            int itemsPerPage = 20;
            int totalPages = (int)Math.Ceiling((double)sortedReservations.Count / itemsPerPage); // reservations per page

            while (true)
            {
                var currentPageReserv = sortedReservations
                                            .Skip(currentPage * itemsPerPage)
                                            .Take(itemsPerPage)
                                            .ToList(); // making sure there are 20 reservations per page

                var reservationOptions = ReservationLogic.GenerateMenuOptions(currentPageReserv, currentPage, totalPages);
                var selectedReservations = SelectionPresent.Show(reservationOptions, "RESERVATIONS\n\n").text; // making use of SelectionPresent.Show

                if (selectedReservations == "Back")
                {
                    return;
                }

                if (selectedReservations == "Next Page >>") // option to go to the next page
                {
                    currentPage = Math.Min(currentPage + 1, totalPages -1);
                    continue;
                }

                if (selectedReservations == "<< Previous Page") // option to go to the previous page
                {
                    currentPage = Math.Max(currentPage - 1, 0);
                    continue;
                }

                if (currentPageReserv.Any(r => ReservationLogic.FormatAccount(r) == selectedReservations)) // ensuring that the options won't be read as reservation options
                {
                    Console.WriteLine($"You selected the reservation {selectedReservations}");
                    Console.WriteLine("Press any key to return...");
                    Console.ReadKey();
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