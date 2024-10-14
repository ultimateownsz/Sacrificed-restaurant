using Microsoft.Data.Sqlite;

using Dapper;


public static class ReservationAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "Reservation";

    public static void Write(ReservationModel account)
    {
        string sql = $"INSERT INTO {Table} (reservationID, date, tableChoice, reservationAmount, userID) VALUES (@ReservationID, @Date, @TableChoice, @ReservationAmount, @UserID)";
        _connection.Execute(sql, account);
    }


    public static ReservationModel GetByReservationID(int reservationID)
    {
        string sql = $"SELECT * FROM {Table} WHERE reservationID = @ReservationID";
        return _connection.QueryFirstOrDefault<ReservationModel>(sql, new { ReservationID = reservationID });
    }

    public static ReservationModel GetByuserID(string userID)
    {
        string sql = $"SELECT * FROM {Table} WHERE userID = @UserID";
        return _connection.QueryFirstOrDefault<ReservationModel>(sql, new { UserID = userID });
    }

    public static void Update(ReservationModel account)
    {
        string sql = $"UPDATE {Table} SET date = @Date, tableChoice = @TableChoice, reservationAmount = @ReservationAmount, userID = @UserID WHERE reservationID = @ReservationID";
        _connection.Execute(sql, account);
    }

    public static void Delete(int reservationID)
    {
        string sql = $"DELETE FROM {Table} WHERE reservationID = @ReservationID";
        _connection.Execute(sql, new { ReservationID = reservationID });
    }

}
