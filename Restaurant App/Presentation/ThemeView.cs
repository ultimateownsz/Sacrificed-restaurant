static class ThemeView
{
    public static void DisplayActiveTheme(ThemeMenuModel theme)
    {
        Console.WriteLine($"Theme: {theme.ThemeName}");
    }

    public static void DisplayAllThemes(List<ThemeMenuModel> themes)
    {
        foreach (var theme in themes)
        {
            Console.WriteLine($"Menu: {theme.MenuId} | Theme: {theme.ThemeName}");
        }
    }
}