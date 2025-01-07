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
            ReservationModel reservation = new ReservationModel
            {
                ID = 1;
                Date = delegate.Now.AddDays(7),
                PlaceID = 2,
                TestUpdateReservation_Admin = 101
            };
            
            UserModel adminUser = new UserModel
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
            ReservationModel reservation = new ReservationModel
            {
                ID = 2,
                Date = DateTime.Now.AddDays(10),
                PlaceID = 5,
                UserID = 202
            };

            UserModel adminUser = new UserModel
            {
                ID = 2;
                Admin = false;
            };

            bool updateCalled = false;

            // Simulate Access.Reservations.Update
            Access.Reservations = new FakeReservationAccess
            {
                UpdateAction = r => updateCalled = true
            };

            // Act
            UpdateReservation.Show(reservation, nonAdminUser);

            // Assert
            Assert.IsTrue(updateCalled, "Reservation update should be called.");
            Assert.AreEqual(10, reservation.PlaceID, "Expected updated PlaceID.");
        }

        public class UserModel
        {
            public int ID { get; set; }
            public bool? Admin { get; set; }
        }

        public class ReservationModel
        {
            public int ID { get; set; }
            public DateTime Date { get; set; }
            public int PlaceID { get; set; }
            public int UserID { get; set; }
        }

        public interface IReservationAccess
        {
            void Update(ReservationModel reservation);
        }

        // Static Access Simulated with a Property
        public static class Access
        {
            public static IReservationAccess Reservations { get; set; } = new ExampleReservationAccess();
        }

        public class ExampleReservationAccess : IReservationAccess
        {
            public Action<ReservationModel>? UpdateAction { get; set; }

            public void Update(ReservationModel reservation)
            {
                UpdateAction?.Invoke(reservation);
            }
        }
    }
}
