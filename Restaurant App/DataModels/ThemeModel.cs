namespace Project;
public class ThemeModel: IModel
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public int? Month { get; set; }

    public ThemeModel() { }
    public ThemeModel(string? name, int? month, int? id = null)
    {
        ID = id;
        Name = name;
        Month = month;
    } 
}
