// this class handles all the logic for adding, updating, and deleting products

using Project;

static class ProductLogic
{
    public static bool AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (Access.Products.GetBy<int?>("ID", product.ID) != null)
        {
            return false;
        }
        Access.Products.Write(product);

        return true;
    }

    //public static bool UpdateProductQuantity(ProductModel product, int newQuantity)
    //{

    //    //product.UpdateQuantity(newQuantity);
    //    ProductsAccess.Update(product);
    //    return true;
    //    // Console.WriteLine($"Updated '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
    //}

    public static bool DoesProductExist(int? productId)
    {
        if (productId == null) return false;
        return Access.Products.GetBy<int?>("ID", productId.Value) != null;
    }


    public static bool DeleteProduct(int productId)
    {
        if (productId < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(productId)} Product ID must be greater than 0.");
        }

        if (Access.Products.GetBy<int>("ID", productId) == null)
        {
            return false;
        }
        Access.Products.Delete(productId);
        return true;
    }

     public static IEnumerable<ProductModel> GetAllProducts()
    {
        return Access.Products.Read();
    }

     public static IEnumerable<ProductModel> GetAllWithinCategory(string category)
    {
        return Access.Products.GetAllBy<string>("Course", category);
    }

}