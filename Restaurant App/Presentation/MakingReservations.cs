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

			// Call GridPresent to handle grid rendering and admin check
			if (!GridPresent.Show(isAdmin))
			{
				// If admin, exit after showing the admin-only message
				Console.WriteLine("Returning to the main menu...");
				return;
			}

			// Regular process for users proceeds here
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

    public static void UserOverViewReservation(UserModel acc)
    {
        int reservationIndex = 0;
        bool inResMenu = true;

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

        public static List<ProductModel> TakeOrders(DateTime selectedDate, UserModel acc, int reservationId, int guests)
        {
            if (reservationId == 0)
            {
                Console.WriteLine("Invalid reservation ID. Exiting TakeOrders.");
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
                                var selectedProduct = products[productIndex];

                                if (selectedProduct.ID.HasValue)
                                {
                                    guestOrder.Add(selectedProduct);

                                    if (!orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value))
                                    {
                                        Console.WriteLine("Failed to save the order. Please try again.");
                                        Console.ReadKey();
                                        break;
                                    }

                                    choosingProduct = false;
                                }
                                else
                                {
                                    Console.WriteLine("Error: Selected product has no valid ID. Please try again.");
                                    Console.ReadKey();
                                }
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
