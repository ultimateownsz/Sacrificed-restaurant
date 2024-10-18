public class ProductCategory : IProduct
{
    public string Theme { get; set; } = "Default Theme";
    public string ProductName { get; set; } = "Default";
    public string Category { get; set; } = "Default";

    public ProductCategory(string category, string theme, string name)
    {
        Category = category;
        Theme = theme;
        ProductName = name;
    }

    public bool isValidCategory(string category)
    {
        return category == ProductCategoryType.MainDishes ||
               category == ProductCategoryType.SideDishes ||
               category == ProductCategoryType.Desserts ||
               category == ProductCategoryType.AlcoholicBeverages ||
               category == ProductCategoryType.NonAlcoholicBeverages;
    }
}