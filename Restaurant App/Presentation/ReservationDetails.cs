namespace Project;
public static class ReservationDetails
{
    public static void ShowDetails(ReservationModel reservation)
    {
        Console.Clear();
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        _ = ControlHelpPresent.GetFooterHeight(); // Dynamically determine footer height
        int reservationStartLine = 0;

        Console.SetCursorPosition(0, reservationStartLine);

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

        ControlHelpPresent.DisplayFeedback("\nPress any key to return to the reservations list.");
        Console.ReadKey();
    }

    public static void ShowOrders(UserModel acc)
    {
        lCalendar:
        int guests = 1;
        bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
        DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);
        if (selectedDate == DateTime.MinValue)
            return;

        while (true)
        {
            IEnumerable<ReservationModel?> orders = Access.Reservations.GetAllBy<DateTime>("Date", selectedDate);

            if (!orders.Any(r => r.Date.HasValue && r.Date.Value == selectedDate))
            {
                Console.Clear();
                ControlHelpPresent.DisplayFeedback($"There are no orders for the date {selectedDate:dd/MM/yyyy}.");

                ControlHelpPresent.Clear();
                ControlHelpPresent.AddOptions("Previous date", "<p>");
                ControlHelpPresent.AddOptions("Next date", "<n>");
                ControlHelpPresent.AddOptions("Exit", "<escape>");
                ControlHelpPresent.ShowHelp();
                // Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
                ConsoleKeyInfo emptyOrders = Console.ReadKey();
                switch (emptyOrders.Key)
                {
                    case ConsoleKey.Escape:
                        goto lCalendar;
                    case ConsoleKey.P:
                        selectedDate = selectedDate.AddDays(-1);
                        continue;
                    case ConsoleKey.N:
                        selectedDate = selectedDate.AddDays(1);
                        continue;
                }
            }
            else
            {
                DisplayOrdersGrid(orders, selectedDate);

                // Console.WriteLine("\n(B)ack - (P)revious date - (N)ext date");
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        goto lCalendar;
                    case ConsoleKey.P:
                        selectedDate = selectedDate.AddDays(-1);
                        break;
                    case ConsoleKey.N:
                        selectedDate = selectedDate.AddDays(1);
                        break;
                }
            }
        }
    }

    private static void DisplayOrdersGrid(IEnumerable<ReservationModel?> orders, DateTime selectedDate)
    {
        // Mapping Course values to Categories
        Dictionary<string, string> productsCategories = new Dictionary<string, string>
        {
            { "Appetizer", "Appetizer" },
            { "Main", "Main" },
            { "Dessert", "Dessert" },
            { "Beverage", "Beverage" }
        };

        // Dictionary to store count of each product by category
        Dictionary<string, Dictionary<string, int>> categoriesCount = new Dictionary<string, Dictionary<string, int>>
        {
            { "Appetizer", new Dictionary<string, int>() },
            { "Main", new Dictionary<string, int>() },
            { "Dessert", new Dictionary<string, int>() },
            { "Beverage", new Dictionary<string, int>() }
        };

        // Iterate through each reservation for the selected date
        foreach (var reserv in orders.Where(r => r != null))
        {
            // Get all requests associated with the current reservation ID
            var requests = Access.Requests.GetAllBy<int?>("ReservationID", reserv!.ID).ToList();

            // Process each request to get the associated product and categorize it
            foreach (var req in requests)
            {
                // Fetch the product details using the ProductID from the Request table
                var product = Access.Products.GetBy<int?>("ID", req.ProductID);
                if (product != null && productsCategories.TryGetValue(product.Course, out string category))
                {
                    // Increment the count of the product in the appropriate category
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

        decimal grandTotalPrice = 0;

        Console.Clear();
        Console.WriteLine($"Orders for {selectedDate:dd/MM/yyyy}\n");

        string[] headers = { "Appetizers", "Main", "Dessert", "Beverage", "Total Price" };
        Console.WriteLine("{0,-30}{1,-30}{2,-30}{3,-30}{4,-30}", headers[0], headers[1], headers[2], headers[3], headers[4]);

        int maxRows = Math.Max(
            Math.Max(categoriesCount["Appetizer"].Count, categoriesCount["Main"].Count),
            Math.Max(categoriesCount["Dessert"].Count, categoriesCount["Beverage"].Count)
        );

        var appetizers = categoriesCount["Appetizer"].ToList();
        var mains = categoriesCount["Main"].ToList();
        var desserts = categoriesCount["Dessert"].ToList();
        var beverages = categoriesCount["Beverage"].ToList();

        for (int i = 0; i < maxRows; i++)
        {
            string appetizer = "";
            string main = "";
            string dessert = "";
            string beverage = "";
            decimal totalPrice = 0;

            if (i < appetizers.Count)
            {
                ProductModel? product = Access.Products.GetBy<string>("Name", appetizers[i].Key);
                appetizer = $"{appetizers[i].Value}x {appetizers[i].Key}";
                totalPrice += appetizers[i].Value * (product?.Price ?? 0);
            }

            if (i < mains.Count)
            {
                ProductModel? product = Access.Products.GetBy<string>("Name", mains[i].Key);
                main = $"{mains[i].Value}x {mains[i].Key}";
                totalPrice += mains[i].Value * (product?.Price ?? 0);
            }

            if (i < desserts.Count)
            {
                ProductModel? product = Access.Products.GetBy<string>("Name", desserts[i].Key);
                dessert = $"{desserts[i].Value}x {desserts[i].Key}";
                totalPrice += desserts[i].Value * (product?.Price ?? 0);
            }

            if (i < beverages.Count)
            {
                ProductModel? product = Access.Products.GetBy<string>("Name", beverages[i].Key);
                beverage = $"{beverages[i].Value}x {beverages[i].Key}";
                totalPrice += beverages[i].Value * (product?.Price ?? 0);
            }

            grandTotalPrice += totalPrice;

            string gridRow = $"{appetizer,-30}{main,-30}{dessert,-30}{beverage,-30}{totalPrice,-30:C}\n";
            Console.WriteLine(gridRow);
        }

        string grandTotalRow = $"{grandTotalPrice:C} Grand Total";
        Console.WriteLine(grandTotalRow.PadLeft(120));
    }

}