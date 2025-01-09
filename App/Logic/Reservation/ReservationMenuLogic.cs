using App.Logic.Theme;
using Restaurant;

namespace App.Logic.Reservation;

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
        ThemeModel? theme = ThemeManageLogic.GetThemeByYearAndMonth(month, year);
        if (theme == null) return null;
        CurrentTheme = Access.Themes.GetBy("ID", theme.ID);
        if (CurrentTheme is not null)
            return CurrentTheme;
        else
            return null;
    }

    public IEnumerable<ProductModel> GetProductsInMenu()
    {
        if (CurrentTheme != null)
        {
            IEnumerable<ProductModel> products = Access.Products.GetAllBy("ID", CurrentTheme.ID);
            return products;
        }
        else
            return null;
    }

}
