namespace Restaurant;
public class ThemeModel: IModel
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public int? __deprecated_attr_month { get; set; }

    public ThemeModel() { }
    public ThemeModel(string? name, int? id = null)
    {
        ID = id;
        Name = name;
    } 
}