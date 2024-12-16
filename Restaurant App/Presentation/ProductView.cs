using Project;

static class ProductView
{
    public static void ProductMenu()
    {
        string banner = $"PRODUCT MENU\n\n";
            List<string> options = new()
        {
            "Add product",
            "Show all products",
            "Choose products course",
            "Choose products theme",
            "Choose products in month\n",
            "back"
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Add product":
                    
                    break;
                case "Show all products":
                    DisplayAllProducts();
                    break;
                case "Choose products theme":
                    
                    break;
                case "Choose products in month":
                    
                    break;
                case "Choose products course":
                    
                    break;
                case "back" or "":
                    return;
            }
        }
    }
    // Display all products
    public static void DisplayAllProducts()
    {
        while(true)
        {
            string banner = "Choose a product to edit/delete:\n\n";
            List<string> products = ProductManager.GetAllProductInfo().ToList();
            string selection = SelectionPresent.Show(products, banner).text;
            Console.WriteLine(selection);
            Console.ReadKey();
        }
    }
}