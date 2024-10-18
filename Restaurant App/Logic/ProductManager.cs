static class ProductManager
{
    public static bool AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (ProductsAccess.GetById(product.ProductId) != null)
        {
            // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} already exists.");
            return false;
        }
        ProductsAccess.Write(product);
        // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} added successfully.");

        return true;
    }

    public static bool UpdateProductQuantity(ProductModel product, int newQuantity)
    {
        product.UpdateQuantity(newQuantity);
        ProductsAccess.Update(product);
        return true;
        // Console.WriteLine($"Updated '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
    }

    public static bool DeleteProduct(long productId)
    {
        if (productId <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(productId)} Product ID must be greater than 0.");
            // return ;
        }

        if (ProductsAccess.GetById(productId) == null)
        {
            // Console.WriteLine($"Database does not contain a product with ID: {productId}.");
            return false;
        }
        ProductsAccess.Delete(productId);
        // Console.WriteLine($"Product with ID: {productId} deleted successfully.");
        return true;
    }
}