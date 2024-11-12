using Microsoft.Data.Sqlite;

using Dapper;

public static class ThemesAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Menu";

    
    public static IEnumerable<ThemeMenuModel> GetAllThemes()
    {
        string sql = $"SELECT menuID, theme AS ThemeName FROM {Table} ORDER BY menuID";
        return _connection.Query<ThemeMenuModel>(sql);
    }

    public static int AddTheme(ThemeMenuModel theme)
    {
        string sql = $"INSERT INTO {Table} (menuID, theme) VALUES (@MenuId, @ThemeName); SELECT last_insert_rowid()";
        int newId = _connection.ExecuteScalar<int>(sql, new
        {
            theme.MenuId,
            theme.ThemeName
        });

        return newId;
    }
    public static bool UpdateTheme(ThemeMenuModel theme)
    {
        string sql = $@"
            UPDATE {Table}
            SET theme = @ThemeName
            WHERE menuID = @MenuId";
        int rowsAffected = _connection.Execute(sql, new 
        {
            theme.ThemeName,
            theme.MenuId
        });

        return rowsAffected > 0;
    }

    public static void DeleteTheme(long menuID)
    {
        string sql = $"DELETE FROM {Table} WHERE menuID = @MenuId";
        _connection.Execute(sql, new { MenuId = menuID });
    }

    public static void DeleteTheme(ThemeMenuModel themeName)
    {
        string sql = $"DELETE FROM {Table} WHERE theme = @ThemeName";
        _connection.Execute(sql, new { ThemeName = themeName });
    }

    public static ThemeMenuModel GetById(long menuID)
    {
        string sql = $"SELECT * FROM {Table} WHERE menuID = @MenuId";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { MenuId = menuID });
    }

    public static ThemeMenuModel GetById(string themeName)
    {
        string sql = $"SELECT * FROM {Table} WHERE theme = @ThemeName";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { Theme = themeName });
    }

    public static ThemeMenuModel GetByName(string themeName)
    {
        string sql = $"SELECT * FROM {Table} WHERE theme = @ThemeName";
        return _connection.QueryFirstOrDefault<ThemeMenuModel>(sql, new { Theme = themeName });
    }

    public static IEnumerable<ThemeMenuModel> GetByThemeName(string themeName)
    {
        string sql = $"Select * from {Table} where theme = @ThemeName";
        return _connection.Query<ThemeMenuModel>(sql, new { ThemeName = themeName });
    }
}