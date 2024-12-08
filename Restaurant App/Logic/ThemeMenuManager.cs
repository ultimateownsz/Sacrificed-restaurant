// This class handles all the logic for the themes, adding, deleting, and eventually activating themes

using Project;

static class ThemeMenuManager
{

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


    // this method is used to delete themes that are attached to a schedule
    public static bool DeleteMonthTheme(int month, int year)
    {
        var item = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();
        if (item == null) return false;
        return Access.Schedules.Delete(item.ID);
    }
}
