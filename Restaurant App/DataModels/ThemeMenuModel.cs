// creates theme menu models for the database

public class ThemeMenuModel
{
    public long MenuId { get; set; } = 0;
    public string Theme { get; set; } = "Default Theme";
    public List<string> themes { get; set; } = new List<string> { };

    public ThemeMenuModel() {}

    public ThemeMenuModel(long menuId, string themeName)
    {
        MenuId = menuId;
        Theme = themeName;
    }

    // public List<ThemeMenuModel> themes = new List<ThemeMenuModel> { };
}