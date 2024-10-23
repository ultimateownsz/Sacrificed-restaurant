static class MakingReservations
{

    static private ReservationLogic reservationLogic = new();
    static private ReservationMenuLogic reservationMenuLogic = new();
    static private OrderLogic orderLogic = new();

    public static void Start(AccountModel acc)//This function gets called through the menu to ask the user for his reservation information
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
            
            //Ask the user for the table of choice
            Console.WriteLine("Please enter your desired table choice\n1.Table for two\n2.Table for four\n3.Table for six");
            string tableChoice = Console.ReadLine();

            //Ask the user for the reservation amount
            Console.WriteLine("Please enter the number of guests");
            string reservationAmount = Console.ReadLine();

            Int64 reservationId = reservationLogic.SaveReservation(date, tableChoice, reservationAmount, acc.UserID);
            Dictionary<string, (Int64 productId, double Price)> products = new();
            double total = 0;
            List<string> orders = new();

            Console.WriteLine("Here is the menu");
            Console.WriteLine($"{reservationMenuLogic.GetCurrentMenu()}");

            foreach(ProductModel product in ProductManager.GetAllProducts())
            {   
                products.Add(product.ProductName, (product.ProductId ,Convert.ToDouble(product.Price)));
                Console.WriteLine($"{product.Category}\n{product.ProductName}, Price: {product.Price}");
            }

            while(true)
            {
                Console.WriteLine("Order more or type exit to move on");
                string productChoice = Console.ReadLine();
                if (productChoice == "exit")
                    break;
                else if(products.ContainsKey(productChoice))
                {
                    double price = products[productChoice].Price;
                    Console.WriteLine($"You have ordered {productChoice} for {price}");
                    total += price;
                    orders.Add(productChoice);
                    orderLogic.SaveOrder(reservationId, products[productChoice].productId);
                }
                else
                {
                    Console.WriteLine("This item is not on the menu");
                }
            }

            Console.WriteLine($"Your reservation ID is {reservationId}");
            foreach(string order in orders)
            {
                Console.WriteLine(order);
            }
            Console.WriteLine($"Your total is {total}");

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