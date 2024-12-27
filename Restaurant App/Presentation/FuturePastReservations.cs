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
            TryCatchHelper.EscapeKeyException(() =>
            {
                int guests = 1;
                bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
                DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc); // Calendar for admin

                while (true)
                {
                    var reservations = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

                    if (!reservations.Any())
                    {
                        Console.Clear();
                        ControlsHelperPresent.DisplayFeedback("There are no reservations for this date.\nPress any key to return...");
                        Console.ReadKey();
                        return;
                    }

                    var reservationDetails = reservations.Select(r => new
                    {
                        Reservation = r,
                        UserName = GetUserFullName(r.UserID) ?? "Unknown User",
                        TableID = r.PlaceID ?? 0
                    }).ToList();  // selecting info from reservation that are needed

                    var reservationOptions = reservationDetails
                        .Select((r, index) => $"{index + 1}. {r.UserName} - Table {r.TableID} (ID: {r.Reservation?.ID})")
                        .ToList(); // using this info in a string
                    
                    reservationOptions.Add("Back");
                    
                    // display the menu
                    var selectedReservation = SelectionPresent.Show(
                        reservationOptions, "RESERVATIONS\n\n").text;

                    //  handle null cases
                    if (string.IsNullOrEmpty(selectedReservation) || selectedReservation.Equals("Back", StringComparison.OrdinalIgnoreCase))
                        return;

                    // Safely parse user selection
                    string[] splitSelection = selectedReservation.Split('.');
                    if (splitSelection.Length > 0 && int.TryParse(splitSelection[0], out int selectedIndex))
                    {
                         if (selectedIndex > 0 && selectedIndex <= reservationDetails.Count)
                         {
                            var selectedRes = reservationDetails[selectedIndex - 1]?.Reservation;

                            if (selectedRes != null)
                            {
                                ShowReservations.ShowReservationOptions(selectedRes);
                            }
                            else
                            {
                                Console.WriteLine("Invalid selection. Returning to the menu...");
                                Console.ReadKey();
                            }
                         }
                    }
                }
            });
        }

        private static void ViewUser(UserModel acc)
        {
            TryCatchHelper.EscapeKeyException(() =>
            {
                var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                        ?.Where(r => r != null)
                                        .Cast<ReservationModel>()
                                        .OrderBy(r => r.Date)
                                        .ToList();

                if (userReservations == null || !userReservations.Any())
                {
                    ControlsHelperPresent.DisplayFeedback("You have no reservations.\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                int currentPage = 0;
                int itemsPerPage = 10;
                int totalPages = (int)Math.Ceiling((double)userReservations.Count / itemsPerPage);

                while (true)
                {
                    Console.Clear();

                    var currentPageReservations = userReservations
                        .Skip(currentPage * itemsPerPage)
                        .Take(itemsPerPage)
                        .ToList();

                    var reservationOptions = currentPageReservations
                        .Select(r => ReservationLogic.FormatAccount(r))
                        .ToList();

                    if (currentPage > 0) reservationOptions.Insert(0, "<< Previous Page");
                    if (currentPage < totalPages - 1) reservationOptions.Add("Next Page >>");
                    reservationOptions.Add("Back");

                    var selectedOption = SelectionPresent.Show(reservationOptions, "YOUR RESERVATIONS\n\n").text;

                    if (selectedOption == null || selectedOption == "Back") return;

                    if (selectedOption == "Next Page >>")
                    {
                        currentPage = Math.Min(currentPage + 1, totalPages - 1);
                        continue;
                    }

                    if (selectedOption == "<< Previous Page")
                    {
                        currentPage = Math.Max(currentPage - 1, 0);
                        continue;
                    }

                    var selectedReservation = currentPageReservations
                        .FirstOrDefault(r => ReservationLogic.FormatAccount(r) == selectedOption);

                    if (selectedReservation != null)
                    {
                        string banner = $"You selected {selectedOption}\n\n";
                        var action = SelectionPresent.Show(new List<string> { "Update Reservation", "\nBack" }, banner).text;

                        if (action == "Update Reservation")
                        {
                            UpdateReservation.Show(selectedReservation, false);
                        }
                    }
                }
            });

            // Console.WriteLine($"Here are your reservations, {acc.FirstName}");

            // int currentPage = 0;
            // int itemsPerPage = 20;
            // int totalPages = (int)Math.Ceiling((double)sortedReservations.Count / itemsPerPage); // reservations per page

            // while (true)
            // {
            //     var currentPageReserv = sortedReservations
            //                                 .Skip(currentPage * itemsPerPage)
            //                                 .Take(itemsPerPage)
            //                                 .ToList(); // making sure there are 20 reservations per page

            //     var reservationOptions = ReservationLogic.GenerateMenuOptions(currentPageReserv, currentPage, totalPages);
            //     var selectedReservations = SelectionPresent.Show(reservationOptions, "RESERVATIONS\n\n").text; // making use of SelectionPresent.Show

            //     if (selectedReservations == "Back")
            //     {
            //         return;
            //     }

            //     if (selectedReservations == "Next Page >>") // option to go to the next page
            //     {
            //         currentPage = Math.Min(currentPage + 1, totalPages -1);
            //         continue;
            //     }

            //     if (selectedReservations == "<< Previous Page") // option to go to the previous page
            //     {
            //         currentPage = Math.Max(currentPage - 1, 0);
            //         continue;
            //     }

            //     if (currentPageReserv.Any(r => ReservationLogic.FormatAccount(r) == selectedReservations)) // ensuring that the options won't be read as reservation options
            //     {
            //         var selectedResModel = currentPageReserv.FirstOrDefault(r => ReservationLogic.FormatAccount(r) == selectedReservations);

            //         if (selectedResModel != null)
            //         {
            //             var banner = $"You selected the {selectedReservations}\n\n";
            //             switch (SelectionPresent.Show(new List<string> { "Update Reservation" }, banner).text)
            //             {
            //                 case "Update Reservation":
            //                     UpdateReservation.Show(selectedResModel, false);
            //                     break;
            //             }
            //             Console.WriteLine("Press any key to return...");
            //             Console.ReadKey();
            //         }
            //         return;
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