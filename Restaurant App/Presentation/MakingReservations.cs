using System;
using System.Collections.Generic;
using System.IO.Compression;
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

            // Step 1: Ask for the number of guests (only once)
            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?\n\n";
            int guests = options.Count() - SelectionPresent.Show(options, banner, true).index;

            DateTime selectedDate;

            // Fetch inactive tables
            var inactiveTables = Access.Places.Read()
                .Where(p => p.Active == 0)
                .Select(p => p.ID.Value)
                .ToArray();

            while (true) // Loop to manage Calendar -> Table Selection navigation
            {
                // Step 2: Display the calendar and mark unreservable dates
                selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);

                if (selectedDate == DateTime.MinValue)
                {
                    Console.Clear(); // Ensure no residual data is left
                    Console.WriteLine("Returning to the previous menu...");
                    return; // Exit completely if user presses back from the calendar
                }


                // Step 3: Filter available tables based on the number of guests
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

                while (true) // Inner loop for Table Selection
                {
                    // Step 4: Select a table
                    int selectedTable = tableSelection.SelectTable(availableTables, inactiveTables, reservedTables, isAdmin);

                    if (selectedTable == -1)
                    {
                        Console.WriteLine("Returning to date selection...");
                        break; // Break the inner loop and return to the calendar
                    }

                    // Step 5: Save the reservation
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
                        continue; // Retry table selection
                    }

                    var orders = TakeOrders(selectedDate, acc, reservationId, guests);
                    if (orders.Count > 0)
                    {
                        PrintReceipt(orders, reservationId, acc);

                        // Prompt the user to press Enter to return to the menu
                        Console.WriteLine("\nPress Enter when you are ready to return to the menu...");
                        while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
                        {
                            // Do nothing, just wait for Enter
                        }
                        return; // Exit after completing reservation
                    }
                }
            }
        }






        // public static int SelectTableUsingTableSelection(DateTime selectedDate, int guests)
        // {
        //     int[] availableTables = guests switch
        //     {
        //         1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
        //         3 or 4 => new int[] { 6, 7, 10, 13, 14 },
        //         5 or 6 => new int[] { 2, 3 },
        //         _ => Array.Empty<int>()
        //     };

        //     if (availableTables.Length == 0)
        //     {
        //         Console.WriteLine("No available tables for this number of guests.");
        //         Console.ReadKey();
        //         return -1;
        //     }

        // var reservedTables = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate)
        //                                         .Select(r => r.PlaceID)
        //                                         .Where(rt => rt.HasValue)
        //                                         .Select(rt => rt.Value)
        //                                         .ToArray();


        //     TableSelection tableSelection = new();
		// int selectedTable = tableSelection.SelectTable(availableTables, reservedTables, isAdmin);

        // }

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
            List<string> categories = new() { "Appetizer", "Main", "Dessert", "Beverage" };
            List<ProductModel> allOrders = new();

            Console.WriteLine("This month's theme is:");
            string theme = reservationMenuLogic.GetCurrentMenu() ?? "No theme available.";
            Console.WriteLine(theme);

            for (int guest = 1; guest <= guests; guest++)
            {
                foreach (string category in categories)
                {
                    var products = ProductLogic.GetAllWithinCategory(category).ToList();
                    if (!products.Any()) continue;

                    string banner = $"Guest {guest}, choose a product for {category}:\n\n";
                    var productOptions = products.Select(p => $"{p.Name} - €{p.Price:F2}").ToList();
                    productOptions.Add("\nCancel");

                    dynamic selectedOption = SelectionPresent.Show(productOptions, banner).text;
                    if (selectedOption == "Cancel") return new List<ProductModel>();

                    var selectedProduct = products.FirstOrDefault(p => 
                        selectedOption.StartsWith(p.Name) && selectedOption.Contains($"{p.Price:0.00}"));
                    
                    if (selectedProduct != null && selectedProduct.ID.HasValue)
                    {
                        if (!orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value))
                        {
                            Console.WriteLine("Failed to save the order. Try again.");
                            continue;
                        }

                        allOrders.Add(selectedProduct);
                        Console.WriteLine($"{selectedProduct.Name} added successfully!");
                    }
                }
            }
            return allOrders;
        }

        public static void PrintReceipt(List<ProductModel> orders, int reservationId, UserModel acc)
        {
            Console.Clear();
            Console.WriteLine("=========== Receipt ===========");
            decimal totalAmount = 0;

            var reservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                .OrderByDescending(r => r?.Date)
                .FirstOrDefault();

            if (reservations != null)
            {
                Console.WriteLine($"Name:         {GetUserFullName(reservations.UserID)}");
                Console.WriteLine($"Date:         {reservations.Date:dd/MM/yyyy}");
                Console.WriteLine($"Table:        {reservations.PlaceID}\n-------------------------------");
            }

            foreach (var product in orders)
            {
                Console.WriteLine($"{product.Name,-20}    €{product.Price:F2}");
                totalAmount += product.Price.HasValue ? (decimal)product.Price.Value : 0;
            }
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Total Amount:           €{totalAmount:F2}");
            Console.WriteLine("===============================");
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
