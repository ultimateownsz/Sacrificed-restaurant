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
            // Step 1: Ask the user for the number of guests
            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?\n\n";
            int guests = options.Count() - SelectionPresent.Show(options, banner, true).index;

            // Step 2: Display the calendar and let the user select a date
            DateTime selectedDate = CalendarPresentation.Show(DateTime.Now);

            // Step 3: Select a table and proceed with the reservation
            SelectTable(acc, selectedDate, guests);
        }

        public static void SelectTable(UserModel acc, DateTime selectedDate, int guests)
        {
            // Define available tables based on guest count
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
                return;
            }

            // Allow the user to select a table
            TableSelection tableSelection = new();
            int selectedTable;
            do
            {
                Console.Clear();
                Console.WriteLine($"Available tables for {guests} guests on {selectedDate:MMMM dd, yyyy}:");
                selectedTable = tableSelection.SelectTable(availableTables, reservationLogic.GetReservedTablesByDate(selectedDate));

                if (selectedTable == -1) // User chose to go back
                {
                    MakingReservation(acc); // Restart the reservation process
                    return;
                }

                if (!Array.Exists(availableTables, table => table == selectedTable))
                {
                    Console.WriteLine($"Table {selectedTable} is not available. Please select a valid table.");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                }
            } while (!Array.Exists(availableTables, table => table == selectedTable));

            Console.WriteLine($"Table {selectedTable} selected for {guests} guests.");

            // Save the reservation
            int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID, selectedTable);

            // Proceed to take orders
            TakeOrders(reservationId, guests);
        }

        public static void TakeOrders(int reservationId, int guests)
        {
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
                bool ordering = true;

                for (int z = 0; z < categories.Count; z++)
                {
                    List<ProductModel> products = ProductManager.GetAllWithinCategory(categories[z]);
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
