using App.DataAccess.Utils;
using Restaurant;
using Dapper;

namespace App.DataAccess;
public class ScheduleAccess : DataAccess<ScheduleModel>
{
    public ScheduleAccess() : base(typeof(ScheduleModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public List<ScheduleModel> GetAll()
    {
        string sql = $"SELECT * FROM {_table}";
        return _db.Query<ScheduleModel>(sql, null).ToList();
    }
}
