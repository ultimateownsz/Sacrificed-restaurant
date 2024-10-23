public class ReservationMenuLogic
{

    //Static properties are shared across all instances of the class
    public static ThemeMenuModel? CurrentTheme { get; private set; }

    public ReservationMenuLogic()
    {
        // Could do something here

    }

    public string GetCurrentMenu()
    {
        CurrentTheme = ThemesAccess.GetById(1);
        return CurrentTheme.Theme;
    }

    public List<ProductModel> GetProductsInMenu()
    {
        if(CurrentTheme != null)
        {
            List<ProductModel> products = ProductsAccess.GetByIds(new[] { CurrentTheme.MenuId }).ToList();
            return products;
        }
        else
            return null;
    }

}
    