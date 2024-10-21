using Microsoft.Data.Sqlite;
using Dapper;
using System.Collections.Generic;

public static class ReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "Reservation";

    public static void Write(ReservationModel reservation)
    {
        string sql = $"INSERT INTO {Table} (reservationID, date, tableChoice, reservationAmount, userID) VALUES (@ReservationID, @Date, @TableChoice, @ReservationAmount, @UserID)";
        _connection.Execute(sql, reservation);
    }

    public static List<ReservationModel> GetAllReservations()
    {
        string sql = $"SELECT * FROM {Table}";
        return _connection.Query<ReservationModel>(sql).AsList();
    }

    public static ReservationModel GetByReservationID(int reservationID)
    {
        string sql = $"SELECT * FROM {Table} WHERE reservationID = @ReservationID";
        return _connection.QueryFirstOrDefault<ReservationModel>(sql, new { ReservationID = reservationID });
    }

    public static List<ReservationModel> GetByUserID(int userID)
    {
        string sql = $"SELECT * FROM {Table} WHERE userID = @UserID";
        return _connection.Query<ReservationModel>(sql, new { UserID = userID }).AsList();
    }

    public static List<ReservationModel> GetByDate(int date)
    {
        string sql = $"SELECT * FROM {Table} WHERE date = @Date";
        return _connection.Query<ReservationModel>(sql, new { Date = date }).AsList();
    }

    public static List<ReservationModel> GetFilteredReservations(int? reservationID = null, int? date = null, int? userID = null)
    {
        var sql = $"SELECT * FROM {Table} WHERE 1=1";
        var parameters = new DynamicParameters();

        if (reservationID.HasValue)
        {
            sql += " AND reservationID = @ReservationID";
            parameters.Add("ReservationID", reservationID.Value);
        }

        if (date.HasValue)
        {
            sql += " AND date = @Date";
            parameters.Add("Date", date.Value);
        }

        if (userID.HasValue)
        {
            sql += " AND userID = @UserID";
            parameters.Add("UserID", userID.Value);
        }

        return _connection.Query<ReservationModel>(sql, parameters).AsList();
    }

    public static void Update(ReservationModel reservation)
    {
        string sql = $"UPDATE {Table} SET date = @Date, tableChoice = @TableChoice, reservationAmount = @ReservationAmount, userID = @UserID WHERE reservationID = @ReservationID";
        _connection.Execute(sql, reservation);
    }

    public static void Delete(int reservationID)
    {
        string sql = $"DELETE FROM {Table} WHERE reservationID = @ReservationID";
        _connection.Execute(sql, new { ReservationID = reservationID });
    }

    //This fucnction is called in the logic layer(ReservationLogic.cs)
    //It gets the latest(MAX) reservation ID in the database
    public static Int64 GetLatestReservationID()
    {
        string sql = $"SELECT MAX(reservationID) FROM {Table}";
        return _connection.QueryFirstOrDefault<Int64>(sql);
    }

}
