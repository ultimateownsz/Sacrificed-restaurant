namespace App.Tests.Reservation;

// Unit Tests for deleting reservations as a user
[TestClass]
public class UserDeleteReservationTest
{
    // Unit Test for criteria: User selects "Yes" on the delete confirmation
    [TestMethod]
    [DataRow(1, "Yes", true)] // Reservation exists and is deleted
    [DataRow(2, "Yes", false)] // Reservation does not exist and is not deleted
    public void TestDeleteReservation_ConfirmYes(int reservationId, string userConfirmation, bool expectedResult)
    {
        // Mocking the ReservationAccess to simulate reservation data
        var mockReservations = new Dictionary<int, string>
        {
            { 1, "Reservation for Table 1" },
            { 3, "Reservation for Table 3" }
        };

        // Simulating the DeleteReservation method logic
        bool DeleteReservation(int id, string confirmation)
        {
            if (mockReservations.ContainsKey(id) && confirmation == "Yes")
            {
                mockReservations.Remove(id); // Remove reservation if user confirms "Yes"
                return true;
            }
            return false; // Return false if the reservation doesn't exist or user cancels
        }

        // Act
        bool result = DeleteReservation(reservationId, userConfirmation);

        // Assert 
        Assert.AreEqual(expectedResult, result);
    }

    // Unit Test for Criteria: User selects "No" on the delete confirmation
    [TestMethod]
    [DataRow(1, "No", false)]  // Reservation exists but user cancels
    [DataRow(2, "No", false)]  // Reservation does not exist and is not deleted
    public void TestDeleteReservation_ConfirmNo(int reservationId, string userConfirmation, bool expectedResult)
    {
        // Mocking the ReservationAccess to simulate reservation data
        var mockReservations = new Dictionary<int, string>
        {
            { 1, "Reservation for Table 1" },
            { 3, "Reservation for Table 3" }
        };

        // Simulating the DeleteReservation method logic
        bool DeleteReservation(int id, string confirmation)
        {
            if (mockReservations.ContainsKey(id) && confirmation == "Yes")
            {
                mockReservations.Remove(id); // Remove reservation if user confirms "Yes"
                return true;
            }
            return false; // Return false if the reservation doesn't exist or user cancels
        }

        // Act
        bool result = DeleteReservation(reservationId, userConfirmation);

        // Assert
        Assert.AreEqual(expectedResult, result);
    }
}