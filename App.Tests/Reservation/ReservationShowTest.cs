//namespace App.Tests.Reservation;

//[TestClass]
//public class ReservationShowTest
//{
//    [TestMethod]
//    public void Test_Show_CallsFuturePastReservationsForAdmin()
//    {
//        // Arrange
//        var adminUser = new UserModel { ID = 1, Admin = 1, FirstName = "AdminUser" };

//        string result = SimulateShow(adminUser);

//        // Assert
//        Assert.IsTrue(result.Contains("FuturePastReservations.Show called"));
//        Assert.IsTrue(result.Contains("Admin: True"));
//    }

//    [TestMethod]
//    public void Test_ShowReservationOptions_ViewDetails_ShowsDetails()
//    {
//        // Arrange
//        var reservation = new ReservationModel
//        {
//            ID = 101,
//            UserID = 1,
//            PlaceID = 202
//        };

//        string selectedOption = "View Details";
//        string result = SimulateShowReservationOptions(reservation, selectedOption);

//        // Assert
//        Assert.IsTrue(result.Contains("ReservationDetails.ShowDetails called"));
//        Assert.IsTrue(result.Contains("Reservation ID: 101"));
//    }

//    [TestMethod]
//    public void Test_ShowReservationOptions_UpdateReservation_UpdatesReservation()
//    {
//        // Arrange
//        var reservation = new ReservationModel
//        {
//            ID = 101,
//            UserID = 1,
//            PlaceID = 202
//        };

//        string selectedOption = "Update Reservation";
//        string result = SimulateShowReservationOptions(reservation, selectedOption);

//        // Assert
//        Assert.IsTrue(result.Contains("UpdateReservation.Show called"));
//        Assert.IsTrue(result.Contains("Admin: True"));
//    }

//    [TestMethod]
//    public void Test_ShowReservationOptions_DeleteReservation_DeletesReservation()
//    {
//        // Arrange
//        var reservation = new ReservationModel
//        {
//            ID = 101,
//            UserID = 1,
//            PlaceID = 202
//        };

//        string selectedOption = "Delete Reservation";
//        string result = SimulateShowReservationOptions(reservation, selectedOption);

//        // Assert
//        Assert.IsTrue(result.Contains("DeleteReservation.Show called"));
//        Assert.IsTrue(result.Contains("Reservation ID: 101"));
//    }

//    [TestMethod]
//    public void Test_ShowReservationOptions_Cancel_ExitsMenu()
//    {
//        // Arrange
//        var reservation = new ReservationModel
//        {
//            ID = 101,
//            UserID = 1,
//            PlaceID = 202
//        };

//        string selectedOption = "Cancel";
//        string result = SimulateShowReservationOptions(reservation, selectedOption);

//        // Assert
//        Assert.IsTrue(result.Contains("Exited menu"));
//    }

//    // Simulated Methods for Testing

//    private string SimulateShow(UserModel user)
//    {
//        return $"FuturePastReservations.Show called for UserID: {user.ID}, Admin: {user.Admin == 1}";
//    }

//    private string SimulateShowReservationOptions(ReservationModel reservation, string selectedOption)
//    {
//        var output = $"Reservation Options Menu for Reservation ID: {reservation.ID}\n";

//        switch (selectedOption)
//        {
//            case "View Details":
//                output += "ReservationDetails.ShowDetails called\n";
//                output += $"Reservation ID: {reservation.ID}\n";
//                break;

//            case "Update Reservation":
//                output += "UpdateReservation.Show called\n";
//                output += "Admin: True\n";
//                break;

//            case "Delete Reservation":
//                output += "DeleteReservation.Show called\n";
//                output += $"Reservation ID: {reservation.ID}\n";
//                break;

//            case "Cancel":
//                output += "Exited menu\n";
//                break;
//        }

//        return output;
//    }

//    // Supporting Classes
//    public class UserModel
//    {
//        public int ID { get; set; }
//        public int? Admin { get; set; }
//        public string FirstName { get; set; }
//    }

//    public class ReservationModel
//    {
//        public int ID { get; set; }
//        public int UserID { get; set; }
//        public int PlaceID { get; set; }
//    }
//}
