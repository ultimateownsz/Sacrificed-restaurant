namespace Restaurant;

using Dapper;
using Microsoft.Data.Sqlite;

public class DataAccess<T1> where T1 : IModel
{
    // centralized connection
    protected static SqliteConnection _db = new($"Data Source=DataSources/project.db");

    private readonly string?[] _fields;
    private readonly string?[] _values;

    // sadly, hard-coded tables identifiers (simplicity)
    protected string _table = typeof(T1) switch
    {
        Type T2 when T2 == typeof(PairModel) => "Pair",
        Type T2 when T2 == typeof(UserModel) => "User",
        Type T2 when T2 == typeof(ThemeModel) => "Theme",
        Type T2 when T2 == typeof(PlaceModel) => "Place",
        Type T2 when T2 == typeof(AllergyModel) => "Allergy",
        Type T2 when T2 == typeof(RequestModel) => "Request",
        Type T2 when T2 == typeof(ProductModel) => "Product",
        Type T2 when T2 == typeof(ScheduleModel) => "Schedule",
        Type T2 when T2 == typeof(AllerlinkModel) => "Allerlink",
        Type T2 when T2 == typeof(ReservationModel) => "Reservation",
        _ => ""
    };

    public DataAccess(string?[] fields)
    {
        _fields = fields;
        _values = _fields.Select(v => '@' + v).ToArray();
    }

    private static bool _execute(string query, object? obj)
    {
        if (obj == null) { return false; }
        try
        {
            _db.Execute(query, obj);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    // low-level operations
    public List<T1?> Read()
    {
        string query = $"SELECT * FROM {_table}";
        return (_db.Query<T1>(query) ?? []).ToList<T1?>();
    }

    public bool Write(T1 item)
    {
        string fields = "(" + String.Join(",", _fields) + ")";
        string values = "(" + String.Join(",", _values) + ")";
        string query = $"INSERT INTO {_table} {fields} VALUES {values}";

        return _execute(query, item);
    }

    public bool Update(T1 item)
    {
        string query = $"UPDATE {_table} SET ";
        foreach (var (field, value) in _fields.Zip(_values))
        {
            // ignore 'ID', since this
            // will be used in the where clause
            if (field == "ID") { continue; }
            query += $"{field} = {value}";

            // do not add a comma
            // on the end of the statement
            if (field != _fields[_fields.Length - 1])
            { query += ", "; }
        }
        query += ' ' + "WHERE ID = @ID";

        return _execute(query, item);
    }

    public bool Delete(int? id)
    {
        string query = $"DELETE FROM {_table} WHERE ID = @ID";
        return _execute(query, new { ID = id });
    }

    // high-level operations
    public T1? GetBy<T2>(string? column, T2? value)
    {
        string query = $"SELECT * FROM {_table} WHERE {column} = @value";
        return _db.QueryFirstOrDefault<T1>(query, new { value = value ?? default});
    }

    public List<T1?> GetAllBy<T2>(string? column, T2? value)
    {
        string query = $"SELECT * FROM {_table} WHERE {column} = @value";
        return _db.Query<T1>(query, new { value = value ?? default }).ToList<T1?>();
    }

    public T1? Trace<T2>(T2 value)
    {
        // coming soon...
        return default;
    }

}
