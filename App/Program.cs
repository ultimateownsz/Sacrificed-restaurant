using App.DataAccess.Utils;
using App.DataModels.Allergy;
using App.DataModels.Product;
using App.DataModels.Utils;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.Reflection;

namespace Restaurant;

class Program
{
    static void Main()
    {
        MenuPresent.Init();
        MenuPresent.Start();
    }
}
