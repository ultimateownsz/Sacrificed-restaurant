
using System;
using Logic;


namespace Presentation
{
    public static class MakingReservations
    {

        static private ReservationLogic reservationLogic = new();
        static private ReservationMenuLogic reservationMenuLogic = new();
        static private OrderLogic orderLogic = new();
        private static CalendarLogic calendarLogic = new CalendarLogic();

        public static void ReservationMenu(AccountModel acc)
        {
            while (true)
            {
                Console.WriteLine("Welcome to the reservation page");
                Console.WriteLine("1. Make reservation");
                Console.WriteLine("2. Remove reservation");
                Console.WriteLine("3. Exit");

                string input = Console.ReadLine().ToLower();

                if  (input == "1" || input == "make reservation")
                {
                    MakingReservation(acc);
                }

                else if (input == "2" || input == "remove reservation")
                {
                    Console.WriteLine("Please enter the reservation code(id)");
                    int removeID = Convert.ToInt32(Console.ReadLine());
                    
                    if(reservationLogic.RemoveReservation(removeID) == true)
                        Console.WriteLine("The reservation has been cancelled");
                    else
                        Console.WriteLine("Invalid ID, reservation has not been cancelled");
                }
                else
                    return;
            }
    }

    public static void MakingReservation(AccountModel acc)
    {
        //Ask the date of the reservation
        //Checks if the date is within a month
        string date;
        while (true)
        {
            Console.WriteLine("Please enter your desired date d/m/y");
            date = Console.ReadLine();
            DateTime currentDate = DateTime.Now;
            DateTime desiredDate = Convert.ToDateTime(date);
            if(desiredDate > currentDate.AddDays(30))
            {
                Console.WriteLine("The desired date is more than 30 days from the current date, please try again");
            }
            else if(desiredDate < currentDate)
            {
                Console.WriteLine("The desired date is lower than today's date, please try again");
            }
            else
                break;
        }
        
        //Ask the user for the reservation amount
        Console.WriteLine("Please enter the number of guests between 1 and 6");
        string reservationAmount = Console.ReadLine();
        while(Convert.ToInt32(reservationAmount) < 1 || Convert.ToInt32(reservationAmount) > 6)
        {
            Console.WriteLine("Please enter a number between 1 and 6");
            reservationAmount = Console.ReadLine();
        }

        Int64 reservationId = reservationLogic.SaveReservation(date, reservationAmount, acc.UserID);
        OrderLogic orderLogic = new OrderLogic();
        List<string> categories = new List<string> { "SideDishes", "MainDishes", "Dessert", "Alcoholic Beverages" };
        List<ProductModel> allOrders = new List<ProductModel>();
        
        Console.WriteLine("This month's theme is:");
        Console.WriteLine($"{reservationMenuLogic.GetCurrentMenu()}");

        for (int i = 0; i < Convert.ToInt32(reservationAmount); i++)
        {
            Console.WriteLine($"\nGuest {i + 1}, You can start your order");

            List<ProductModel> guestOrder = new List<ProductModel>();
            bool ordering = true;
            
            while (ordering)
            {
                // Category selection
                int categoryIndex = 0;
                bool choosingCategory = true;
                
                while (choosingCategory)
                {
                    Console.Clear();
                    Console.WriteLine("Choose a category:");
                    for (int j = 0; j < categories.Count; j++)
                    {
                        if (j == categoryIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"> {categories[j]}"); // Highlight selected item
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"  {categories[j]}");
                        }
                    }
                    
                    // Read key input for category navigation
                    var key = Console.ReadKey(intercept: true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (categoryIndex > 0) categoryIndex--; // Move up
                            break;
                        case ConsoleKey.DownArrow:
                            if (categoryIndex < categories.Count - 1) categoryIndex++; // Move down
                            break;
                        case ConsoleKey.Enter:
                            choosingCategory = false; // Category selected
                            break;
                    }
                }
                
                // Product navigation within the selected category
                List<ProductModel> products = ProductManager.GetAllWithinCategory(categories[categoryIndex]);
                int productIndex = 0;
                bool choosingProduct = true;

                while (choosingProduct)
                {
                    Console.Clear();
                    Console.WriteLine($"Selected Category: {categories[categoryIndex]}");
                    Console.WriteLine("Choose a product:");

                    for (int k = 0; k < products.Count; k++)
                    {
                        if (k == productIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"> {products[k].ProductName}({products[k].Type}) - ${products[k].Price:F2}"); // Highlight selected item with price
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"  {products[k].ProductName}({products[k].Type}) - ${products[k].Price:F2}");
                        }
                    }
                    
