static class ProductView
{
    // Display all products
    public static void DisplayAllProducts()
    {
        List<ProductModel> products = ProductManager.GetAllProducts();
        if (products.Count > 0)
        {
            foreach (var product in products)
            {
                Console.WriteLine($"- Product: {product.ProductName}, quantity: {product.Quantity}");
            }
        }
        else
        {
            Console.WriteLine("No products found.");
        }
    }
}