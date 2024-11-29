using Project;

public class ReservationMenuLogic
{

    //Static properties are shared across all instances of the class
    public static ThemeModel? CurrentTheme { get; private set; }

    public ReservationMenuLogic()
    {
        // Could do something here

    }

    public string GetCurrentMenu()
    {
        CurrentTheme = Access.Themes.GetBy<int>("ID", 1);
        if(CurrentTheme is not null)
            return CurrentTheme.Name;
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
    