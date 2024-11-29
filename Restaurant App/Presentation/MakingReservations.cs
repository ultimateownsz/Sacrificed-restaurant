using System;
using Project;

namespace Presentation
{
    public static class MakingReservations
    {
        static private ReservationLogic reservationLogic = new();
        static private ReservationMenuLogic reservationMenuLogic = new();
        static private OrderLogic orderLogic = new();
        //private static CalendarLogic calendarLogic = new CalendarLogic();

    public static void MakingReservation(UserModel acc, DateTime date)
    {   
        //Ask the user for the reservation amount
        Console.WriteLine("Please enter the number of guests between 1 and 6");
        string reservationAmount = Console.ReadLine();
        reservationAmount = reservationAmount.Replace(" ", "");
        bool isDigit = reservationAmount.All(char.IsDigit);
        while(string.IsNullOrEmpty(reservationAmount) || !isDigit || Convert.ToInt32(reservationAmount) < 1 || Convert.ToInt32(reservationAmount) > 6)
        {
            Console.Clear();
            Console.WriteLine("Invalid input");
            Console.WriteLine("Please enter a number between 1 and 6");
            reservationAmount = Console.ReadLine();
            isDigit = reservationAmount.All(char.IsDigit);
        }

        int reservationId = reservationLogic.SaveReservation(date, reservationAmount, acc.ID);
        OrderLogic orderLogic = new OrderLogic();
        List<string> categories = new List<string> { "Appetizer", "Main", "Dessert", "Beverage" };
        List<ProductModel> allOrders = new List<ProductModel>();
        
        Console.WriteLine("This month's theme is:");
        if(reservationMenuLogic.GetCurrentMenu() is not null)
            Console.WriteLine($"{reservationMenuLogic.GetCurrentMenu()}");
        else
        {
            Console.WriteLine("This month is not accessible");
            Console.WriteLine("Press any key to return to the reservation menu");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < Convert.ToInt32(reservationAmount); i++)
        {

            List<ProductModel> guestOrder = new List<ProductModel>();
            bool ordering = true;
            int geustNumber = i + 1;
            
            for (int z = 0; z < categories.Count(); z++)
            {
                // Product navigation within the selected category
                IEnumerable<ProductModel> products = Access.Products.GetAllBy<string>("Course", categories[z]);
                int productIndex = 0;
                bool choosingProduct = true;

                while (choosingProduct)
                {
                    Console.Clear();
                    Console.WriteLine("Press escape to move on");
                    Console.WriteLine($"Guest {geustNumber} Choose a product:");
                    Console.WriteLine($"Selected Category: {categories[z]}");

                    for (int k = 0; k < products.Count(); k++)
                    {
                        if (k == productIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"> {products.ElementAt(k).Name}({products.ElementAt(k).Course}) - €{products.ElementAt(k).Price:F2}"); // Highlight selected item with price
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"  {products.ElementAt(k).Name}({products.ElementAt(k).Course}) - €{products.ElementAt(k).Price:F2}");
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
                            if (productIndex < products.Count() - 1) productIndex++; // Move down
                            break;
                        case ConsoleKey.Enter:
                            choosingProduct = false; // Product selected
                            Console.WriteLine($"You selected {products.ElementAt(productIndex).Name} for ${products.ElementAt(productIndex).Price:F2}");
                            guestOrder.Add(products.ElementAt(productIndex)); // Add product to guest's order
                            orderLogic.SaveOrder(reservationId, products.ElementAt(productIndex).ID); // Save order to "database"
                            ordering = false;
                            break;
                        case ConsoleKey.Escape:
                            choosingProduct = false; // Exit product selection
                            ordering = false;
                            break;
                    }
                }
            }
            

            allOrders.AddRange(guestOrder);

            // Proceed to the next guest after finishing their order
            if(i == Convert.ToInt32(reservationAmount))
            {
                Console.WriteLine("\nPress any key to continue");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("\nPress any key to continue to the next guest...");
                Console.ReadKey();
            }
        }
        if(allOrders.Count == 0)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("  Invalid order, you have ordered nothing  ");
            Console.WriteLine("============================================");
            Console.WriteLine("\nPress any key to go close the prompt");
            Console.ReadKey();
        }
        else
        {
            PrintReceipt(allOrders, reservationId);
            Console.WriteLine("\nPress any key to go close the receipt");
            Console.ReadKey();
        }
        return;
    }


    public static void PrintReceipt(List<ProductModel> guestOrder, int reservationId)
        {
            Console.Clear();
            Console.WriteLine("===========Receipt===========");
            float? totalAmount = 0;

            foreach (var product in guestOrder)
            {
                Console.WriteLine($"{product.Name,-20} €{product.Price:F2}");
                totalAmount += product.Price;
            }

            Console.WriteLine("----------------------------");
            Console.WriteLine($"Total Amount:         €{totalAmount:F2}");
            Console.WriteLine($"Reservation code:      {reservationId}");
            Console.WriteLine("============================");
        }

    public static void UserOverViewReservation(UserModel acc)
    {
        int reservationIndex = 0;
        bool inResMenu = true;
        IEnumerable<ReservationModel> userReservations = reservationLogic.GetUserReservations(Convert.ToInt32(acc.ID)); 
        if (userReservations == null || userReservations.Count() == 0)
        {
            Console.WriteLine("You have no reservations.");
            Console.WriteLine("Choose a reservations to cancel it.");
            Console.ReadKey();
            return;
        }
        while (inResMenu)
        {
            Console.Clear();
            Console.WriteLine($"Here are your Reservations {acc.FirstName}:");
            for (int j = 0; j < userReservations.Count(); j++)
            {
                userReservations = reservationLogic.GetUserReservations(Convert.ToInt32(acc.ID)); 
                if (j == reservationIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"> Reservation:{userReservations.ElementAt(j).Date}"); // Highlight selected item
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  Reservation:{userReservations.ElementAt(j).Date}");
                }
            }

            var key = Console.ReadKey(intercept: true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (reservationIndex > 0) reservationIndex--; // Move up
                    break;
                case ConsoleKey.DownArrow:
                    if (reservationIndex < userReservations.Count() - 1) reservationIndex++; // Move down
                    break;
                case ConsoleKey.Enter:
                    // Process the selected reservation
                    Console.Clear();
                    Console.WriteLine($"You selected Reservation on: {userReservations.ElementAt(reservationIndex).Date}");
                    // Why in the flying fuck would "view" = "delete?"
                    //DeleteReservation(Convert.ToInt32(userReservations.ElementAt(reservationIndex).ID));
                    Console.WriteLine("Press any key to return to the reservation over view menu or press escape to return to the reservation menu...");
                    var key2 = Console.ReadKey();
                    if(key2.Key == ConsoleKey.Escape)
                    {
                        inResMenu = false;
                        break; // Exit loop
                    }
                    else
                    {
                        break;
                    }
                case ConsoleKey.Escape:// Exit without selection
                    inResMenu = false;
                    break;
            }
        }
        return;
    }

    public static void DeleteReservation(int resID)
    {
      if(reservationLogic.RemoveReservation(resID) is true)
      {
        // reservationLogic.RemoveReservation(resID);
        Console.WriteLine("This reservation has been removed");
        return;
      }
      else
      {
        Console.WriteLine("Failed to remove reservation");
        return;
      }
    }

    public static void DisplayCalendar(DateTime currentDate, int selectedDay)
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
            if (day == selectedDay)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{day,2} ");
                Console.ResetColor();
            }
            else
            {
                Console.Write($"{day,2} ");
            }

            if ((day + startDay) % 7 == 0) Console.WriteLine();
        }

        Console.WriteLine("\nUse Arrow Keys to Navigate, Enter to Select Date, P for Previous Month, N for Next Month, Q to Quit.");
    }

    public static void CalendarNavigation(UserModel acc)
    {
        DateTime currentDate = DateTime.Now;
        int selectedDay = currentDate.Day;
        bool running = true;

        while (running)
        {
            DisplayCalendar(currentDate, selectedDay);
            var key = Console.ReadKey(intercept: true);

            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (selectedDay > 1) selectedDay--;
                    break;
                case ConsoleKey.RightArrow:
                    if (selectedDay < DateTime.DaysInMonth(currentDate.Year, currentDate.Month)) selectedDay++;
                    break;
                case ConsoleKey.UpArrow:
                    if (selectedDay - 7 > 0)
                        selectedDay -= 7;
                    else
                        selectedDay = 1; // Jump to start if moving up goes out of bounds
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedDay + 7 <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
                        selectedDay += 7;
                    else
                        selectedDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month); // Jump to end if moving down goes out of bounds
                    break;
                case ConsoleKey.P: // Previous month
                    currentDate = currentDate.AddMonths(-1);
                    selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)); // Adjust selected day if new month has fewer days
                    break;
                case ConsoleKey.N: // Next month
                    currentDate = currentDate.AddMonths(1);
                    selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)); // Adjust selected day if new month has fewer days
                    break;
                case ConsoleKey.Enter: // Select date
                    DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, selectedDay);
                    //ShowAvailableTables(selectedDate);
                    MakingReservation(acc, selectedDate);
                    running = false;
                    break;
                case ConsoleKey.Q: // Quit
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                    break;
            }
        }
    }

    //public static void ShowAvailableTables(DateTime selectedDate)
    //{
    //    var availableTables = calendarLogic.GetAvailableTables(selectedDate);
    //    Console.WriteLine($"\nAvailable tables for {selectedDate.ToString("MMMM dd, yyyy")}:");
    //    foreach (var table in availableTables)
    //    {
    //        Console.WriteLine($" - {table}");
    //    }
    //    Console.WriteLine("\nPress any key to return to the calendar...");
    //    Console.ReadKey();
    //}
}
}

