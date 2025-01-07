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
                ID = 1,
                Date = DateTime.Now.AddDays(7),
                PlaceID = 2,
                UserID = 101
            };
            
            UserModel adminUser = new UserModel
            {
                ID = 1,
                Admin = true
            };

            bool updateCalled = false;

            Access.Reservations = new ExampleReservationAccess
            {
                UpdateAction = r => updateCalled = true
            };

            UpdateReservation.Show(reservation, adminUser);
            TableSelection.SetSimulatedTableID(4);

            Assert.IsTrue(updateCalled, "Reservation update should be called.");
            Assert.AreEqual(4, reservation.PlaceID, "Expected updated PlaceID.");
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

            UserModel nonAdminUser = new UserModel
            {
                ID = 2,
                Admin = false
            };

            bool updateCalled = false;

            // Simulate Access.Reservations.Update
            Access.Reservations = new ExampleReservationAccess
            {
                UpdateAction = r => updateCalled = true
            };

            // Act
            UpdateReservation.Show(reservation, nonAdminUser);
            TableSelection.SetSimulatedTableID(4);

            // Assert
            Assert.IsTrue(updateCalled, "Reservation update should be called.");
            Assert.AreEqual(4, reservation.PlaceID, "Expected updated PlaceID.");
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

        public static class CalendarPresent
        {
            private static DateTime _simulatedDate;

            public static void SetSimulatedDate(DateTime date)
            {
                _simulatedDate = date;
            }

            public static DateTime Show(DateTime currentDate, bool isAdmin, int guests, UserModel acc)
            {
                return _simulatedDate;
            }
        }

        public static class TableSelection
        {
            private static int _simulatedTableID;

            public static void SetSimulatedTableID(int tableID)
            {
                _simulatedTableID = tableID;
            }

            public static int SelectTable(int[] availableTables, int[] inactiveTables, int[] reservedTables, int guests, bool isAdmin)
            {
                return _simulatedTableID;
            }
        }

        public static class UpdateReservation
        {
            public static void Show(ReservationModel reservation, UserModel acc)
            {
                bool admin = acc.Admin.HasValue && acc.Admin.Value;

                if (admin)
                {
                    UpdateReservationDate(reservation);
                }
                else
                {
                    UpdateTableID(reservation);
                }

                // Simulate saving the reservation
                Access.Reservations.Update(reservation);
            }

            private static void UpdateReservationDate(ReservationModel reservation)
            {
                // Simulate updating the date
                reservation.Date = CalendarPresent.Show(DateTime.Now, true, 1, new UserModel { Admin = true });
            }

            private static void UpdateTableID(ReservationModel reservation)
            {
                // Simulate updating the table ID
                reservation.PlaceID = TableSelection.SelectTable(new int[] { 1, 2, 3 }, new int[] { }, new int[] { }, 4, false);
            }
        }
    }
}
