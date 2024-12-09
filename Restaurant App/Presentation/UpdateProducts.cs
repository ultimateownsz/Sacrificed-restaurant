using Project;

public static class UpdateProducts
{
    public static void AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        if (ProductLogic.AddProduct(product) == false)
        {
            Console.WriteLine($"Product: {product.Name}, with ID: {product.ID} already exists.");
        }
        else
        {
            Console.WriteLine($"Product: {product.Name}, with ID: {product.ID} added.");
        }
    }

    public static void DeleteProduct(int productId)
    {
        if (ProductLogic.DeleteProduct(productId))
        {
            Console.WriteLine($"Product with ID: {productId} deleted.");
        }
    } 
}