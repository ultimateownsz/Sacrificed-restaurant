public static class ProductController
{
    public static void AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        if (ProductManager.AddProduct(product) == false)
        {
            Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} already exists.");
        }
        else
        {
            Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} added.");
        }
    }

    public static void UpdateProductQuantity(ProductModel product, int newQuantity)
    {
        if (ProductManager.UpdateProductQuantity(product, newQuantity) == true)
        {
            Console.WriteLine($"Updated '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
        }
        else
        {
            Console.WriteLine($"Failed to update '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
        }
    }

    public static void DeleteProduct(long productId)
    {
        if (ProductManager.DeleteProduct(productId))
        {
            Console.WriteLine($"Product with ID: {productId} deleted.");
        }
        // else
        // {
        //     Console.WriteLine($"No product found with ID: {productId}, or the product has already been deleted.");
        // }
    }
}