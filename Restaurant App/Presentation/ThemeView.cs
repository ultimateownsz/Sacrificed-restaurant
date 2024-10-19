static class ThemeView
{
    
    public static void AddTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemeMenuManager.AddTheme(theme) == false)
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} already exists.");
        }
        else
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} added.");
        }
    }

    public static void DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemeMenuManager.DeleteTheme(theme) == false)
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} does not exist.");
        }
        else
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} deleted.");
        }
    }
    
    // public static void DisplayActiveTheme(ThemeMenuModel theme)
    // {
    //     if (theme == null)
    //     {
    //         throw new ArgumentNullException(nameof(theme));
    //     }

    //     if (ThemeMenuManager.ActiveTheme(theme) == false)
    //     {
    //         Console.WriteLine($"\nTheme: {theme.Theme}, with ID: {theme.MenuId} does not exist.");
    //     }
    //     else
    //     {
    //         Console.WriteLine($"\nTheme: {theme.Theme}, with ID: {theme.MenuId}.");
    //     }
    // }

    // TODO Add method to display all themes (not yet working)
    public static void DisplayAllThemes()
    {
        List<ThemeMenuModel> themes = ThemeMenuManager.GetAllThemes();
        if (themes.Count > 0)
        {
            foreach (var theme in themes)
            {
                Console.WriteLine($"Menu: {theme.MenuId}, theme: {theme.Theme}");
            }
        }
        else
        {
            Console.WriteLine("No themes found.");
        }
    }
}