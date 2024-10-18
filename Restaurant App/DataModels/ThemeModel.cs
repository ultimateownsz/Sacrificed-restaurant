public class ThemeModel
{
    public bool IsThemeActive { get; set; }
    public string CurrentTheme { get; set; } = "Default Theme";
    public List<string> ThemeCategories { get; set; } = new();
    public ThemeModel()
    {
        IsThemeActive = false;
    }

    public void SetTheme(string themeName, List<ProductCategory> categories)
    {
        CurrentTheme = themeName ?? "Default Theme";  // fallback to default if themeName is null
        IsThemeActive = true;
        ThemeCategories.Clear();

        if (categories != null)
        {
            // convert each category's CategoryType value to a string, then add it to ThemeCategories
            foreach (var category in categories)
            {
                ThemeCategories.Add(category.Category);
            }
        }
    }

    public void ClearTheme()
    {
        CurrentTheme = "Default Theme";
        IsThemeActive = false;
        ThemeCategories.Clear();
    }
}