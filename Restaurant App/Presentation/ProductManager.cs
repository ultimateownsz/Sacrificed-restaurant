static class ProductManager
{
    // static private ProductModel _productModel = new ProductModel();
    public static void AddProduct(ProductModel product)
    {
        // if (product == null)
        // {
        //     throw new ArgumentNullException(nameof(product));
        // }

        if (ProductsAccess.GetById(product.ProductId) != null)
        {
            Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} already exists.");
            return;
        }
        ProductsAccess.Write(product);
        Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} added successfully.");
    }

    public static void UpdateProductQuantity(ProductModel product, int newQuantity)
    {
        product.UpdateQuantity(newQuantity);
        ProductsAccess.Update(product);
        Console.WriteLine($"Updated '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
    }

    public static void DeleteProduct(long productId)
    {
        if (productId <= 0)
        {
            Console.WriteLine(nameof(productId), "Product ID must be greater than 0.");
            return;
        }

        if (ProductsAccess.GetById(productId) == null)
        {
            Console.WriteLine($"No product found with ID {productId}");
            return;
        }
        ProductsAccess.Delete(productId);
        Console.WriteLine($"Product with ID {productId} deleted successfully.");
    }
}