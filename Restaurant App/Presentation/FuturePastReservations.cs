// using System.Collections;
// using System.Formats.Asn1;
// using Project;
// using Project.Logic;
// using Project.Presentation;

// namespace Presentation
// {
//     static class FuturePastReservations
//     {
//         public static void Show(UserModel acc, bool admin) // One method that makes use of modularity
//         {
//             if (admin) // if statement to check wether the user is a admin or not
//             {
//                 ViewAdmin(acc);
//             }
//             else
//             {
//                 ViewUser(acc);
//             }
//         }

//         // private static void ViewAdmin(UserModel acc)
//         // {
//         //     TryCatchHelper.EscapeKeyException(() =>
//         //     {
//         //         int guests = 1;
//         //         bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
//         //         DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc); // Calendar for admin

//         //         while (true)
//         //         {
//         //             var reservations = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

//         //             if (!reservations.Any())
//         //             {
//         //                 Console.Clear();
//         //                 ControlHelpPresent.DisplayFeedback("There are no reservations for this date.\nPress any key to return...");
//         //                 Console.ReadKey();
//         //                 return;
//         //             }

//         //             var reservationDetails = reservations.Select(r => new
//         //             {
//         //                 Reservation = r,
//         //                 UserName = GetUserFullName(r!.UserID) ?? "Unknown User",
//         //                 TableID = r.PlaceID ?? 0
//         //             }).ToList();  // selecting info from reservation that are needed

//         //             var reservationOptions = reservationDetails
//         //                 .Select((r, index) => $"{index + 1}. {r.UserName} - Table {r.TableID} (ID: {r.Reservation?.ID})")
//         //                 .ToList(); // using this info in a string
                    
//         //             reservationOptions.Add("Back");
                    
//         //             // display the menu
//         //             var selectedReservation = SelectionPresent.Show(
//         //                 reservationOptions, "RESERVATIONS\n\n").text;

//         //             //  handle null cases
//         //             if (string.IsNullOrEmpty(selectedReservation) || selectedReservation.Equals("Back", StringComparison.OrdinalIgnoreCase))
//         //                 return;

//         //             // Safely parse user selection
//         //             string[] splitSelection = selectedReservation.Split('.');
//         //             if (splitSelection.Length > 0 && int.TryParse(splitSelection[0], out int selectedIndex))
//         //             {
//         //                  if (selectedIndex > 0 && selectedIndex <= reservationDetails.Count)
//         //                  {
//         //                     var selectedRes = reservationDetails[selectedIndex - 1]?.Reservation;

//         //                     if (selectedRes != null)
//         //                     {
//         //                         ShowReservations.ShowReservationOptions(selectedRes);
//         //                     }
//         //                     else
//         //                     {
//         //                         Console.WriteLine("Invalid selection. Returning to the menu...");
//         //                         Console.ReadKey();
//         //                     }
//         //                  }
//         //             }
//         //         }
//         //     });
//         // }

//         private static void ViewUser(UserModel acc)
//         {
//             TryCatchHelper.EscapeKeyException(() =>
//             {
//                 var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
//                                         ?.Where(r => r != null)
//                                         .Cast<ReservationModel>()
//                                         .OrderBy(r => r.Date)
//                                         .ToList();

//                 if (userReservations == null || !userReservations.Any())
//                 {
//                     ControlHelpPresent.DisplayFeedback("You have no reservations.\nPress any key to return...");
//                     Console.ReadKey();
//                     return;
//                 }

//                 int currentPage = 0;
//                 int itemsPerPage = 10;
//                 int totalPages = (int)Math.Ceiling((double)userReservations.Count / itemsPerPage);

//                 while (true)
//                 {
//                     Console.Clear();

//                     var currentPageReservations = userReservations
//                         .Skip(currentPage * itemsPerPage)
//                         .Take(itemsPerPage)
//                         .ToList();

//                     var reservationOptions = currentPageReservations
//                         .Select(r => ReservationLogic.FormatAccount(r))
//                         .ToList();

//                     if (currentPage > 0) reservationOptions.Insert(0, "<< Previous Page");
//                     if (currentPage < totalPages - 1) reservationOptions.Add("Next Page >>");
//                     reservationOptions.Add("Back");

//                     var selectedOption = SelectionPresent.Show(reservationOptions, "YOUR RESERVATIONS\n\n").text;

//                     if (selectedOption == null || selectedOption == "Back") return;

//                     if (selectedOption == "Next Page >>")
//                     {
//                         currentPage = Math.Min(currentPage + 1, totalPages - 1);
//                         continue;
//                     }

//                     if (selectedOption == "<< Previous Page")
//                     {
//                         currentPage = Math.Max(currentPage - 1, 0);
//                         continue;
//                     }

//                     var selectedReservation = currentPageReservations
//                         .FirstOrDefault(r => ReservationLogic.FormatAccount(r) == selectedOption);

