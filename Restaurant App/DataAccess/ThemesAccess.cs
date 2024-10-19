using Microsoft.Data.Sqlite;

using Dapper;

public static class ThemesAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Menu";

    public static void Write(ThemeMenuModel menu)
    {
        _connection.Open();
        string sql = $"INSERT INTO {Table} (menuID, theme) VALUES (@MenuId, @Theme)";
        _connection.Execute(sql, new
        {
            menu.MenuId,
            menu.Theme
        });

        _connection.Close();
    }

    public static ThemeMenuModel GetById(long menuID)
    {
        string sql = $"SELECT * FROM {Table} WHERE menuID = @MenuId";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { MenuId = menuID });
    }

    public static ThemeMenuModel GetById(string themeName)
    {
        string sql = $"SELECT * FROM {Table} WHERE theme = @Theme";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { Theme = themeName });
    }

    public static ThemeMenuModel GetByName(string themeName)
    {
        string sql = $"SELECT * FROM {Table} WHERE theme = @ProductName";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { Theme = themeName });
    }

    public static IEnumerable<ThemeMenuModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ThemeMenuModel>(sql);
    }

    public static void Update(ThemeMenuModel menu)
    {
        string sql = $"UPDATE {Table} SET theme = @Theme WHERE menuID = @MenuId";
        _connection.Execute(sql, new 
        {
            menu.MenuId,
            menu.Theme
        });
    }

    public static void Delete(long menuID)
    {
        string sql = $"DELETE FROM {Table} WHERE menuID = @MenuId";
        var result = _connection.Execute(sql, new { MenuId = menuID });

        // if (result == 0)
        // {
        //     return;
        // }
    }

    public static void Delete(ThemeMenuModel themeName)
    {
        string sql = $"DELETE FROM {Table} WHERE theme = @Theme";
        _connection.Execute(sql, new { Theme = themeName });
    }

    public static IEnumerable<ThemeMenuModel> GetByThemeName(string themeName)
    {
        string sql = $"Select * from {Table} where theme = @Theme";
        return _connection.Query<ThemeMenuModel>(sql, new { Theme = themeName });
    }
}