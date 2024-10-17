using Microsoft.Data.Sqlite;

using Dapper;

public static class ThemesAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Menu";

    public static void Write(MenuModel menu)
    {
        string sql = $"INSERT INTO {Table} (menuID, theme) VALUES (@MenuId, @ThemeName)";
        _connection.Execute(sql, menu);
    }

    public static ProductModel GetById(long menuID)
    {
        string sql = $"SELECT * FROM {Table} WHERE menuID = @MenuId";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { MenuId = menuID });
    }

    public static ProductModel GetByName(string themeName)
    {
        string sql = $"SELECT * FROM {Table} WHERE theme = @ProductName";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ThemeName = themeName });
    }

    public static IEnumerable<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ProductModel>(sql);
    }

    public static void Update(MenuModel menu)
    {
        string sql = $"UPDATE {Table} SET theme = @ThemeName WHERE menuID = @MenuId";
        _connection.Execute(sql, menu);
    }

    public static void Delete(long menuID)
    {
        string sql = $"DELETE FROM {Table} WHERE menuID = @MenuId";
        _connection.Execute(sql, new { MenuId = menuID });
    }
}