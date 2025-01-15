using Restaurant;

namespace App.DataAccess;

using App.DataAccess.Utils;
using Dapper;

public class RequestAccess : DataAccess<RequestModel>
{
    public RequestAccess() : base(typeof(RequestModel).GetProperties().Select(p => p.Name).ToArray()) { }

    // compatibility/integration
    public long GetLatestOrderID()
    {
        string sql = $"SELECT MAX(orderID) FROM {_table}";
        return _db.ExecuteScalar<long?>(sql) ?? 0;
    }
}
