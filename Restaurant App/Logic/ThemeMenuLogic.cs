// This class handles all the logic for the themes, adding, deleting, and eventually activating themes

using Project;

static class ThemeMenuManager
{

    // this method updates the theme name
    public static void UpdateThemeSchedule(int month, int year, string themeName)
    {
        ScheduleModel? scheduleItem = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();
        ThemeModel? themeItem = Access.Themes.GetAllBy<string>("Name", themeName).FirstOrDefault();
        ScheduleModel newScheduleItem;

        if (scheduleItem == null)
        {
            if(themeItem == null)
            {
                ThemeModel newTheme = new(themeName, null);
                Access.Themes.Write(newTheme);  
                newScheduleItem = new(null, year, month, (int)Access.Themes.GetLatestThemeID());
            }
            else
            {
                newScheduleItem = new(null, year, month, themeItem.ID);
            }

            if (!Access.Schedules.Write(newScheduleItem))
            {
                Console.WriteLine("Failed to write to database");
                Console.ReadKey();
            }
        }
        else
        {
            if(themeItem == null)
            {
                ThemeModel newTheme = new(themeName, null);
                Access.Themes.Write(newTheme);
                UpdateThemeSchedule(month, year, themeName);
            }
            else
            {
                scheduleItem.ThemeID = themeItem.ID;
                Access.Schedules.Update(scheduleItem);
            }
        }
    }

    
    public static List<string?> GetAllThemes()
    {
        var themes = Access.Themes.GetAll().Select(t => t.Name).ToList();
        return themes;
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

    // this method generates the months names
    public static string GetMonthName(int? month)
    {
        return month switch
        {
            1 =>  "January",
            2 =>  "February",
            3 =>  "March",
            4 =>  "April",
            5 =>  "May",
            6 =>  "June",
            7 =>  "July",
            8 =>  "August",
            9 =>  "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => "Invalid month"
        };
    }

    // this method is used to delete themes that are attached to a schedule
    // This method retrieves all available years from the database that have associated schedules.
    public static List<int> GetAvailableYears()
    {
        // Fetch distinct years from the schedules table, excluding null values
        return Access.Schedules.Read()
            .Select(schedule => schedule.Year)
            .Where(year => year.HasValue) // Exclude null values
            .Select(year => year.Value) // Convert int? to int
            .Distinct()
            .OrderBy(year => year) // Ensure years are in ascending order
            .ToList(); // Convert to a List<int> for the final return
    }


    public static bool DeleteMonthTheme(int month, int year)
    {
        var item = Access.Schedules.GetAllBy<int>("Year", year).Where(s => s.Month == month).FirstOrDefault();
        if (item == null) return false;
        return Access.Schedules.Delete(item.ID);
    }

    
    public static int? GetThemeIDByName(string name)
    {
        var theme = Access.Themes.GetBy<string>("Name", name?.Substring(0, 1).ToUpper() + name?.Substring(1));
        return theme?.ID;
    }
}