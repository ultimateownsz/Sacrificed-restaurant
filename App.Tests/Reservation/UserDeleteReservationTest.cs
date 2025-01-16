using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dapper;
using Microsoft.Data.Sqlite;
using Restaurant;
using App.DataModels;
using App.DataAccess;
using System;

[TestClass]
public class ReservationDeletionTest
{
    private static SqliteConnection _dbConnection;

    // Setup the connection to the actual database
    [TestInitialize]
    public void SetUp()
    {
        _dbConnection = new SqliteConnection("Data Source=DataSources/project.db");
        _dbConnection.Open();

        // Insert a user into the User table
        var insertUserQuery = _dbConnection.Execute(
            "INSERT INTO User (FirstName, LastName, Email, Phone, Password, Admin) " +
            "VALUES (@FirstName, @LastName, @Email, @Phone, @Password, @Admin)",
            new { FirstName = "John", LastName = "Doe", Email = "john@example.com", Phone = "123456789", Password = "password123", Admin = 0 }
        );

        // Get the inserted user ID (last inserted user)
        var userId = _dbConnection.QuerySingle<int>("SELECT last_insert_rowid()");

        // Insert a reservation for the created user
        var insertReservationQuery = _dbConnection.Execute(
            "INSERT INTO Reservation (Date, UserID, PlaceID) VALUES (@Date, @UserID, @PlaceID)",
            new { Date = DateTime.Now, UserID = userId, PlaceID = 1 }
        );

        // Get the inserted reservation ID (last inserted reservation)
        var reservationId = _dbConnection.QuerySingle<int>("SELECT last_insert_rowid()");
    }

    // Clean up the database after each test
    [TestCleanup]
    public void CleanUp()
    {
        _dbConnection.Close();
    }

    [TestMethod]
    public void TestReservationDelete()
    {
        // Arrange: Set up the ReservationAccess class
        var reservationAccess = new ReservationAccess();

        // Act: Try to delete the reservation by its ID
        var reservationIdToDelete = 1;
        bool deletionResult = reservationAccess.Delete(reservationIdToDelete);

        // Assert: Verify that the reservation is no longer in the database
        var deletedReservation = _dbConnection.QueryFirstOrDefault<ReservationModel>(
            "SELECT * FROM Reservation WHERE ID = @ID", new { ID = reservationIdToDelete });

        Assert.IsNull(deletedReservation);  // Reservation should be null after deletion
    }
}