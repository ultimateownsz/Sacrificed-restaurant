using App.DataAccess;
using App.DataAccess.Allergy;
using App.DataAccess.Product;
using Restaurant;

namespace App.DataAccess.Utils;
public abstract class Access
{
    // this is for centralized and simplified usage 
    public static PairAccess Pairs = new();
    public static UserAccess Users = new();
    public static PlaceAccess Places = new();
    public static ThemeAccess Themes = new();
    public static ProductAccess Products = new();
    public static RequestAccess Requests = new();
    public static AllergyAccess Allergies = new();
    public static ScheduleAccess Schedules = new();
    public static AllerlinkAccess Allerlinks = new();
    public static ReservationAccess Reservations = new();
}
