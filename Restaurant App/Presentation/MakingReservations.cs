using System;
using System.Collections.Generic;
using System.Linq;
using Project;

namespace Presentation
{
    public static class MakingReservations
    {
        static private ReservationLogic reservationLogic = new();
        static private ReservationMenuLogic reservationMenuLogic = new();
        static private OrderLogic orderLogic = new();

        public static void MakingReservation(UserModel acc)
        {
            bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
            DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin);

            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?\n\n";
            int guests = options.Count() - SelectionPresent.Show(options, banner, true).index;

            TableSelection tableSelection = new();
            int[] availableTables = guests switch
            {
                1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                5 or 6 => new int[] { 2, 3 },
                _ => Array.Empty<int>()
            };
            var reservedTables = Access.Reservations
                                        .GetAllBy<DateTime>("Date", selectedDate)
                                        .Where(r => r?.PlaceID != null)
                                        .Select(r => r!.PlaceID!.Value)
                                        .ToArray();

            int selectedTable = tableSelection.SelectTable(availableTables, reservedTables);

            if (selectedTable == -1)
            {
                Console.WriteLine("Returning to the calendar...");
                return;
            }

            int reservationId;
            if (acc.ID.HasValue)
            {
                reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);
            }
            else
            {
                Console.WriteLine("Error: User ID is null. Unable to create reservation.");
                return;
            }
            if (reservationId == 0)
            {
                Console.WriteLine("Failed to create a reservation. Please try again.");
                return;
            }

