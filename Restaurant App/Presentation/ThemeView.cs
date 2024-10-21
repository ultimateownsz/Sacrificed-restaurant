static class ThemeView
{
    public static void AddTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }
        if (ThemeMenuManager.AddTheme(theme) == true)
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} added.");
        }
        else
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} already exists.");
        }
        
    }


    public static void DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemeMenuManager.DeleteTheme(theme) )
        {
            Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} deleted.");
        }
        // else
        // {
        //     Console.WriteLine($"Theme: {theme.Theme}, with ID: {theme.MenuId} does not exist, or is already deleted.");
        // }
    }

    public static void DisplayAllThemes()
    {
        List<ThemeMenuModel> themes = ThemeMenuManager.GetAllThemes();
        if (themes.Count > 0)
        {
            foreach (var theme in themes)
            {
                Console.WriteLine($"- ID: {theme.MenuId}, theme: {theme.Theme}");
            }
        }
        else
        {
            Console.WriteLine("No themes found.");
        }
    }
}