                    // Read key input for product navigation
                    var key = Console.ReadKey(intercept: true);
                    if (key.Key == ConsoleKey.Q)
                    {
                        // Skip to the next guest
                        choosingProduct = false;
                        ordering = false;
                        break;
                    }

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (productIndex > 0) productIndex--; // Move up
                            break;
                        case ConsoleKey.DownArrow:
                            if (productIndex < products.Count - 1) productIndex++; // Move down
                            break;
                        case ConsoleKey.Enter:
                            choosingProduct = false; // Product selected
                            Console.WriteLine($"You selected {products[productIndex].ProductName} for ${products[productIndex].Price:F2}");
                            guestOrder.Add(products[productIndex]); // Add product to guest's order
                            orderLogic.SaveOrder(reservationId, products[productIndex].ProductId); // Save order to "database"
                            break;
                        case ConsoleKey.Escape:
                            choosingProduct = false; // Exit product selection
                            break;
                    }
                }

                if (!ordering) break;

                // Ask if the user wants to add more products
                Console.WriteLine("\nDo you want to order another product? (Y/N)");
                var response = Console.ReadKey(intercept: true);
                if (response.Key == ConsoleKey.N)
                {
                    ordering = false; // Exit ordering loop for this guest
                }
            }

            allOrders.AddRange(guestOrder);

            // Proceed to the next guest after finishing their order
            Console.WriteLine("\nPress any key to continue to the next guest...");
            Console.ReadKey();
            if(allOrders.Count == 0)
            {
                Console.WriteLine("============================================");
                Console.WriteLine("  Invalid order, you have ordered nothing  ");
                Console.WriteLine("============================================");
            }
            else
                PrintReceipt(allOrders, reservationId);
        }
    }


    public static void PrintReceipt(List<ProductModel> guestOrder, Int64 reservationId)
        {
            Console.Clear();
            Console.WriteLine("===========Receipt===========");
            decimal totalAmount = 0;

            foreach (var product in guestOrder)
            {
                Console.WriteLine($"{product.ProductName,-20} ${product.Price:F2}");
                totalAmount += product.Price;
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine($"Total Amount:         ${totalAmount:F2}");
            Console.WriteLine($"Reservation code:      {reservationId}");
            Console.WriteLine("============================");
        }

    public static void UserOverViewReservation()
    {
        reservationLogic.GetUserReservatoions
    }

    public static void DeleteReservation()
    {

    }

        public static void DisplayCalendar(DateTime currentDate)
        {
            Console.Clear();
            Console.WriteLine(currentDate.ToString("MMMM yyyy").ToUpper());

            // Display calendar days layout
            Console.WriteLine("Mo Tu We Th Fr Sa Su");
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;

            // Adjust for 0-based index for Monday start
            startDay = startDay == 0 ? 6 : startDay - 1;

            // Print spaces for the first week
            for (int i = 0; i < startDay; i++)
                Console.Write("   ");

            for (int day = 1; day <= daysInMonth; day++)
            {
                Console.Write($"{day,2} ");
                if ((day + startDay) % 7 == 0) Console.WriteLine();
            }

            Console.WriteLine("\nPREVIOUS         NEXT");
            Console.WriteLine("Press P for Previous month, N for Next month, S to select a date, or Q to quit.");
        }

        public static void CalendarNavigation()
        {
            DateTime currentDate = DateTime.Now;
            bool running = true;

            while (running)
            {
                DisplayCalendar(currentDate);
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "p":
                        currentDate = currentDate.AddMonths(-1);
                        break;
                    case "n":
                        currentDate = currentDate.AddMonths(1);
                        break;
                    case "s":
                        Console.Write("Enter day to select: ");
                        if (int.TryParse(Console.ReadLine(), out int day) && day >= 1 && day <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
                        {
                            DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, day);
                            ShowAvailableTables(selectedDate);
                        }
                        else
                        {
                            Console.WriteLine("Invalid day.");
                        }
                        break;
                    case "q":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }
            }
        }

        public static void ShowAvailableTables(DateTime selectedDate)
        {
            var availableTables = calendarLogic.GetAvailableTables(selectedDate);
            Console.WriteLine($"Available tables for {selectedDate.ToString("MMMM dd, yyyy")}:");
            foreach (var table in availableTables)
            {
                Console.WriteLine($" - {table}");
            }
        }
    }
}
