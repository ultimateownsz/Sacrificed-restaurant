namespace Project;
public abstract class Access
{
    // this is for centralized and simplified usage 
    public static DataAccess<OrderModel> Orders = new(new string?[] { "ID", "ReservationID", "ProductID" });

    public static UserAccess Users = new();
    public static PlaceAccess Places = new();
    public static ThemeAccess Themes = new();
    public static ProductAccess Products = new();
    public static RequestAccess Requests = new();
    public static ScheduleAccess Schedules = new();
    public static ReservationAccess Reservations = new();
}
