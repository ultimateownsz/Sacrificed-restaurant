using System;
using System.Collections.Generic;
using System.Linq;
using Project;

namespace Presentation
{
    public static class MakingReservations
    {
        private static ReservationLogic reservationLogic = new();
        private static ReservationMenuLogic reservationMenuLogic = new();
        private static OrderLogic orderLogic = new();

        public static void MakingReservation(UserModel acc)
        {
            bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;

            // Step 1: Select the number of guests using arrow keys
            int guests = GetGuestCount();
            if (guests == -1) // Escape key pressed during selection
            {
                Console.WriteLine("Guest selection canceled. Returning to the main menu...");
                return;
            }

            DateTime selectedDate;

            var inactiveTables = Access.Places.Read()
                .Where(p => p.Active == 0)
                .Select(p => p.ID.Value)
                .ToArray();

            while (true)
            {
                // Step 2: Calendar selection
                selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);
                if (selectedDate == DateTime.MinValue)
                {
                    Console.WriteLine("Returning to the previous menu...");
                    return; // Escape pressed
                }

                // Step 3: Table selection
                int selectedTable = SelectTable(selectedDate, guests, inactiveTables, isAdmin);
                if (selectedTable == -1) continue; // Return to the calendar view

                // Step 4: Save reservation
                int reservationId = SaveReservation(selectedDate, selectedTable, acc);
                if (reservationId == 0) continue;

                // Step 5: Take orders
                var orders = TakeOrders(selectedDate, acc, reservationId, guests);
                if (orders.Count > 0)
                {
                    PrintReceipt(orders, reservationId, acc);
                    Console.WriteLine("\nPress Enter to return to the main menu...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                    return;
                }
            }
        }

        private static int GetGuestCount()
        {
            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?\n\n";

            dynamic result = SelectionPresent.Show(options, banner, true);
            if (result.index == -1) return -1; // Escape pressed
            return options.Count - result.index;
        }

        private static int SelectTable(DateTime selectedDate, int guests, int[] inactiveTables, bool isAdmin)
        {
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

            return tableSelection.SelectTable(availableTables, inactiveTables, reservedTables, guests, isAdmin);
        }

        private static int SaveReservation(DateTime selectedDate, int selectedTable, UserModel acc)
        {
            if (!acc.ID.HasValue)
            {
                Console.WriteLine("Error: User ID is null. Unable to create reservation.");
                return 0;
            }

            int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);
            if (reservationId == 0)
            {
                Console.WriteLine("Failed to save reservation. Try again.");
                return 0;
            }
            return reservationId;
        }

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
                    productOptions.Add("Cancel");

                    dynamic selection = SelectionPresent.Show(productOptions, banner).text;
                    if (selection == "Cancel") return new List<ProductModel>();

                    var selectedProduct = products.FirstOrDefault(p => 
                        selection.StartsWith(p.Name) && selection.Contains($"{p.Price:0.00}"));

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

        private static void PrintReceipt(List<ProductModel> orders, int reservationId, UserModel acc)
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
                totalAmount += (decimal)(product.Price ?? 0);
            }
                Console.WriteLine("-------------------------------");
                Console.WriteLine($"Total Amount:           €{totalAmount:F2}");
                Console.WriteLine("===============================");
        }

        private static string GetUserFullName(int? userID)
        {
            var account = Access.Users.GetBy<int?>("ID", userID);
            return account != null ? $"{account.FirstName} {account.LastName}" : "Unknown User";
        }
    }
}
