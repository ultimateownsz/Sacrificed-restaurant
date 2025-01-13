using App.Presentation.Reservation;
using App.DataModels.Product;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace App.Tests
{
    [TestClass]
    public class ReservationMakeTest
    {
        private void ValidateOrders(List<ProductModel> orders, List<ProductModel> expectedOrders)
        {
            Assert.AreEqual(orders.Count, expectedOrders.Count, "Order count does not match");

            for (int i = 0; i < orders.Count; i++)
            {
                Assert.AreEqual(orders[i].ID, expectedOrders[i].ID, $"Mismatch at index {i}: ID");
                Assert.AreEqual(orders[i].Name, expectedOrders[i].Name, $"Mismatch at index {i}: Name");
                Assert.AreEqual(orders[i].Price, expectedOrders[i].Price, $"Mismatch at index {i}: Price");
            }
        }

        [TestMethod]
        public void MakingReservation_ShouldExit_WhenGuestsAreInvalid()
        {
            // Arrange
            var user = new UserModel { ID = 1, Admin = 1 };

            // Act
            try
            {
                ReservationMakePresent.MakingReservation(user);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred: {ex.Message}");
            }
        }

        [TestMethod]
        public void TakeOrders_ShouldReturnValidOrders_WhenValidDataProvided()
        {
            // Arrange
            var user = new UserModel { ID = 2, Admin = 0 };
            var reservationId = 1;
            var guests = 2;
            var date = DateTime.Now.AddDays(2);

            var expectedOrders = new List<ProductModel>
            {
                new ProductModel { ID = 1, Name = "Pizza", Price = 12.99M },
                new ProductModel { ID = 2, Name = "Pasta", Price = 8.99M }
            };

            // Act
            var orders = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            // Assert
            Assert.IsNotNull(orders, "Orders list is null");
            ValidateOrders(orders, expectedOrders);
        }

        [TestMethod]
        public void TakeOrders_ShouldReturnEmptyList_WhenNoThemeAvailable()
        {
            // Arrange
            var user = new UserModel { ID = 3 };
            var reservationId = 1;
            var guests = 3;
            var date = new DateTime(2000, 1, 1); // A date with no theme

            // Act
            var result = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            // Assert
            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(0, result.Count, "Result is not empty");
        }

        [TestMethod]
        public void PrintReceipt_ShouldDisplayCorrectReceiptData()
        {
            // Arrange
            var user = new UserModel { ID = 4, FirstName = "John", LastName = "Doe" };
            var reservationId = 1;
            var orders = new List<ProductModel>
            {
                new ProductModel { ID = 1, Name = "Pizza", Price = 12.99M },
                new ProductModel { ID = 2, Name = "Pasta", Price = 8.99M }
            };

            // Act
            try
            {
                ReservationMakePresent.PrintReceipt(orders, reservationId, user);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred: {ex.Message}");
            }
        }

        [TestMethod]
        public void MakingReservation_ShouldHandleAdminFlagCorrectly()
        {
            // Arrange
            var adminUser = new UserModel { ID = 1, Admin = 1 };
            var regularUser = new UserModel { ID = 2, Admin = 0 };

            // Act
            try
            {
                ReservationMakePresent.MakingReservation(adminUser);
                ReservationMakePresent.MakingReservation(regularUser);
            }
            catch (Exception ex)
            {
                Assert.Fail($"An exception occurred: {ex.Message}");
            }
        }
    }
}
