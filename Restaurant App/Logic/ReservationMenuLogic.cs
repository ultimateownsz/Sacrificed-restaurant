using Project;

public class ReservationMenuLogic
{

    //Static properties are shared across all instances of the class
    public static ThemeModel? CurrentTheme { get; private set; }

    public ReservationMenuLogic()
    {
        // Could do something here
    }

    public static ThemeModel? GetCurrentTheme(DateTime selectedDate)
    {
        int month = selectedDate.Month;
        int year = selectedDate.Year;
        ThemeModel? theme = ThemeMenuManager.GetThemeByYearAndMonth(month, year);
        if(theme == null) return null;
        CurrentTheme = Access.Themes.GetBy<int?>("ID", theme.ID);
        if(CurrentTheme is not null)
            return CurrentTheme;
        else
            return null;
    }

    public IEnumerable<ProductModel> GetProductsInMenu()
    {
        if(CurrentTheme != null)
        {
            IEnumerable<ProductModel> products = Access.Products.GetAllBy<int?>("ID", CurrentTheme.ID);
            return products;
        }
        else
            return null;
    }

}
    