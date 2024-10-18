public class ThemeMenuModel
{
    public long MenuId { get; set; }
    public string ThemeName { get; set; }

    public ThemeMenuModel(long menuId, string themeName)
    {
        MenuId = menuId;
        ThemeName = themeName;
    }
}