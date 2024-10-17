public class ProductCategory : IProduct
{
    public string Theme { get; set; } = "Default Theme";
    public string ProductName { get; set; } = "Default";
    public Dictionary<string, object> CustomAttributes { get; set; } = new ();
    // public Dictionary<string, string> CategoryAttributes { get; set; } = new();
    public string Category { get; set; } = "Defaulkt";

    public ProductCategory(string category, string theme, string name)
    {
        Category = category;
        Theme = theme;
        ProductName = name;
        if (!isValidCategory(category))
        {
            return;
        }
    }

    public bool isValidCategory(string category)
    {
        return category == ProductCategoryType.MainDishes ||
               category == ProductCategoryType.SideDishes ||
               category == ProductCategoryType.Desserts ||
               category == ProductCategoryType.AlcoholicBeverages ||
               category == ProductCategoryType.NonAlcoholicBeverages;
    }

    public bool BelongsToActiveTheme(ThemeLogic themeLogic)
    {
        bool IsThemeActive = themeLogic.IsThemeActive;

        bool themeMatches = themeLogic.CurrentTheme == Theme;

        // bool categoryIsInTheme = themeLogic.ThemeCategories

        return IsThemeActive && themeMatches;
        
        
    }
 
}