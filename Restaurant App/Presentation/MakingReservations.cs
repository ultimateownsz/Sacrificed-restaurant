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
            while (true) // Loop to allow returning to the calendar
            {
                // Step 1: Display the calendar and let the user select a date
                DateTime selectedDate = CalendarPresent.Show(DateTime.Now);

                // Step 2: Ask the user for the number of guests
                List<string> options = new() { "1", "2", "3", "4", "5", "6" };
                string banner = "How many guests will be coming?\n\n";
                int guests = options.Count() - SelectionPresent.Show(options, banner, true).index;

                // Step 3: Use TableSelection for table selection
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

                // Step 4: Check if the user pressed Back
                if (selectedTable == -1)
                {
                    Console.WriteLine("Returning to the calendar...");
                    continue; // Restart the process, allowing the user to select a new date
                }

                // Step 5: Proceed to take orders
                TakeOrders(selectedDate, acc, selectedTable, guests);
                break; // Exit the loop if the reservation is completed
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

    public static void UserOverViewReservation(UserModel acc)
    {
        int reservationIndex = 0;
        bool inResMenu = true;

        // Use GetAllBy to fetch reservations for the user
        var userReservations = Access.Reservations.GetAllBy<int?>("UserID", acc.ID)
                                                .Where(r => r != null)
                                                .Cast<ReservationModel>()
                                                .ToList();

        if (userReservations == null || userReservations.Count == 0)
        {
            Console.WriteLine("You have no reservations.");
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
            return;
        }

        while (inResMenu)
        {
            Console.Clear();
            Console.WriteLine($"Here are your Reservations, {acc.FirstName}:");

            // Display reservations with navigation
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

        return;
    }



        public static void TakeOrders(DateTime selectedDate, UserModel acc, int tableId, int guests)
        {
            // Save the reservation
            int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID, tableId);

            List<string> categories = new List<string> { "Appetizer", "Main", "Dessert", "Beverage" };
            List<ProductModel> allOrders = new List<ProductModel>();

            Console.WriteLine("This month's theme is:");
            if (reservationMenuLogic.GetCurrentMenu() is not null)
                Console.WriteLine($"{reservationMenuLogic.GetCurrentMenu()}");
            else
            {
                Console.WriteLine("This month is not accessible.");
                Console.WriteLine("Press any key to return to the reservation menu.");
                Console.ReadKey();
                return;
            }

            for (int i = 0; i < guests; i++)
            {
                List<ProductModel> guestOrder = new();
                for (int z = 0; z < categories.Count; z++)
                {
                    List<ProductModel> products = ProductManager.GetAllWithinCategory(categories[z]).ToList();

                    int productIndex = 0;
                    bool choosingProduct = true;

                    while (choosingProduct)
                    {
                        Console.Clear();
                        Console.WriteLine($"Guest {i + 1}, choose a product for {categories[z]}:");
                        for (int k = 0; k < products.Count; k++)
                        {
                            if (k == productIndex)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"> {products[k].Name} - €{products[k].Price:F2}");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine($"  {products[k].Name} - €{products[k].Price:F2}");
                            }
                        }

                        var key = Console.ReadKey(intercept: true);
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (productIndex > 0) productIndex--;
                                break;
                            case ConsoleKey.DownArrow:
                                if (productIndex < products.Count - 1) productIndex++;
                                break;
                            case ConsoleKey.Enter:
                                guestOrder.Add(products[productIndex]);
                                orderLogic.SaveOrder(reservationId, products[productIndex].ID);
                                choosingProduct = false;
                                break;
                            case ConsoleKey.Escape:
                                choosingProduct = false;
                                break;
                        }
                    }
                }

                allOrders.AddRange(guestOrder);
                Console.WriteLine("\nPress any key to continue to the next guest...");
                Console.ReadKey();
            }

            PrintReceipt(allOrders, reservationId);
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
