public class ProductCategory : IProduct
{
    public ProductCategoryType CategoryType { get; set; }
    public string Theme { get; set; }
    public string ProductName { get; set; }
    public Dictionary<string, object> CustomAttributes { get; set; } = new ();

    public ProductCategory(ProductCategoryType categoryType, string theme, string name)
    {
        CategoryType = categoryType;
        Theme = theme;
        ProductName = name;
    }
}