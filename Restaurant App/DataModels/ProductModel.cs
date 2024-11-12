public class ProductModel : IProduct
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long MenuID { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Type { get; set; } = " ";

    // you must need an empty constructor because of in the ProductsAccess write/update method you create temporary variable placeholders
    public ProductModel() {}

    public ProductModel(long productID, string productName, long quantity, decimal price, long menuID, ProductCategory category, string type)
    {
        ProductId = productID;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        MenuID = menuID;
        Category = category.Category;
        Type = type;
    }

    public void UpdateQuantity(long newQuantity)
    {
        Quantity = newQuantity;
    }

    public void UpdateQuantity(long adjustment, bool isIncrement)
    {
        if (isIncrement)
        {
            Quantity += adjustment;
        }
        else
        {
            Quantity -= adjustment;
        }
    }
}