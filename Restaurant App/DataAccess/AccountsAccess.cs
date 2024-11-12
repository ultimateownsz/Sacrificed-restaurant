using Microsoft.Data.Sqlite;

using Dapper;


public static class AccountsAccess
{
    private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

    private static string Table = "User";

    public static int Write(AccountModel account)
    {
        string sql = $@"INSERT INTO {Table} (firstName, lastName, email, password, phoneNumber, isAdmin) VALUES (@FirstName, @LastName, @EmailAddress, @Password, @PhoneNumber, @IsAdmin); SELECT last_insert_rowid();";
        int newID = _connection.ExecuteScalar<int>(sql, account);

        return newID;
    }


    //NIET aanpassen
    public static AccountModel GetById(int userID)
    {
        string sql = $"SELECT * FROM {Table} WHERE userID = @UserID";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { UserID = userID });
    }

    //NIET aanpassen
    public static AccountModel GetByEmail(string email)
    {
        string sql = $"SELECT * FROM {Table} WHERE email = @EmailAddress";
        return _connection.QueryFirstOrDefault<AccountModel>(sql, new { EmailAddress = email });
    }

    public static void Update(AccountModel account)
    {
        string sql = $"UPDATE {Table} SET userID = @UserID, firstName = @FirstName, lastName = @LastName, email = @EmailAddress, password = @Password, phoneNumber = @PhoneNumber, isAdmin = @IsAdmin WHERE userID = @UserID";
        _connection.Execute(sql, account);
    }

    public static void Delete(int userID)
    {
        string sql = $"DELETE FROM {Table} WHERE userID = @UserID";
        _connection.Execute(sql, new { UserID = userID });
    }

}
