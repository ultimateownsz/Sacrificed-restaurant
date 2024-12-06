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
            while (true) // Allow returning to the calendar
            {
                // Step 1: Select a date
                DateTime selectedDate = CalendarPresent.Show(DateTime.Now);

                // Step 2: Get the number of guests
                List<string> options = new() { "1", "2", "3", "4", "5", "6" };
                string banner = "How many guests will be coming?\n\n";
                int guests = options.Count() - SelectionPresent.Show(options, banner, true).index;

                // Step 3: Select a table
                int selectedTable = SelectTableUsingTableSelection(selectedDate, guests);

                // Ensure valid table selection
                if (selectedTable == -1)
                {
                    Console.WriteLine("Returning to the menu...");
                    break; // Exit back to the menu
                }

                Console.WriteLine($"DEBUG: Selected table {selectedTable}");

                // Step 4: Create reservation immediately after table selection
                int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);

                // Handle reservation failure
                if (reservationId == 0)
                {
                    Console.WriteLine("Failed to create a reservation. Please try again.");
                    continue; // Restart the process if reservation creation fails
                }

                Console.WriteLine($"DEBUG: Reservation created with ID={reservationId}");

                // Step 5: Proceed to meal selection
                TakeOrders(selectedDate, acc, reservationId, guests);
                Console.WriteLine("DEBUG: Completed meal selection");
                break; // Exit after completing the reservation
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
                                                .Select(r => r.Place)
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

    public static void TakeOrders(DateTime selectedDate, UserModel acc, int reservationId, int guests)
    {
        Console.WriteLine($"DEBUG: Entering TakeOrders with ReservationID={reservationId}");

        // Check if reservation ID is valid
        if (reservationId == 0)
        {
            Console.WriteLine("DEBUG: Invalid reservation ID. Exiting TakeOrders.");
            return;
        }

        List<string> categories = new List<string> { "Appetizer", "Main", "Dessert", "Beverage" };
        List<ProductModel> allOrders = new List<ProductModel>();

        Console.WriteLine("This month's theme is:");
        var reservationMenuLogic = new ReservationMenuLogic(); // Create an instance
        var theme = reservationMenuLogic.GetCurrentMenu(); // Call the method

        if (theme is not null)
        {
            Console.WriteLine($"{theme}");
        }
        else
        {
            Console.WriteLine("This month is not accessible.");
            Console.WriteLine("Press any key to return to the reservation menu.");
            Console.ReadKey();
            return;
        }


        for (int i = 0; i < guests; i++)
        {
            Console.WriteLine($"DEBUG: Starting order for Guest {i + 1}");
            List<ProductModel> guestOrder = new List<ProductModel>();

            for (int z = 0; z < categories.Count; z++)
            {
                var products = ProductManager.GetAllWithinCategory(categories[z]).ToList();
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

                            if (selectedProduct.ID == null || !ProductManager.DoesProductExist(selectedProduct.ID.Value))
                            {
                                Console.WriteLine("The selected product does not exist. Please try again.");
                                Console.ReadKey();
                                break;
                            }

                            guestOrder.Add(selectedProduct);
                            var orderLogic = new OrderLogic(); // Create an instance
                            if (!orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value))
                            {
                                Console.WriteLine("Failed to save the order. Please try again.");
                                Console.ReadKey();
                                break;
                            }


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
