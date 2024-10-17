static class ProductView
{
    public static void DisplayAllProducts()
    {
        var products = ProductsAccess.GetAll();

        Console.WriteLine("Available products:");
        foreach (var product in products)
        {
            Console.WriteLine($"- Product: {product.ProductName} | Quantity: {product.Quantity}");
        }
    }
}