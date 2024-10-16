public class MenuLogic
{
    public int MenuId { get; set; }
    public ThemeLogic Theme { get; set; }

    public MenuLogic(int menuId, ThemeLogic theme)
    {
        MenuId = menuId;
        Theme = theme;
    }
}