using Microsoft.Data.Sqlite;

using Dapper;

public static class ProductsAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Product";

    public static void Write(ProductModel product)
    {
        // open the db connection
        _connection.Open();
        string sql = $"INSERT INTO {Table} (productID, productName, quantity, price, menuID, category) VALUES (@ProductId, @ProductName, @Quantity, @Price, @MenuID, @Category)";
        
        // instead of using _connection.Execute(sql, product), we can use
        _connection.Execute(sql, new
        {
            // in dapper you make a temporary placeholder similair to the "?" with new { }, instead of directly using the variable name
            // these placeholders will be replaced by the actual values
            // also in the model class you need to make an empty constructor to make it work
            product.ProductId,
            product.ProductName,
            product.Quantity,
            product.Price,
            product.MenuID,
            Category = product.Category.ToString()
        });

        // close the db connection
        _connection.Close();
    }

    public static ProductModel? GetById(long productID)
    {
        // query all columns from product table
        string sql = $"SELECT * FROM {Table} WHERE productID = @ProductId";
        
        // query the result and map it to a dynamic object
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductId = productID });
    }

    public static IEnumerable<ProductModel> GetByIds(IEnumerable<long> productIds)
    {
        // query all columns from product table
        string sql = $"SELECT * FROM {Table} WHERE productID IN @ProductIds";
        
        // query the result and map it to a dynamic object
        return _connection.Query<ProductModel>(sql, new { ProductIds = productIds });
    }

    public static ProductModel? GetByName(string productName)
    {
        string sql = $"SELECT 1 FROM {Table} WHERE productName = @ProductName";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductName = productName });
    }

    public static IEnumerable<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ProductModel>(sql);
    }

    public static IEnumerable<ProductModel> GetAllByCategory(string category)
    {
        string sql = $"SELECT * FROM {Table} WHERE category = @Category";
        return _connection.Query<ProductModel>(sql, new { Category = category });
    }

    public static void Update(ProductModel product)
    {
        string sql = $"UPDATE {Table} SET productName = @ProductName, quantity = @Quantity, price = @Price, menuID = @MenuID, category = @Category WHERE productID = @ProductId";
        _connection.Execute(sql, new
        {
            product.ProductId,
            product.ProductName,
            product.Quantity,
            product.Price,
            product.MenuID,
            product.Category
        });
    }

    public static void Delete(long productID)
    {
        string sql = $"DELETE FROM {Table} WHERE productID = @ProductId";
        var result = _connection.Execute(sql, new { ProductId = productID });

        if (result == 0)
        {
            return;
        }
    }

    public static IEnumerable<ProductModel> GetByCategory(string category)
    {
        string sql = $"SELECT * FROM {Table} WHERE category = @Category";
        return _connection.Query<ProductModel>(sql, new { Category = category });
    }
}