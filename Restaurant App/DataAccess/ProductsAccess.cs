using Microsoft.Data.Sqlite;

using Dapper;

public static class ProductsAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    private static string Table = "Product";

    public static void Write(ProductModel product)
    {
        // 
        _connection.Open();
        string sql = $"INSERT INTO {Table} (productID, productName, quantity, price, menuID, category) VALUES (@ProductId, @ProductName, @Quantity, @Price, @MenuID, @Category)";
        _connection.Execute(sql, new
        {
            product.ProductId,
            product.ProductName,
            product.Quantity,
            product.Price,
            product.MenuID,
            product.Category
        });

        _connection.Close();
    }

    public static bool ProductExists(long productId)
    {
        // Select 1 means that it checks if the row productID exists, instead of using 'Select *'
        string sql = $"SELECT * FROM {Table} WHERE productID = @ProductId";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductId = productId }) != null;
    }

    public static ProductModel GetById(long productID)
    {
        // query all columns from product table
        string sql = $"SELECT * FROM {Table} WHERE productID = @ProductId";
        
        // query the result and map it to a dynamic object
        var result = _connection.QueryFirstOrDefault<dynamic>(sql, new { ProductId = productID });

        if (result == null)
        {
            return null;
        }

        var categoryString = result.Category.ToString();
        
        // map the result to a ProductModel object
        return new ProductModel(
            result.ProductId,
            result.ProductName,
            result.Quantity,
            result.Price,
            result.MenuID,
            result.Category
        );
    }

    public static ProductModel GetByName(string productName)
    {
        string sql = $"SELECT 1 FROM {Table} WHERE productName = @ProductName";
        return _connection.QueryFirstOrDefault<ProductModel>(sql, new { ProductName = productName });
    }

    public static IEnumerable<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ProductModel>(sql);
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