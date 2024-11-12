
using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccess
{
    public static class CalendarAccess
    {
        private static SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");

        public static List<string> GetAvailableTables(DateTime selectedDate)
        {
            string sql = "SELECT TableNumber FROM Tables WHERE IsAvailable = 1 AND Date = @Date";
            var tables = _connection.Query<string>(sql, new { Date = selectedDate.Date });
            return tables.AsList();
        }
    }
}
