public class ProductModel : IProduct
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public long Quantity { get; set; }
    public decimal Price { get; set; }
    public long MenuID { get; set; }
    public string Category { get; set; }

    // public string CategoryTypeString => Category.GetCategoryType();
    
    // a dict for storing custom attributes specific to each product
    // - "Theme": string, "CategoryType": ProductCategoryType
    // - example: {"Theme": "Italian", "CategoryType": ProductCategoryType.MainDishes}
    // don't forget to cast the value to the correct type when using it
    // - example: (string)product.CustomAttributes["Theme"]
    public Dictionary<string, object> CustomAttributes { get; set; } = new();

    public ProductModel() {}

    public ProductModel(long productID, string productName, long quantity, decimal price, long menuID, ProductCategory category)
    {
        ProductId = productID;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        MenuID = menuID;
        Category = category.Category;
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