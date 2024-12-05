using Project;

static class ProductDetails
{
    // Display all products
    public static void DisplayAllProducts()
    {
        IEnumerable<ProductModel> products = ProductLogic.GetAllProducts();
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