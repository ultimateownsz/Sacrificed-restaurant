using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Project;
using Project.Logic;

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
            Console.WriteLine("RESERVATION MENU");
            // Step 1: Ask for the number of guests (only once)
            List<string> options = new() { "1", "2", "3", "4", "5", "6" };
            string banner = "How many guests will be coming?";
            int guests = options.Count() - SelectionPresent.Show(
                options, banner: banner, mode: SelectionLogic.Mode.Scroll).ElementAt(0).index;

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
                    ControlHelpPresent.DisplayFeedback("Returning to the previous menu...");
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
                    int selectedTable = tableSelection.SelectTable(availableTables, inactiveTables, reservedTables, guests, isAdmin);

                    if (selectedTable == -1)
                    {
                        ControlHelpPresent.DisplayFeedback("Returning to date selection...");
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
                        ControlHelpPresent.DisplayFeedback("User ID is not available. Unable to create reservation.");
                        return;
                    }

                    if (reservationId == 0)
                    {
                        ControlHelpPresent.DisplayFeedback("Failed to create a reservation. Please try again.");
                        continue; // Retry table selection
                    }

                    var orders = TakeOrders(selectedDate, acc, reservationId, guests);
                    PrintReceipt(orders, reservationId, acc);
                    
                    ControlHelpPresent.DisplayFeedback("Press Enter when you are ready to return to the menu...", "bottom", "tip");
                    while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
                    {
                        // Do nothing, just wait for Enter
                    }

                    return;
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
    //                 Console.ForegroundColor = ConsoleColor.Blue;
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
        if (reservationId == 0)
        {
            ControlHelpPresent.DisplayFeedback("Invalid reservation ID. Exiting TakeOrders.");
            return new List<ProductModel>(); // Return an empty list for invalid reservations
        }

        List<string> categories = new List<string> { "Appetizer", "Main", "Dessert", "Beverage" };
        List<ProductModel> allOrders = new List<ProductModel>();

        Console.WriteLine("This month's theme is:");
        ThemeModel? theme = ReservationMenuLogic.GetCurrentTheme(selectedDate);

        if (theme is not null)
        {
            Console.WriteLine($"{theme.Name}");
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("This month is not accessible.");
            ControlHelpPresent.DisplayFeedback("Press any key to return to the reservation menu.", "bottom", "tip");
            Console.ReadKey();
            return new List<ProductModel>(); // Return an empty list if no theme is available
        }

        // Fetch the reservation details using reservationId
        var reservation = Access.Reservations.GetBy<int>("ID", reservationId);
        if (reservation == null)
        {
            Console.WriteLine("Reservation not found. Unable to save orders.");
            return new List<ProductModel>();
        }

        // Create a temporary user for allergy handling
        Access.Users.Delete(-1);
        Access.Users.Write(new UserModel("", "", "", "", "", 0, -1));

        for (int i = 0; i < guests; i++)
        {
            int? id = (i == 0) ? acc.ID : -1; // Use account ID for the first guest, temporary ID (-1) for others
            List<ProductModel> guestOrder = new();

            // Start allergy handling for the guest
            LinkAllergyLogic.Start(LinkAllergyLogic.Type.User, id, (i == 0) ? null : i + 1);

            for (int z = 0; z < categories.Count; z++)
            {
                // Filter products by theme, category, and allergy restrictions
                List<ProductModel> products = ProductLogic
                    .GetAllWithinThemeCourse(categories[z], theme.ID)
                    .Where(product => !LinkAllergyLogic.IsAllergic(id, product.ID))
                    .ToList();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("RESERVATION MENU");

                    var banner = $"PRODUCT SELECTION\nGuest {i + 1}, choose a product for {categories[z]}:";
                    var productOptions = products.Select(p => $"{p.Name} - €{Convert.ToString(p.Price).Replace(".", ",")}\n").ToList();
                    productOptions.Add("Skip this course"); // Option to skip the course

                    var selectedOption = SelectionPresent.Show(productOptions, banner: banner).ElementAt(0).text;
                    if (selectedOption == "Skip this course")
                    {
                        break;
                    }

                    var selectedProduct = products.FirstOrDefault(p =>
                        selectedOption.StartsWith(p.Name) && selectedOption.Contains($"{Convert.ToString(p.Price).Replace(".", ",")}"));

                        // recommend product (drink pair)
                        PairModel linkage = Access.Pairs.GetBy<int?>("FoodID", selectedProduct.ID);
                        if (linkage != null)
                        {
                            ProductModel recommended = Access.Products.GetBy<int?>("ID", linkage.DrinkID);
                            string _banner = "DRINK PAIRING\n\nWould you like to pair " +
                                           $"{recommended.Name} with {selectedProduct.Name}";


                            switch (SelectionPresent.Show(["Yes", "No"], 
                                banner: _banner).ElementAt(0).index)
                            {
                                case 0:
                                    guestOrder.Add(recommended);
                                    break;
                            }
                        }

                        if (selectedProduct != null && selectedProduct.ID.HasValue)
                        {
                            guestOrder.Add(selectedProduct);

                            // EMERGENCY MODIFICATION: 1
                            var saveResult = orderLogic.SaveOrder(reservationId, selectedProduct.ID.Value);
                            if (!saveResult.isValid)
                            {
                               ControlHelpPresent.DisplayFeedback("Failed to save the order. Please try again.");
                               Console.ReadKey();
                               continue;
                            }

                        break; // Exit the selection loop for this category
                    }
                    else
                    {
                        ControlHelpPresent.DisplayFeedback("Invalid selection. Please try again.");
                        Console.ReadKey();
                    }
                }
            }

            allOrders.AddRange(guestOrder);

            // Cleanup temporary allergy links for the guest
            foreach (var lnk in Access.Allerlinks.Read().Where(x => x.EntityID == -1 && x.Personal == 1))
            {
                Access.Allerlinks.Delete(lnk.ID);
            }

            ControlHelpPresent.DisplayFeedback("\nPress any key to continue...", "bottom", "tip");
            Console.ReadKey();
        }

        // Remove the temporary user after all guests are processed
        Access.Users.Delete(-1);

        return allOrders; // Return the collected orders
    }



        public static void PrintReceipt(List<ProductModel> orders, int reservationId, UserModel acc)
        {
            Console.Clear();
            Console.WriteLine("=========== Receipt ===========");
            decimal totalAmount = 0;

            // Fetch the reservation using the passed reservationId
            var reservation = Access.Reservations.GetBy<int>("ID", reservationId);

            if (reservation == null)
            {
                ControlHelpPresent.DisplayFeedback("Reservation not found. Unable to display receipt.");
                return;
            }

            // Debug log to confirm correct reservation
            //Console.WriteLine($"DEBUG: Printing receipt for ReservationID: {reservation.ID}, Date: {reservation.Date}, PlaceID: {reservation.PlaceID}");

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Name of the customer:   {GetUserFullName(reservation.UserID)}");
            Console.WriteLine($"Reservation Date:       {reservation.Date:dd/MM/yyyy}");
            Console.WriteLine($"Table ID:               {reservation.PlaceID}");
            Console.WriteLine("-------------------------------");

            foreach (var product in orders)
            {
                Console.WriteLine($"{product.Name,-20}    €{product.Price:F2}");
                totalAmount += product.Price.HasValue ? (decimal)product.Price.Value : 0;
            }

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Total Amount:           €{totalAmount:F2}");
            Console.WriteLine($"Reservation number:     {reservation.ID}");
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