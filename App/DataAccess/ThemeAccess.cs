using Restaurant;

namespace App.DataAccess;

using App.DataAccess.Utils;
using Dapper;

internal class ThemeAccess : DataAccess<ThemeModel>
{
    internal ThemeAccess() : base(typeof(ThemeModel).GetProperties().Select(p => p.Name).ToArray()) { }


    public long GetLatestThemeID()
    {
        string sql = $"SELECT MAX(ID) FROM {_table}";
        return _db.ExecuteScalar<long?>(sql, null) ?? 0;
    }

    public List<ThemeModel> GetAll()
    {
        string sql = $"SELECT * FROM {_table}";
        return _db.Query<ThemeModel>(sql, null).ToList();
    }
}

