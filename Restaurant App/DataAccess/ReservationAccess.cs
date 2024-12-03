using Microsoft.Data.Sqlite;
using Dapper;
using System.Collections.Generic;

public static class ReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "Reservation";

    public static void Write(ReservationModel reservation)
    {
        string sql = $"INSERT INTO {Table} (reservationID, date, tableChoice, reservationAmount, userID, tableID) VALUES (@ReservationID, @Date, @TableChoice, @ReservationAmount, @UserID, @TableID)";
        _connection.Execute(sql, new
        {
            ReservationID = reservation.ID,
            Date = reservation.Date,
            TableChoice = reservation.TableChoice,
            ReservationAmount = reservation.ReservationAmount,
            UserID = reservation.UserID,
            TableID = reservation.TableID
        });
    }

    public static List<ReservationModel> GetReservationsByDate(int date)
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice,  
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table} 
            WHERE date = @Date";

        return _connection.Query<ReservationModel>(sql, new { Date = date }).AsList();
    }



    public static List<ReservationModel> GetAllReservations()
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice,  
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table}";
        return _connection.Query<ReservationModel>(sql).AsList();
    }

    public static List<ReservationModel> GetReservationsByMonthYear(int month, int year)
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice, 
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table}
            WHERE CAST(SUBSTR(date, 3, 2) AS INT) = @Month 
              AND CAST(SUBSTR(date, 5, 4) AS INT) = @Year";

        return _connection.Query<ReservationModel>(sql, new { Month = month, Year = year }).AsList();
    }

    public static string GetThemeByMenuID(int menuID)
    {
        string sql = "SELECT theme FROM Menu WHERE menuID = @MenuID";
        return _connection.QueryFirstOrDefault<string>(sql, new { MenuID = menuID });
    }

    public static ReservationModel GetByReservationID(int reservationID)
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice, 
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table} 
            WHERE reservationID = @ReservationID";
        return _connection.QueryFirstOrDefault<ReservationModel>(sql, new { ReservationID = reservationID });
    }

    public static List<ReservationModel> GetByUserID(int userID)
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice, 
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table} 
            WHERE userID = @UserID";
        return _connection.Query<ReservationModel>(sql, new { UserID = userID }).AsList();
    }

    public static List<ReservationModel> GetByTableID(int tableID)
    {
        string sql = $@"
            SELECT 
                reservationID AS ID, 
                date AS Date, 
                tableChoice AS TableChoice, 
                reservationAmount AS ReservationAmount, 
                userID AS UserID,
                tableID AS TableID
            FROM {Table} 
            WHERE tableID = @TableID";

        return _connection.Query<ReservationModel>(sql, new { TableID = tableID }).AsList();
    }

    public static List<ProductModel> GetMenuItemsByReservationID(int reservationID)
    {
        string sql = @"
        SELECT 
            p.productID AS ProductId, 
            p.productName AS ProductName, 
            p.quantity AS Quantity, 
            p.price AS Price, 
            p.menuID AS MenuID, 
            p.category AS Category
        FROM 
            [Order] o
        JOIN 
            Product p ON o.productID = p.productID
        WHERE 
            o.reservationID = @ReservationID";

        return _connection.Query<ProductModel>(sql, new { ReservationID = reservationID }).AsList();
    }

    public static void Update(ReservationModel reservation)
    {
        using (var connection = new SqliteConnection("Data Source=DataSources/project.db"))
        {
            connection.Open();
            string sql = $@"
                UPDATE {Table} 
                SET 
                    date = @Date, 
                    tableChoice = @TableChoice,  
                    reservationAmount = @ReservationAmount, 
                    userID = @UserID,
                    tableID = @TableID
                WHERE reservationID = @ReservationID";
            connection.Execute(sql, new
            {
                ReservationID = reservation.ID,
                Date = reservation.Date,
                TableChoice = reservation.TableChoice,
                ReservationAmount = reservation.ReservationAmount,
                UserID = reservation.UserID,
                TableID = reservation.TableID
            });
        }
    }

    public static void Delete(int reservationID)
    {
        string sql = $"DELETE FROM {Table} WHERE reservationID = @ReservationID";
        _connection.Execute(sql, new { ReservationID = reservationID });
    }

    //This function is called in the logic layer(ReservationLogic.cs)
    //It gets the latest(MAX) reservation ID in the database
    public static Int64 GetLatestReservationID()
    {
        string sql = $"SELECT MAX(reservationID) FROM {Table}";
        return _connection.ExecuteScalar<Int64?>(sql) ?? 0;
    }
}
