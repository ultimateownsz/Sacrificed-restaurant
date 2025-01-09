namespace Restaurant;
using Dapper;

public class ThemeAccess : DataAccess<ThemeModel>
{
    public ThemeAccess() : base(typeof(ThemeModel).GetProperties().Select(p => p.Name).ToArray()) { }

    
    public Int64 GetLatestThemeID()
    {
        string sql = $"SELECT MAX(ID) FROM {_table}";
        return _db.ExecuteScalar<Int64?>(sql, null) ?? 0;
    }

    public List<ThemeModel> GetAll()
    {
        string sql = $"SELECT * FROM {_table}";
        return _db.Query<ThemeModel>(sql, null).ToList();
    }
}

