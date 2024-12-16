using Project;

static class ProductView
{
    public static void ProductMenu()
    {
        string banner = $"Choose:\n\n";
            List<string> options = new()
        {
            "Show all products",
            "Choose products theme",
            "Choose products in month\n",
            "back"
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Show all products":
                    DisplayAllProducts();
                    break;
                case "Choose products theme":
                    
                    break;
                case "Choose products in month":
                    
                    break;
                case "back":
                    return;
            }
        }
    }
    // Display all products
    public static void DisplayAllProducts()
    {
        IEnumerable<ProductModel> products = ProductManager.GetAllProducts();
        if (products.Count() > 0)
        {
            foreach (var product in products)
            {
                Console.WriteLine($"- Product: {product.Name}");
            }
        }
        else
        {
            Console.WriteLine("No products found.");
        }
    }
}