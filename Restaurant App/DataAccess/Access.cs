namespace Project;
public abstract class Access
{
    // this is for centralized and simplified usage
    public static UserAccess Users = new();
    public static ProductAccess Products = new();
    public static RequestAccess Requests = new();
    public static ReservationAccess Reservations = new();
    public static ThemeAccess Themes = new();
}
