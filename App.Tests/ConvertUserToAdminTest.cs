using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using Microsoft.Data.Sqlite;
using Restaurant;
using App.DataModels.Utils;
using System.Linq;

[TestClass]
public class ConvertUserToAdminTest
{
    private static SqliteConnection _dbConnection;

    // Setup the connection to the actual database (replace with your real connection string)
    [TestInitialize]
    public void SetUp()
    {
        // Connect to your actual database (replace with your actual database path or connection string)
        _dbConnection = new SqliteConnection("Data Source=DataSources/project.db");

        // Open the connection to the actual database
        _dbConnection.Open();
    }

    // Cleanup the connection after the test is done
    [TestCleanup]
    public void CleanUp()
    {
        // Close the connection after the test
        _dbConnection.Close();
    }

    [TestMethod]
    [DataRow("John", "Doe", false, 1)] // User exists but is already an admin
    [DataRow("Jane", "Doe", true, 1)]  // User exists and is promoted
    [DataRow("Piet", "Pieter", false, 0)] // User does not exist
    public void TestUpdateDatabaseForPromotion(string firstName, string lastName, bool expectedSuccess, int expectedAdminStatus)
    {
        // Act: Try to fetch the user from the actual database
        var user = _dbConnection.QueryFirstOrDefault<UserModel>(
            "SELECT * FROM User WHERE FirstName = @FirstName AND LastName = @LastName",  // Correct table name 'User'
            new { FirstName = firstName, LastName = lastName });

        bool result = false;
        if (user != null && user.Admin == 0)
        {
            // Promote to admin (update the record)
            _dbConnection.Execute(
                "UPDATE User SET Admin = 1 WHERE ID = @ID",  // Correct table name 'User'
                new { ID = user.ID });
            result = true;
        }

        // Fetch the updated user from the database
        var updatedUser = _dbConnection.QueryFirstOrDefault<UserModel>(
            "SELECT * FROM User WHERE FirstName = @FirstName AND LastName = @LastName",  // Correct table name 'User'
            new { FirstName = firstName, LastName = lastName });

        // Assert: Verify the outcome
        Assert.AreEqual(expectedSuccess, result);
        Assert.AreEqual(expectedAdminStatus, updatedUser?.Admin ?? 0);
    }
}