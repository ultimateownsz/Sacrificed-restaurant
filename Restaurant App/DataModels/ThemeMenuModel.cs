// creates theme menu models for the database

public class ThemeMenuModel
{
    private static long _nextId = 1;
    public long MenuId { get; set; }
    public string ThemeName { get; internal set; }
    public int ScheduledYear { get; set; }  // temporary property for display purposes
    public int ScheduledMonth { get; set; }  // temporary property for display purposes

    public ThemeMenuModel()
    {
        SetMenuId();
        ThemeName = "Default theme";
    }

    public ThemeMenuModel(string themeName)
    {
        SetMenuId();
        ThemeName = themeName;
    }

    private void SetMenuId() => MenuId = _nextId++;
}