//                     if (selectedReservation != null)
//                     {
//                         // string banner = $"You selected {selectedOption}\n\n";
//                         // var action = SelectionPresent.Show(new List<string> { "Update Reservation", "\nBack" }, banner).text;

//                         // if (action == "Update Reservation")
//                         // {
//                         UpdateReservation.Show(selectedReservation, false);
//                         // }
//                     }
//                 }
//             });
//         }

//         private static string GetUserFullName(int? userID)
//         {
//             var account = Access.Users.GetBy<int?>("ID", userID); // Fetch the account details
//             if (account != null)
//             {
//                 return $"{account.FirstName} {account.LastName}";
//             }
//             return "Unknown User"; // Fallback in case no account is found
//         }
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Logic;
using Project.Presentation;

namespace Presentation
{
    static class FuturePastReservations
    {
        public static void Show(UserModel acc, bool isAdmin = false) // One method that makes use of modularity
        {
            TryCatchHelper.EscapeKeyException(() =>
            {
                List<ReservationModel> reservations;

                while (true)
                {

                
                    if (isAdmin)
                    {
                        DateTime selectedDate = CalendarPresent.Show(DateTime.Now, true, 1, acc);
                        reservations = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate)
                            ?.Where(r => r != null) // Filter out null values
                            .Cast<ReservationModel>() // Cast to ReservationModel
                            .OrderBy(r => r.Date)
                            .ToList() ?? new List<ReservationModel>(); // Default to empty list

                        if (selectedDate == DateTime.MinValue) // escape key pressed
                        {
                            ControlHelpPresent.DisplayFeedback("Date selection cancelled.");
                            // Console.ReadKey();
                            return;
                        }

                        if (reservations == null || !reservations.Any())
                        {
                            // Console.Clear();
                            ControlHelpPresent.DisplayFeedback(
                                $"No reservations found for {selectedDate:MM-dd-yyyy}. You can still navigate to other dates.",
                                "bottom", feedbackType: "tip", 2000);
                            continue; // Allow user to pick another date
                        }
                    }
                    else
                    {
                        reservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                                            ?.Where(r => r != null)
                                                            .Select(r => r!)
                                                            .OrderBy(r => r.Date)
                                                            .ToList() ?? new List<ReservationModel>();

                        if (reservations == null || !reservations.Any())
                        {
                            ControlHelpPresent.DisplayFeedback("You have no reservations. Press any key to return...");
                            Console.ReadKey();
                            return;
                        }
                    }

                    int currentPage = 0;
                    int itemsPerPage = 10;
                    int totalPages = (int)Math.Ceiling((double)reservations.Count / itemsPerPage);

                    while (true)
                    {
                        Console.Clear();

                        ControlHelpPresent.Clear();
                        ControlHelpPresent.ResetToDefault();
                        ControlHelpPresent.ShowHelp();

                        var currentPageReservations = reservations
                            .Skip(currentPage * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToList();

                        var reservationOptions = currentPageReservations
                            .Select((r, index) =>
                                isAdmin
                                    ? $"{index + 1 + currentPage * itemsPerPage}. {GetUserFullName(r.UserID)} - Table {r.PlaceID} (ID: {r.ID})"
                                    : ReservationLogic.FormatAccount(r))
                            .ToList();

                        if (currentPage > 0) reservationOptions.Insert(0, "<< Previous Page");
                        if (currentPage < totalPages - 1) reservationOptions.Add("Next Page >>");
                        reservationOptions.Add("Back");

                        var selectedOption = SelectionPresent.Show(reservationOptions, 
                            isAdmin ? "RESERVATIONS\n\n" : "YOUR RESERVATIONS\n\n").text;

                        if (string.IsNullOrEmpty(selectedOption) || selectedOption.Equals("Back", StringComparison.OrdinalIgnoreCase))
                            return;
                        
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

                        ReservationModel? selectedReservation = null;
                        if (isAdmin)
                        {
                            string[] splitSelection = selectedOption.Split('.');
                            if (splitSelection.Length > 0 && int.TryParse(splitSelection[0], out int selectedIndex))
                            {
                                if (selectedIndex > 0 && selectedIndex <= currentPageReservations.Count)
                                {
                                    selectedReservation = currentPageReservations[selectedIndex - 1];
                                }
                            }
                        }
                        else
                        {
                            selectedReservation = currentPageReservations
                                .FirstOrDefault(r => ReservationLogic.FormatAccount(r) == selectedOption);
                        }


                        if (selectedReservation != null)
                        {
                            if (isAdmin)
                            {
                                ShowReservations.ShowReservationOptions(selectedReservation);
                            }
                            else
                            {
                                UpdateReservation.Show(selectedReservation, false);
                            }
                        }
                    }
                }
            });
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
