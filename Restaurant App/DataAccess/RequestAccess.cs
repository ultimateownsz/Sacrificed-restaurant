namespace Project;
using Dapper;

public class RequestAccess : DataAccess<RequestModel>
{
    public RequestAccess() : base(typeof(RequestModel).GetProperties().Select(p => p.Name).ToArray()) { }

    // compatibility-integration
    public Int64 GetLatestOrderID()
    {
        string sql = $"SELECT MAX(orderID) FROM {_table}";
        return _db.ExecuteScalar<Int64?>(sql) ?? 0;
    }
}
