using Microsoft.VisualBasic;

namespace Project;
public static class ReservationDetails
{
    public static void ShowDetails(ReservationModel reservation)
    {
        Console.Clear();

        // Fetch the account details for the user
        var account = Access.Reservations.GetBy<int?>("UserID", reservation.UserID);

        // Ensure data exists
        if (account == null)
        {
            Console.WriteLine("Unable to retrieve user details.");
            Console.WriteLine("Press any key to return to the reservations list.");
            Console.ReadKey();
            return;
        }

        // Display the details
        Console.WriteLine("Reservation Details");
        Console.WriteLine("-------------------");
        // Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
        Console.WriteLine();
        Console.WriteLine($"Reservation Date: {reservation.Date.ToString()}");
        Console.WriteLine($"Assigned Table number: {reservation.PlaceID}");
        Console.WriteLine("More details may appear in the future...");

        Console.WriteLine("\nPress any key to return to the reservations list.");
        Console.ReadKey();
    }

    public static void ShowOrders(UserModel acc)
    {
        int guests = 1;
        bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
        DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);

        while (true)
        {
            var orders = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

            if (!orders.Any(r => r.Date.HasValue && r.Date.Value == selectedDate))
            {
                Console.Clear();
                Console.WriteLine("There are no orders for this date.\nPress any key to return...");
                Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
                ConsoleKeyInfo emptyOrders = Console.ReadKey();
                switch (emptyOrders.Key)
                {
                    case ConsoleKey.B:
                        return;
                    case ConsoleKey.P:
                        selectedDate = selectedDate.AddDays(-1);
                        break;
                    case ConsoleKey.N:
                        selectedDate = selectedDate.AddDays(1);
                        break;
                }
            }

            Dictionary<int, string> productsCategories = new Dictionary<int, string>
            {
                { 1, "Main" },
                { 3, "Beverage" },
                { 5, "Appetizer" },
                { 8, "Dessert" }
            };
            // Dictionary<string, int> productCounts = new Dictionary<string, int>();
            // const int maxProductCount = 15;
            
            // string selectionMenu = SelectionPresent.Show(["Appetizer", "Main", "Dessert", "Beverage\n", "Back"], "ORDERS\n\n").text;

            Dictionary<string, Dictionary<string, int>> categoriesCount = new Dictionary<string, Dictionary<string, int>>
            {
                { "Appetizer", new Dictionary<string, int>() },
                { "Main", new Dictionary<string, int>() },
                { "Dessert", new Dictionary<string, int>() },
                { "Beverage", new Dictionary<string, int>() }
            };

            // Console.Clear();
            // Console.WriteLine($"Orders for {selectedDate:dd/MM/yyyy}\n");

            foreach (var reserv in orders)
            {
                var request = Access.Requests.GetAllBy<int?>("ReservationID", reserv.ID).ToList();

                foreach (var req in request)
                {
                    var product = Access.Products.GetBy<int?>("ID", req.ProductID);
                    if (product != null && productsCategories.TryGetValue((int)product.ID, out string category))
                    {
                        // if ("Back" == selectionMenu)
                        // {
                        //     return;
                        // }

                        if (categoriesCount[category].ContainsKey(product.Name))
                        {
                            categoriesCount[category][product.Name]++;
                        }
                        else
                        {
                            categoriesCount[category][product.Name] = 1;
                        }
                    }
                }
            }

            Console.Clear();
            Console.WriteLine($"Orders for {selectedDate:dd/MM/yyyy}\n");

            string[] headers = { "Appetizers", "Main", "Dessert", "Beverage"};
            Console.WriteLine("{0,-30}{1,-30}{2,-30}{3,-30}", headers[0], headers[1], headers[2], headers[3]);

            int maxRows = Math.Max(
                Math.Max(categoriesCount["Appetizer"].Count, categoriesCount["Main"].Count),
                Math.Max(categoriesCount["Dessert"].Count, categoriesCount["Beverage"].Count)
            );

            var appetizers = categoriesCount["Appetizer"].ToList();
            var mains = categoriesCount["Main"].ToList();
            var desserts = categoriesCount["Dessert"].ToList();
            var beverages = categoriesCount["Beverage"].ToList();

            List<string> menuOptions = new List<string>();
            
            for (int i = 0; i < maxRows; i++)
            {
                string appetizer = i < appetizers.Count ? $"{appetizers[i].Value}x {appetizers[i].Key}" : "";
                string main = i < mains.Count ? $"{mains[i].Value}x {mains[i].Key}" : "";
                string dessert = i < desserts.Count ? $"{desserts[i].Value}x {desserts[i].Key}" : "";
                string beverage = i < beverages.Count ? $"{beverages[i].Value}x {beverages[i].Key}" : "";

                string gridRow = $"{appetizer,-30}{main,-30}{dessert,-30}{beverage,-30}\n";
                // menuOptions.Add(gridRow);
                Console.WriteLine(gridRow);
            }

            // menuOptions.Add("Back");

            // string selectionMenu = SelectionPresent.Show(menuOptions, "ORDERS\n\n").text;

            // if (selectionMenu == "Back")
            // {
            //     return;
            // }

            // foreach (var products in productCounts)
            // {
            //     int total = products.Value;
            //     while (total > 0)
            //     {   
            //         int displayCount = Math.Min(total, maxProductCount);
            //         Console.WriteLine($"- {displayCount}x {products.Key}");
            //         total -= displayCount;
            //     }
            // }

            Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.B:
                    return;
                case ConsoleKey.P:
                    selectedDate = selectedDate.AddDays(-1);
                    break;
                case ConsoleKey.N:
                    selectedDate = selectedDate.AddDays(1);
                    break;
            }
        }
    }

    // private static string FormatDate(long date)
    // {
    //     // Format the date from ddMMyyyy (e.g., 12122024) to dd/MM/yyyy
    //     string dateString = date.ToString("D8"); // Ensure it's 8 digits long
    //     return $"{dateString.Substring(0, 2)}/{dateString.Substring(2, 2)}/{dateString.Substring(4)}";
    // }
}
