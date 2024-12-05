// This class handles all the logic for the themes, adding, deleting, and eventually activating themes

using Project;

static class ThemeMenuManager
{

    // Initialize data access for Schedule
    public static ScheduleAccess scheduleAccess = new ScheduleAccess();
    // public static Dictionary<int, ThemeModel> ThemeSchedule = new();
    
    // public static List<(int month, string themeName)> GetMonthlyDisplay()
    // {
    //     var displayData = new List<(int month, string themeName)>();
    //     int startMonth = DateTime.Now.Month;

    //     for (int i = 0; i < 12; i++)
    //     {
    //         var key = startMonth;
    //         if (!ThemeSchedule.TryGetValue(key, out var theme))
    //         {
    //             theme = new ThemeModel()
    //             {
    //                 Month = startMonth,
    //                 Name = "Not scheduled"
    //             };
    //         }
            
    //         displayData.Add((theme.Month ?? 0, theme.Name)); 

    //         startMonth++;
    //         if (startMonth > 12)
    //         {
    //             startMonth = 1;
    //         }
    //     }
    //     return displayData;
    // }

    // this method updates the theme name
    public static void UpdateThemeSchedule(int month, int year, string themeName)
    {
        ScheduleModel? scheduleItem = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();

        if (scheduleItem == null)
        {
            ThemeModel newTheme = new(themeName, null);
            Access.Themes.Write(newTheme);
            
            ScheduleModel newScheduleItem = new(null, year, month, (int)Access.Themes.GetLatestThemeID());

            if (!Access.Schedules.Write(newScheduleItem))
            {
                Console.WriteLine("Failed to write to database");
                Console.ReadKey();
            }
        }
        else
        {
            ThemeModel? theme = Access.Themes.GetBy<int?>("ID", scheduleItem.ThemeID);
            theme.Name = themeName;
            Access.Themes.Update(theme);
        }
    }

    // this method gets all the Theme model attatched to a schedule
    public static ThemeModel? GetThemeByYearAndMonth(int month, int year)
    {
        
        ScheduleModel? item = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();
        if (item == null)
        {
            return null;
        }
        else
        {
            return Access.Themes.GetBy<int?>("ID", item.ThemeID);
        }
    }

    // this method generates the months names and gets the themes names next to them, its used to make the lost for the selection menu
    public static string GetMonthThemeName(int? month, int year)
    {
        return month switch
        {
            1 =>  $"January   -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            2 =>  $"February  -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            3 =>  $"March     -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            4 =>  $"April     -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            5 =>  $"May       -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            6 =>  $"June      -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            7 =>  $"July      -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            8 =>  $"August    -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            9 =>  $"September -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            10 => $"October   -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            11 => $"November  -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            12 => $"December  -  {GetThemeByYearAndMonth(month ?? 0, year)?.Name ?? "no theme"}",
            _ => "Invalid month"
        };
    }

    // return falsew hether the failure was due to a duplicate theme name.
    // public static bool AddOrUpdateTheme(ThemeModel theme, int scheduledMonth, out bool isDuplicate)
    // {
    //     if (theme == null) throw new ArgumentNullException(nameof(theme));

    //     // check if the theme name already exists
    //     if (ThemeSchedule.Values.Any(t => t.Name.Equals(theme.Name, StringComparison.OrdinalIgnoreCase)))
    //     {
    //         isDuplicate = true;
    //         return false;  // return false due to duplicate theme
    //     }

    //     isDuplicate = false;  // no duplicate found
        
    //     var key = (theme.Month ?? 0);
    //     if (ThemeSchedule.ContainsKey(key))
    //     {
    //         if (!Access.Themes.Update(theme)) return false;
    //     }
    //     else
    //     {
    //         if (!Access.Themes.Write(theme)) return false;
    //     }

    //     ThemeSchedule[key] = theme;
    //     return true;
    // }

    // public static bool IsFutureDate(int year, int month)
    // {
    //     DateTime currentDate = DateTime.Now;
    //     return year > currentDate.Year || (year == currentDate.Year && month >= currentDate.Month);
    // }

    // this method is used to delete themes that are attached to a schedule
    public static bool DeleteMonthTheme(int month, int year)
    {
        var item = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();
        if (item == null) return false;
        return Access.Schedules.Delete(item.ID);
    }
}
