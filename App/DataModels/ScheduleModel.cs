namespace Restaurant;
public class ScheduleModel : IModel
{
    public int? ID { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? ThemeID { get; set; }

    public ScheduleModel() { }
    public ScheduleModel(string? name, int? year, int? month, int? themeID, int? id = null)
    {
        ID = id;
        Year = year;
        Month = month;
        ThemeID = themeID;
    }

}
