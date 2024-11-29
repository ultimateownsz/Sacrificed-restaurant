using Project;

static class ProductView
{
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