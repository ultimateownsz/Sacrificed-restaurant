using App.DataAccess.Utils;
using App.DataModels.Product;
using Dapper;
namespace App.DataAccess.Product;
public class PairAccess : DataAccess<PairModel>
{
    public PairAccess() : base(typeof(PairModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public List<PairModel> GetAll()
    {
        string sql = $"SELECT * FROM {_table}";
        return _db.Query<PairModel>(sql, null).ToList();
    }
}
