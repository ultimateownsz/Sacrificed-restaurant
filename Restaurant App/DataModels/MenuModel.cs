public class MenuModel
{
    public long MenuId { get; set; }
    public string ThemeName { get; set; }

    public MenuModel(long menuId, string themeName)
    {
        MenuId = menuId;
        ThemeName = themeName;
    }
}