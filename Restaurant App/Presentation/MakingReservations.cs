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
            TryCatchHelper.EscapeKeyException(() =>
            {
                bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;

                // Step 1: Ask for the number of guests
                var guestOptions = Enumerable.Range(1, 6).Select(n => n.ToString()).ToList();
                string guestBanner = "How many guests will be coming?\n\n";
                dynamic guestSelection = SelectionPresent.Show(guestOptions, guestBanner, true).text;

                if (guestSelection == null) return;
                int guests = int.Parse(guestSelection);

                // Step 2: Display the calendar and mark unreservable dates
                DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);
                if (selectedDate == DateTime.MinValue) return;

                // Step 3: Select table
                int[] availableTables = GetAvailableTables(guests);
                var reservedTables = Access.Reservations
                    .GetAllBy<DateTime>("Date", selectedDate)
                    .Where(r => r?.PlaceID != null)
                    .Select(r => r!.PlaceID!.Value)
                    .ToArray();

                TableSelection tableSelection = new();
                int selectedTable = tableSelection.SelectTable(availableTables, reservedTables);
                if (selectedTable == -1)
                {
                    Console.WriteLine("Returning to the calendar...");
                    return;
                }

                // Step 4: Save the reservation
                int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);
                if (reservationId == 0)
                {
                    Console.WriteLine("Failed to create a reservation. Please try again.");
                    return;
                }

                var orders = TakeOrders(reservationId, guests);
                if (orders.Count > 0)
                {
                    PrintReceipt(orders, reservationId, acc);
                    Console.WriteLine("\nPress Enter to return to the main menu...");
                    while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) { }
                }
            });
        }

            private static int[] GetAvailableTables(int guests) =>
            guests switch
            {
                1 or 2 => new[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new[] { 6, 7, 10, 13, 14 },
                5 or 6 => new[] { 2, 3 },
                _ => Array.Empty<int>()
            };

        public static List<ProductModel> TakeOrders(int reservationId, int guests)
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
