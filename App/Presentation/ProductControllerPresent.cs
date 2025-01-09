namespace Restaurant;

public static class ProductControllerPresent
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

    //public static void UpdateProductQuantity(ProductModel product, int newQuantity)
    //{
    //    if (ProductManager.UpdateProductQuantity(product, newQuantity) == true)
    //    {
    //        Console.WriteLine($"Updated '{product.Name} (ID: {product.ID})");
    //    }
    //    else
    //    {
    //        Console.WriteLine($"Failed to update '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
    //    }
    //}

    public static void DeleteProduct(int productId)
    {
        if (ProductLogic.DeleteProduct(productId))
        {
            Console.WriteLine($"Product with ID: {productId} deleted.");
        }
        // else
        // {
        //     Console.WriteLine($"No product found with ID: {productId}, or the product has already been deleted.");
        // }
    } 
}