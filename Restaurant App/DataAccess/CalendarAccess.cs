using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.Sqlite;

namespace DataAccess
{
    public static class CalendarAccess
    {
        private static SqliteConnection _connection = new SqliteConnection("Data Source=DataSources/project.db");

        public static List<string> GetAvailableTables(DateTime selectedDate)
        {
            // Format the date to match the format in the database (daymonthyear)
            string formattedDate = selectedDate.ToString("ddMMyyyy");

            // Updated SQL query to use the correct table name (reservation) and date format
            string sql = "SELECT tableChoice FROM Reservation WHERE date = @Date";
            var tables = _connection.Query<string>(sql, new { Date = formattedDate });

            return tables.AsList();
        }
    }
}
