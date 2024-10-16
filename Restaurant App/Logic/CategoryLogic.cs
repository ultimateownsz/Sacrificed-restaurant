public class CategoryLogic
{
    private readonly List<ProductCategory> _categories;

    public CategoryLogic(List<ProductCategory> categories)
    {
        _categories = categories;
    }

    public List<ProductCategory> FilterByCategory(ProductCategoryType categoryType)
    {
        return _categories.Where(category => category.CategoryType == categoryType).ToList();
    }
}