public class CategoryLogic
{
    private readonly List<ProductCategory> _categories;

    public CategoryLogic(List<ProductCategory> categories)
    {
        _categories = categories;
    }

    public List<ProductCategory> FilterByCategory(string categoryType)
    {
        return _categories
            .Where(x => x.Category == categoryType)
            .ToList();
    }
}