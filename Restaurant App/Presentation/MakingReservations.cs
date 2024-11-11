static class MakingReservations
{

    static private ReservationLogic reservationLogic = new();
    static private ReservationMenuLogic reservationMenuLogic = new();
    static private OrderLogic orderLogic = new();

    public static void Start(AccountModel acc)//This function gets called through the menu to ask the user for his reservation information
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
                Dictionary<string, (Int64 productId, double Price)> product = new();
                double total = 0;
                List<string> orders = new();

                // Console.WriteLine("This month's theme is:");
                // Console.WriteLine($"{reservationMenuLogic.GetCurrentMenu()}");
                // for(int j = 0; i < Convert.ToInt32(reservationAmount); i++)
                // {
                //     string catagory = "";
                //     Console.WriteLine($"Guest {i + 1} You can start your order");
                //     switch(i)
                //     {
                //         case 0:
                //             catagory = "SideDishes";
                //             Console.WriteLine($"Here are the side dishes");
                //             break;
                //         case 1:
                //             catagory = "MainDishes";
                //             Console.WriteLine($"Here are the main dishes");
                //             break;
                //         case 2:
                //             catagory = "Desserts";
                //             Console.WriteLine($"Here are the desserts");
                //             break;
                //         case 3:
                //             catagory = "AlcoholicBeverages";
                //             Console.WriteLine($"Here are the alcoholic beverages");
                //             break;
                //     }

                OrderLogic orderLogic = new OrderLogic();
                List<string> categories = new List<string> { "SideDishes", "MainDishes", "Dessert", "Alcoholic Beverages" };


                for (int i = 0; i < Convert.ToInt32(reservationAmount); i++)
                {
                    Console.WriteLine($"\nGuest {i + 1}, You can start your order");

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
                        List<string> products = ProductManager.GetAllWithinCategory(categories[categoryIndex]).Select(p => p.ProductName).ToList();
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
                                    Console.WriteLine($"> {products[k]}"); // Highlight selected item
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"  {products[k]}");
                                }
                            }
                            
                            // Read key input for product navigation
                            var key = Console.ReadKey(intercept: true);
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
                                    Console.WriteLine($"You selected {products[productIndex]}");
                                    orderLogic.SaveOrder(i + 1, productIndex); // Save order to DB
                                    break;
                                case ConsoleKey.Escape:
                                    choosingProduct = false; // Exit product selection
                                    break;
                            }
                        }

                        // Ask if the user wants to add more products
                        Console.WriteLine("\nDo you want to add another product? (Y/N)");
                        var response = Console.ReadKey(intercept: true);
                        if (response.Key == ConsoleKey.N)
                        {
                            ordering = false; // Exit ordering loop for this guest
                        }
                    }

                    // Proceed to the next guest after finishing their order
                    Console.WriteLine("\nPress any key to continue to the next guest...");
                    Console.ReadKey();
                }
            }
                // foreach(ProductModel product in ProductManager.GetAllProducts())
                // {   
                //     products.Add(product.ProductName, (product.ProductId ,Convert.ToDouble(product.Price)));
                //     Console.WriteLine($"{product.Category}\n{product.ProductName}, Price: {product.Price}");
                // }

                // while(true)
                // {
                //     Console.WriteLine("Order more or type exit to move on");
                //     string productChoice = Console.ReadLine();
                //     if (productChoice == "exit")
                //         break;
                //     else if(products.ContainsKey(productChoice))
                //     {
                //         double price = products[productChoice].Price;
                //         Console.WriteLine($"You have ordered {productChoice} for {price}");
                //         total += price;
                //         orders.Add(productChoice);
                //         orderLogic.SaveOrder(reservationId, products[productChoice].productId);
                //     }
                //     else
                //     {
                //         Console.WriteLine("This item is not on the menu");
                //     }
                // }

                // Console.WriteLine($"Your reservation ID is {reservationId}");
                // foreach(string order in orders)
                // {
                //     Console.WriteLine(order);
                // }
                // Console.WriteLine($"Your total is {total}");


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
}