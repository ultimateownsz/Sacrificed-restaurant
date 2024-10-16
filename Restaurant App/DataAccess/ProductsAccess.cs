using Microsoft.Data.Sqlite;

using Dapper;

public static class ProductsAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Product";

    public static void Write(ProductModel product)
    {
        string sql = $"INSERT INTO {Table} (productID, productName, quantity, price, menuID, category) VALUES (@ProductId, @ProductName, @Quantity, @Price, @Menu, @CategoryType)";
        _connection.Execute(sql, product);
    }

    public static ProductModel GetById(int productID)
    {
        string sql = $"SELECT * FROM {Table} WHERE productID = @ProductId";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductId = productID });
    }

    public static ProductModel GetByName(string productName)
    {
        string sql = $"SELECT * FROM {Table} WHERE productName = @ProductName";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductName = productName });
    }

    public static IEnumerable<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ProductModel>(sql);
    }

    public static void Update(ProductModel product)
    {
        string sql = $"UPDATE {Table} SET productName = @ProductName, quantity = @Quantity, price = @Price, menuID = @MenuID, category = @CategoryType WHERE productID = @ProductId";
        _connection.Execute(sql, product);
    }

    public static void Delete(int productID)
    {
        string sql = $"DELETE FROM {Table} WHERE productID = @ProductId";
        _connection.Execute(sql, new { ProductId = productID });
    }

    public static IEnumerable<ProductModel> GetByCategory(string category)
    {
        string sql = $"SELECT * FROM {Table} WHERE category = @CategoryType";
        return _connection.Query<ProductModel>(sql, new { Category = category });
    }
}