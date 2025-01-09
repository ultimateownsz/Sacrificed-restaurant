using Project;

public static class UpdateProducts
{
    public static void AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }
        
        if (ProductManager.AddProduct(product) == false)
        {
            ControlHelpPresent.DisplayFeedback($"Product: {product.Name}, with ID: {product.ID} already exists.", "bottom", "error");
        }
        else
        {
            ControlHelpPresent.DisplayFeedback($"Product: {product.Name}, with ID: {product.ID} added.", "bottom", "success");
        }
    }

    public static void DeleteProduct(int productId)
    {
        if (ProductManager.DeleteProduct(productId))
        {
            ControlHelpPresent.DisplayFeedback($"Product with ID: {productId} deleted.", "bottom", "success");
        }
    } 
}