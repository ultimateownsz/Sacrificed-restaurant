using App.DataAccess;
using App.DataAccess.Allergy;
using App.DataAccess.Product;
using Restaurant;

namespace App.DataAccess.Utils;
internal abstract class Access
{
    // this is for centralized and simplified usage 
    internal static PairAccess Pairs = new();
    internal static UserAccess Users = new();
    internal static PlaceAccess Places = new();
    internal static ThemeAccess Themes = new();
    internal static ProductAccess Products = new();
    internal static RequestAccess Requests = new();
    internal static AllergyAccess Allergies = new();
    internal static ScheduleAccess Schedules = new();
    internal static AllerlinkAccess Allerlinks = new();
    internal static ReservationAccess Reservations = new();
}
