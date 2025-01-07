using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class UpdateReservationTest
    {
        [TestMethod]
        public void TestUpdateReservation_Admin()
        {
            var reservation = new ReservationModel
            {
                ID = 1;
                Date = delegate.Now.AddDays(7),
                PlaceID = 2,
                TestUpdateReservation_Admin = 101
            };
            
            var adminUser = new UserModel
            {
                ID = 1;
                Admin = true;
            };

            bool updateCalled = false;

            Access.Reservations = new ExampleReservationAccess
            {
                UdpateAction = ref => updateCalled = true;
            };

            UpdateReservation.Show(reservation, adminUser);

            Assert.IsTrue(updateCalled, "Reservation update should be called.");
            Assert.AreEqual(1, reservation.PlaceID, "Expected updated PlaceID.");
        }

        [TestMethod]
        public void TestUpdateReservation_User()
        {
            var reservation = new ReservationModel
            {
                ID = 2,
                Date = DateTime.Now.AddDays(10),
                PlaceID = 5,
                UserID = 202
            };

            var adminUser = new UserModel
            {
                ID = 2;
                Admin = false;
            };
        }
    }
    // public class FuturePastReservationsTests
    // {
    //     [TestMethod]
    //     public void Test_Show_Admin_DisplaysAdminView()
    //     {
    //         // Arrange
    //         var adminUser = new UserModel
    //         {
    //             Admin = 1,
    //             ID = 123,
    //             FirstName = "AdminUser"
    //         };

    //         var reservationList = new List<ReservationModel>
    //         {
    //             new ReservationModel { ID = 1, Date = DateTime.Now, UserID = 456, PlaceID = 789 }
    //         };

    //         DateTime selectedDate = DateTime.Now;

    //         // Act
    //         string output = SimulateShowReservationsForAdmin(adminUser, selectedDate, reservationList);

    //         // Assert
    //         Assert.IsTrue(output.Contains("Date:"));
    //         Assert.IsTrue(output.Contains("Place ID: 789"));
    //     }

    //     [TestMethod]
    //     public void Test_Show_User_DisplaysUserReservations()
    //     {
    //         // Arrange
    //         var regularUser = new UserModel
    //         {
    //             Admin = null,
    //             ID = 123,
    //             FirstName = "RegularUser"
    //         };

    //         var reservationList = new List<ReservationModel>
    //         {
    //             new ReservationModel { ID = 1, Date = DateTime.Now.AddDays(1), UserID = 123, PlaceID = 10 },
    //             new ReservationModel { ID = 2, Date = DateTime.Now.AddDays(2), UserID = 123, PlaceID = 20 }
    //         };

    //         // Act
    //         string output = SimulateShowReservationsForUser(regularUser, reservationList);

    //         // Assert
    //         Assert.IsTrue(output.Contains("Place ID: 10"));
    //         Assert.IsTrue(output.Contains("Place ID: 20"));
    //     }

    //     [TestMethod]
    //     public void Test_Show_User_NoReservations_ShowsMessage()
    //     {
    //         // Arrange
    //         var regularUser = new UserModel
    //         {
    //             Admin = null,
    //             ID = 123,
    //             FirstName = "RegularUser"
    //         };

    //         var reservationList = new List<ReservationModel>(); // No reservations

    //         // Act
    //         string output = SimulateShowReservationsForUser(regularUser, reservationList);

    //         // Assert
    //         Assert.IsTrue(output.Contains("No reservations found."));
    //     }

    //     // Simulated Methods for Admin/User Display Logic

    //     private string SimulateShowReservationsForAdmin(UserModel user, DateTime date, List<ReservationModel> reservations)
    //     {
    //         var output = string.Empty;

    //         output += $"Admin View - Reservations for {date.ToShortDateString()}:\n";

    //         foreach (var reservation in reservations)
    //         {
    //             output += $"Reservation ID: {reservation.ID}\n";
    //             output += $"Date: {reservation.Date}\n";
    //             output += $"Place ID: {reservation.PlaceID}\n";
    //             output += $"User ID: {reservation.UserID}\n";
    //         }

    //         if (reservations.Count == 0)
    //             output += "No reservations found.";

    //         return output;
    //     }

    //     private string SimulateShowReservationsForUser(UserModel user, List<ReservationModel> reservations)
    //     {
    //         var output = $"User View - Reservations for {user.FirstName}:\n";

    //         foreach (var reservation in reservations)
    //         {
    //             if (reservation.UserID == user.ID)
    //             {
    //                 output += $"Reservation ID: {reservation.ID}\n";
    //                 output += $"Date: {reservation.Date?.ToShortDateString()}\n";
    //                 output += $"Place ID: {reservation.PlaceID}\n";
    //             }
    //         }

    //         if (reservations.Count == 0)
    //             output += "No reservations found.";

    //         return output;
    //     }

    //     // Supporting Classes for Tests
    //     public class UserModel
    //     {
    //         public int? Admin { get; set; }
    //         public int ID { get; set; }
    //         public string FirstName { get; set; }
    //     }

    //     public class ReservationModel
    //     {
    //         public int ID { get; set; }
    //         public DateTime? Date { get; set; }
    //         public int UserID { get; set; }
    //         public int PlaceID { get; set; }
    //     }
    // }
}