            var orders = TakeOrders(selectedDate, acc, reservationId, guests);
            if (orders.Count > 0)
            {
                PrintReceipt(orders, reservationId);

                // Prompt the user to press Enter to return to the menu
                Console.WriteLine("\nPress Enter when you are ready to return to the menu...");
                while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
                {
                    // Do nothing, just wait for Enter
                }
            }
        }


        public static int SelectTableUsingTableSelection(DateTime selectedDate, int guests)
        {
            int[] availableTables = guests switch
            {
                1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                5 or 6 => new int[] { 2, 3 },
                _ => Array.Empty<int>()
            };

            if (availableTables.Length == 0)
            {
                Console.WriteLine("No available tables for this number of guests.");
                Console.ReadKey();
                return -1;
            }

        var reservedTables = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate)
                                                .Select(r => r.PlaceID)
                                                .Where(rt => rt.HasValue)
                                                .Select(rt => rt.Value)
                                                .ToArray();


            TableSelection tableSelection = new();
            return tableSelection.SelectTable(availableTables, reservedTables);
        }

    // public static void UserOverViewReservation(UserModel acc)
    // {
    //     int reservationIndex = 0;
    //     bool inResMenu = true;

    //     var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
    //                                             .Where(r => r != null)
    //                                             .Cast<ReservationModel>()
    //                                             .ToList();

    //     if (userReservations == null || userReservations.Count == 0)
    //     {
    //         Console.WriteLine("You have no reservations.");
    //         Console.WriteLine("Press any key to return to the main menu...");
    //         Console.ReadKey();
    //         return;
    //     }

    //     while (inResMenu)
    //     {
    //         Console.Clear();
    //         Console.WriteLine($"Here are your Reservations, {acc.FirstName}:");

    //         // Display reservations with navigation
    //         for (int j = 0; j < userReservations.Count; j++)
    //         {
    //             if (j == reservationIndex)
    //             {
    //                 Console.ForegroundColor = ConsoleColor.Yellow;
    //                 Console.WriteLine($"> Reservation: {userReservations[j].Date}"); // Highlight selected item
    //                 Console.ResetColor();
    //             }
    //             else
    //             {
    //                 Console.WriteLine($"  Reservation: {userReservations[j].Date}");
    //             }
    //         }

    //         var key = Console.ReadKey(intercept: true);
    //         switch (key.Key)
    //         {
    //             case ConsoleKey.UpArrow:
    //                 if (reservationIndex > 0) reservationIndex--; // Move up
    //                 break;

    //             case ConsoleKey.DownArrow:
    //                 if (reservationIndex < userReservations.Count - 1) reservationIndex++; // Move down
    //                 break;

    //             case ConsoleKey.Enter:
    //                 // Process the selected reservation
    //                 Console.Clear();
    //                 Console.WriteLine($"You selected Reservation on: {userReservations[reservationIndex].Date}");
    //                 Console.WriteLine("Press any key to return to the reservation overview menu or press Escape to return to the main menu...");
    //                 var key2 = Console.ReadKey();

    //                 if (key2.Key == ConsoleKey.Escape)
    //                 {
    //                     inResMenu = false;
    //                     break; // Exit loop
    //                 }
    //                 else
    //                 {
    //                     break;
    //                 }

    //             case ConsoleKey.Escape: // Exit without selection
    //                 inResMenu = false;
    //                 break;
    //         }
    //     }

    //     return;
    // }

        public static List<ProductModel> TakeOrders(DateTime selectedDate, UserModel acc, int reservationId, int guests)
        {
            Console.WriteLine($"DEBUG: Entering TakeOrders with ReservationID={reservationId}");

            if (reservationId == 0)
            {
                Console.WriteLine("DEBUG: Invalid reservation ID. Exiting TakeOrders.");
                return new List<ProductModel>(); // Return an empty list for invalid reservations
            }

            List<string> categories = new List<string> { "Appetizer", "Main", "Dessert", "Beverage" };
            List<ProductModel> allOrders = new List<ProductModel>();

            Console.WriteLine("This month's theme is:");
            var theme = reservationMenuLogic.GetCurrentMenu();

            if (theme is not null)
            {
                Console.WriteLine($"{theme}");
            }
            else
            {
                Console.WriteLine("This month is not accessible.");
                Console.WriteLine("Press any key to return to the reservation menu.");
                Console.ReadKey();
                return new List<ProductModel>(); // Return an empty list if no theme is available
            }

            for (int i = 0; i < guests; i++)
            {
                List<ProductModel> guestOrder = new();
                // Replace manual navigation logic with SelectionPresent.Show
                for (int z = 0; z < categories.Count; z++)
                {
                    List<ProductModel> products = ProductManager.GetAllWithinCategory(categories[z]).ToList();

                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Guest {i + 1}, choose a product for {categories[z]}:");

                        // Create menu options for SelectionPresent.Show
                        var productOptions = products.Select(p => $"{p.Name} - €{p.Price:F2}").ToList();
                        productOptions.Add("Cancel"); // Option to cancel or skip

                        // Display the menu and get the selected option
                        var selectedOption = SelectionPresent.Show(productOptions, "PRODUCT SELECTION\n\n").text;

                        if (selectedOption == "Cancel")
                        {
                            Console.WriteLine("Selection canceled. Returning to the previous menu.");
                            Console.ReadKey();
                            break;
                        }

                        // Find the selected product based on the menu text
                        var selectedProduct = products.FirstOrDefault(p => 
                            selectedOption.StartsWith(p.Name) && selectedOption.Contains($"{p.Price:0.00}"));

                        if (selectedProduct != null && selectedProduct.ID.HasValue)
                        {
                            guestOrder.Add(selectedProduct);

                            if (!orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value))
                            {
                                Console.WriteLine("Failed to save the order. Please try again.");
                                Console.ReadKey();
                                continue;
                            }

                            Console.WriteLine($"{selectedProduct.Name} added successfully!");
                            Console.ReadKey();
                            break; // Exit the selection loop for this category
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. Please try again.");
                            Console.ReadKey();
                        }
                    }
                }

                allOrders.AddRange(guestOrder);
                Console.WriteLine("\nPress any key to continue to the next guest...");
                Console.ReadKey();
            }

            return allOrders; // Return the collected orders
        }



        public static void PrintReceipt(List<ProductModel> orders, int reservationId)
        {
            Console.Clear();
            Console.WriteLine("=========== Receipt ===========");
            decimal totalAmount = 0;

            foreach (var product in orders)
            {
                Console.WriteLine($"{product.Name,-20} €{product.Price:F2}");

                // Convert nullable float to decimal, treat null as 0
                totalAmount += product.Price.HasValue ? (decimal)product.Price.Value : 0;
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine($"Total Amount:         €{totalAmount:F2}");
            Console.WriteLine($"Reservation ID:        {reservationId}");
            Console.WriteLine("============================");
        }
    }
}
