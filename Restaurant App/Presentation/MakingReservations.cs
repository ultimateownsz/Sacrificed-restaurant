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
            TryCatchHelper.EscapeKeyException(() =>
            {
                bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;

                // Step 1: Select the number of guests
                int guests = TryCatchHelper.EscapeKeyWithResult(GetGuestCount, -1, "Guest selection canceled.");
                if (guests == -1)
                {
                    ControlHelpPresent.DisplayFeedback("Guest selection canceled.", "bottom", "error");
                    return;
                }

                // Step 2: Calendar selection
                DateTime selectedDate = TryCatchHelper.EscapeKeyWithResult(() => SelectDate(guests, isAdmin, acc), DateTime.MinValue, "Date selection canceled.");	
                if (selectedDate == DateTime.MinValue)
                {
                    ControlHelpPresent.DisplayFeedback("Date selection canceled.", "bottom", "error");
                    return;
                }

                // Step 3: Table selection
                int selectedTable = TryCatchHelper.EscapeKeyWithResult(() => SelectTable(selectedDate, guests, isAdmin), -1, "Table selection canceled.");
                if (selectedTable == -1)
                {
                    ControlHelpPresent.DisplayFeedback("Table selection canceled.", "bottom", "error");
                    return;
                }

                // Step 4: Save reservation
                int reservationId = TryCatchHelper.EscapeKeyWithResult(() => SaveReservation(selectedDate, selectedTable, acc), 0, "Reservation not saved.");
                if (reservationId == 0)
                {
                    ControlHelpPresent.DisplayFeedback("Reservation not saved.", "bottom", "error");
                    return;
                }

                // Step 5: Take orders
                var orders = TryCatchHelper.EscapeKeyWithResult(() => TakeOrders(reservationId, guests), new List<ProductModel>(), "Order process canceled.");
                if (!orders.Any())
                {
                    ControlHelpPresent.DisplayFeedback("Order process canceled.", "bottom", "error");
                    return;
                }
                
                else // Print receipt
                {
                    PrintReceipt(orders, reservationId, acc);
                    ControlHelpPresent.DisplayFeedback("\nPress 'Escape' to return to the previous menu...", "bottom", "tip");
                    while (Console.ReadKey(true).Key != ConsoleKey.Escape) { }
                }
            });
        }

        private static int GetGuestCount()
        {
            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?\n\n";

            dynamic result = SelectionPresent.Show(options, banner, true);
            if (result?.index == -1) 
                throw new OperationCanceledException(); // Escape key pressed or invalid input
            return result?.index != null ? options.Count - result.index : -1;
        }

        private static DateTime SelectDate(int guests, bool isAdmin, UserModel acc)
        {
            return CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);
        }

        private static int SelectTable(DateTime selectedDate, int guests, bool isAdmin)
        {
            TableSelection tableSelection = new();

            int[] availableTables = guests switch
            {
                1 or 2 => [1, 4, 5, 8, 9, 11, 12, 15],
                3 or 4 => [6, 7, 10, 13, 14],
                5 or 6 => [2, 3],
                _ => Array.Empty<int>()
            };

            var reservedTables = Access.Reservations
                .GetAllBy<DateTime>("Date", selectedDate)
                .Where(r => r?.PlaceID != null)
                .Select(r => r!.PlaceID!.Value)
                .ToArray();

            return tableSelection.SelectTable(availableTables, [], reservedTables, guests, isAdmin);
        }

        private static int SaveReservation(DateTime selectedDate, int selectedTable, UserModel acc)
        {
            if (!acc.ID.HasValue)
            {
                ControlHelpPresent.DisplayFeedback("User ID is null. Unable to create reservation.");
                return 0;
            }

            int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);
            if (reservationId == 0)
            {
                ControlHelpPresent.DisplayFeedback("Failed to save reservation. Try again.");
                return 0;
            }
            return reservationId;
        }

        public static List<ProductModel> TakeOrders(int reservationId, int guests)
        {
            List<ProductModel> allOrders = new();
            List<string> categories = new() { "Appetizer", "Main", "Dessert", "Beverage" };

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
                    productOptions.Add("Skip this category");
                    productOptions.Add("Cancel");

                    dynamic selection = SelectionPresent.Show(productOptions, banner, false);
                    // Check for null or cancellation
                    if (selection?.text == null || selection?.text == "Cancel") 
                    {
                        ControlHelpPresent.DisplayFeedback("Order process canceled", "bottom", "error");
                        return allOrders;
                    }
                    else if (selection?.text == "Skip this category")
                    {
                        ControlHelpPresent.DisplayFeedback($"Category '{category}' skipped.", "bottom", "success");
                        continue;
                    }
                    var selectedProduct = products.FirstOrDefault(p => 
                        selection?.text?.StartsWith(p.Name) == true && selection?.text?.Contains($"{p.Price:0.00}") == true);

                    if (selectedProduct != null && selectedProduct.ID.HasValue)
                    {
                        var saveOrderResult = orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value);
                        if (!saveOrderResult.isValid)
                        {
                            ControlHelpPresent.DisplayFeedback($"Failed to save the order for '{category}'", "bottom", "error");
                            continue;
                        }
                        allOrders.Add(selectedProduct);
                        ControlHelpPresent.DisplayFeedback($"{selectedProduct.Name} has been added to your order!", "bottom", "success");
                    }
                }
            }
            return allOrders;
        }

        private static void PrintReceipt(List<ProductModel> orders, int reservationId, UserModel acc)
        {
            Console.Clear();
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Escape", "<escape>");
            ControlHelpPresent.ShowHelp();

           // Reserve lines for the footer
            int reservedFooterLines = ControlHelpPresent.GetFooterHeight(); // Dynamically determine footer height
            int receiptStartLine = 0; // Start drawing the receipt from the top

            Console.SetCursorPosition(0, receiptStartLine);
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
