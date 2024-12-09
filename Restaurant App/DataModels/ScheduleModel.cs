namespace Project;
public class ScheduleModel : IModel
{
    public int? ID { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? ThemeID { get; set; }
    // public DateTime? DateTime
    // {
    //     get
    //     {
    //         if (Year == null || Month == null)
    //             return null;

    //         return new DateTime(year:Year ?? 0, month:Month ?? 0, day:1);
    //     }
    // }

    public ScheduleModel() { }
    public ScheduleModel(string? name, int? year, int? month, int? themeID, int? id = null)
    {
        ID = id;
        Year = year;
        Month = month;
        ThemeID = themeID;
    }

}
