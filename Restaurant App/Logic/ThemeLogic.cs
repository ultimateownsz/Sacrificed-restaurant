public class ThemeLogic
{
    public bool IsThemeActive { get; set; }
    public string CurrentTheme { get; set; }
    public List<ProductCategoryType> ThemeCategories { get; set; } = new();
    public ThemeLogic()
    {
        IsThemeActive = false;
        CurrentTheme = string.Empty;
    }

    public void SetTheme(string themeName, List<ProductCategoryType> categories = null)
    {
        CurrentTheme = themeName;
        IsThemeActive = true;
        ThemeCategories = categories ?? new List<ProductCategoryType>();
    }

    public void ClearTheme()
    {
        CurrentTheme = string.Empty;
        IsThemeActive = false;
        ThemeCategories.Clear();
    }

    public List<ProductModel> GetProductsByCurrentTheme(List<ProductModel> products)
    {
        if (!IsThemeActive)
        {
            return products;
        }
        else
        {
            return GetProductsByThemeAndCategory(products, CurrentTheme);
        }
    }

    public List<ProductModel> GetProductsByThemeAndCategory(List<ProductModel> products, string theme)
    {
    //     return products
    //         .Where(product => 
    //             product.CustomAttributes.ContainsKey("Theme") &&
    //             product.CustomAttributes["Theme"].ToString() == theme &&
    //             ThemeCategories.Contains(product.CategoryType))
    //         .ToList();
            return products.Where(product =>
                product.CategoryType.Equals(ThemeCategories) && // Match the category type
                product.CustomAttributes.ContainsKey("Theme") &&
                product.CustomAttributes["Theme"].Equals(CurrentTheme) // Match the theme
                ).ToList();
    }

}