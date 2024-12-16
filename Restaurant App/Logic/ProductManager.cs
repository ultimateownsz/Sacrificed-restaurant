// this class handles all the logic for adding, updating, and deleting products

using Project;

static class ProductManager
{
    public static bool AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (Access.Products.GetBy<int?>("ID", product.ID) != null)
        {
            // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} already exists.");
            return false;
        }
        Access.Products.Write(product);
        // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} added successfully.");

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
            // return ;
        }

        if (Access.Products.GetBy<int>("ID", productId) == null)
        {
            // Console.WriteLine($"Database does not contain a product with ID: {productId}.");
            return false;
        }
        Access.Products.Delete(productId);
        // Console.WriteLine($"Product with ID: {productId} deleted successfully.");
        return true;
    }

     public static List<ProductModel> GetAllProducts()
    {
        return Access.Products.Read().ToList();
    }

     public static List<string> GetAllProductInfo()
    {
        return Access.Products.Read()
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name
                    : "No theme";
                return $"{p.Name} - {p.Price}€ - {p.Course} - {themeName}";
            })
            .ToList();
    }


     public static IEnumerable<ProductModel> GetAllWithinCategory(string category)
    {
        return Access.Products.GetAllBy<string>("Course", category);
    }

}