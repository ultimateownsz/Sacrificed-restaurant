using App.DataModels.Allergy;
using App.DataModels.Product;
using Dapper;
using Microsoft.Data.Sqlite;
using Restaurant;

namespace App.Tests;

[TestClass]
public class DataSynchronizationTest
{

    private static SqliteConnection _db = new($"Data Source=DataSources/project.db");

    private static Dictionary<string, Type> _mapping = new()
    {
        {"Reservation", typeof(ReservationModel)},
        {"Allerlink", typeof(AllerlinkModel)},
        {"Schedule", typeof(ScheduleModel)},
        {"Product", typeof(ProductModel)},
        {"Allergy", typeof(AllergyModel)},
        {"Request", typeof(RequestModel)},
        {"Theme", typeof(ThemeModel)},
        {"Place", typeof(PlaceModel)},
        {"Pair", typeof(PairModel)},
        {"User", typeof(UserModel)},
    };

    public static bool ColumnSynchronization(string table)
    {

        // ignore auto-increment tracking table
        if (table == "sqlite_sequence")
            return true;

        // collect all column
        IEnumerable<dynamic> registers = _db.Query(
            $"PRAGMA table_info({table})");

        // obtain all properties in model
        List<string> properties;
        try
        {
            properties = _mapping[table]
                .GetProperties().Select(x => x.Name).ToList();
        }
        catch (KeyNotFoundException)
        {
            Assert.Fail($"Table: '{table}' exists in database the " +
                        "but is not registered in the model-map");
            return false;
        }

        // verify that all table columns are in the models
        foreach (string column in registers.Select(x => x.name))
        {
            if (!properties.Contains(column))
                return false;
        }

        return true;
    }

    public bool TableSynchronization(List<string> tables)
    {        
        foreach (string table in tables)
        {
            if (!ColumnSynchronization(table))
                return false;
        }

        return true;
    }

    [TestMethod] // test all existing tables by default
    public void TableSynchronization()
    {
        // collect all tables (raw)
        IEnumerable<dynamic> collection = _db.Query(
            "SELECT name FROM sqlite_master WHERE type='table'");

        // extract table identities (dynamic -> names)
        List<string> tables = new();
        foreach (string table in collection.Select(x => x.name))
            tables.Add(table);

        // validate all tables
        Assert.IsTrue(TableSynchronization(tables));
    }
}
