using App.Presentation.Reservation;
using App.DataModels.Product;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Tests
{
    [TestClass]
    public class ReservationMakeTest
    {
        // [TestMethod]
        // public void TestMakingReservation_AdminUser_ValidInputs()
        // {
        //     var user = new UserModel { Admin = 1, ID = 1 };
        //     ReservationMakePresent.MakingReservation(user);

        //     // Assertions can be added to verify the flow based on mock or actual logic.
        // }

        // [TestMethod]
        // public void TestMakingReservation_NonAdminUser_ValidInputs()
        // {
        //     var user = new UserModel { Admin = 0, ID = 2 };
        //     ReservationMakePresent.MakingReservation(user);

        //     // Assertions to ensure correct flow for non-admin users.
        // }

        [TestMethod]
        public void TestTakeOrders_ValidReservationAndTheme()
        {
            var user = new UserModel { ID = 3 };
            int reservationId = 1001;
            DateTime date = DateTime.Now;
            int guests = 2;

            List<ProductModel> orders = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            Assert.IsNotNull(orders, "Orders should not be null");
            Assert.IsTrue(orders.Count >= 0, "Orders count should be valid");
        }

        [TestMethod]
        public void TestTakeOrders_InvalidReservationId()
        {
            var user = new UserModel { ID = 3 };
            int reservationId = 0;
            DateTime date = DateTime.Now;
            int guests = 2;

            List<ProductModel> orders = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            Assert.IsNotNull(orders, "Orders should not be null");
            Assert.AreEqual(0, orders.Count, "Orders should be empty for invalid reservation ID");
        }

        // [TestMethod]
        // public void TestPrintReceipt_ValidReservation()
        // {
        //     var user = new UserModel { ID = 4, FirstName = "John", LastName = "Doe" };
        //     int reservationId = 1002;
        //     var orders = new List<ProductModel>
        //     {
        //         new ProductModel { Name = "Pasta", Price = 10.5m },
        //         new ProductModel { Name = "Salad", Price = 5.0m }
        //     };

        //     ReservationMakePresent.PrintReceipt(orders, reservationId, user);

        //     // Assertions would involve checking console output if necessary.
        // }

        // [TestMethod]
        // public void TestPrintReceipt_InvalidReservation()
        // {
        //     var user = new UserModel { ID = 5 };
        //     int reservationId = 0;
        //     var orders = new List<ProductModel>
        //     {
        //         new ProductModel { Name = "Steak", Price = 20.0m }
        //     };

        //     ReservationMakePresent.PrintReceipt(orders, reservationId, user);

        //     // Assertions to ensure error handling works.
        // }

        // [TestMethod]
        // public void TestGetUserFullName_ValidUserId()
        // {
        //     var methodInfo = typeof(ReservationMakePresent)
        //         .GetMethod("GetUserFullName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        //     Assert.IsNotNull(methodInfo, "The private method GetUserFullName could not be found.");

        //     var result = methodInfo.Invoke(null, new object[] { 6 });

        //     Assert.AreEqual("Test User", result, "Full name should match the mocked user.");
        // }


        // [TestMethod]
        // public void TestGetUserFullName_InvalidUserId()
        // {
        //     var userId = -1;
        //     string fullName = PrivateMethodInvoker.InvokePrivateMethod<string>(typeof(ReservationMakePresent), "GetUserFullName", userId);

        //     Assert.AreEqual("Unknown User", fullName, "Fallback should be 'Unknown User'");
        // }
    }
}











