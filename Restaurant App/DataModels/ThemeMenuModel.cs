// creates theme menu models for the database

public class ThemeMenuModel
{
    private static long _nextId = 1;
    public long MenuId { get; private set; }
    public string ThemeName { get; set; }
    public int ScheduledYear { get; set; } = 0;  // temporary property for display purposes
    public int ScheduledMonth { get; set; } = 0;  // temporary property for display purposes

    public ThemeMenuModel() { }

    public ThemeMenuModel(string themeName)
    {
        MenuId = _nextId;
        _nextId++;
        ThemeName = themeName;
    }
}