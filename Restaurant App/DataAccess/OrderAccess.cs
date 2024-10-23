using Microsoft.Data.Sqlite;

using Dapper;


public static class OrderAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    public static string Table => "[Order]";
    
    public static void Write(OrderModel Order)
    {
        string sql = $"INSERT INTO {Table} (orderID, reservationID, productID) VALUES (@OrderID, @ReservationID, @ProductID)";
        _connection.Execute(sql, Order);
    }

    public static OrderModel GetById(int orderID)
    {
        string sql = $"SELECT * FROM {Table} WHERE orderID = @OrderID";
        return _connection.QueryFirstOrDefault<OrderModel>(sql, new { OrderID = orderID });
    }

    public static OrderModel GetByEmail(string reservationID)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @ReservationID";
        return _connection.QueryFirstOrDefault<OrderModel>(sql, new { ReservationID = reservationID });
    }

    public static void Update(OrderModel order)
    {
        string sql = $"UPDATE {Table} SET orderID = @OrderID, reservationID = @ReservationID, productID = @ProductID";
        _connection.Execute(sql, order);
    }

    public static void Delete(int orderID)
    {
        string sql = $"DELETE FROM {Table} WHERE orderID = @OrderID";
        _connection.Execute(sql, new { OrderID = orderID });
    }
    public static Int64 GetLatestOrderID()
    {
        string sql = $"SELECT MAX(orderID) FROM {Table}";
        return _connection.ExecuteScalar<Int64?>(sql) ?? 0;
    }
}