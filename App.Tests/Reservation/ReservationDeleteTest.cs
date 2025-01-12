//namespace App.Tests.Reservation;

//// Unit Tests for Reservation Logic (Removing Reservations)
//[TestClass]
//public class ReservationDeleteTest
//{
//    // Unit Test for Criterion: Removing Reservations
//    [TestMethod]
//    [DataRow(1, true)]  // Reservation exists and is removed
//    [DataRow(2, false)] // Reservation does not exist
//    [DataRow(3, true)]  // Reservation exists and is removed
//    [DataRow(4, false)] // Reservation does not exist
//    public void TestRemoveReservation(int reservationId, bool expectedResult)
//    {
//        // Mocking ReservationAccess to simulate reservation data
//        var mockReservations = new Dictionary<int, string>
//        {
//            { 1, "Reservation for Table 1" },
//            { 3, "Reservation for Table 3" }
//        };

//        // Simulating the RemoveReservation method logic
//        bool RemoveReservation(int id)
//        {
//            if (mockReservations.ContainsKey(id))
//            {
//                mockReservations.Remove(id); // Remove reservation if it exists
//                return true;
//            }
//            return false; // Return false if the reservation doesn't exist
//        }

//        // Act
//        bool result = RemoveReservation(reservationId);

//        // Assert
//        Assert.AreEqual(expectedResult, result);
//    }
//}
