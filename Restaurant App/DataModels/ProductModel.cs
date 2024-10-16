public class ProductModel : IProduct
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long MenuID { get; set; }
    public ProductCategoryType CategoryType { get; set; }
    public Dictionary<string, object> CustomAttributes { get; set; } = new();

    public ProductModel(long productID, string productName, long quantity, decimal price, long menuID, ProductCategoryType category)
    {
        ProductId = productID;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        MenuID = menuID;
        CategoryType = category;
    }
}