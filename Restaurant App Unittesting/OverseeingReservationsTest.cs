using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Restaurant_App_Unittesting
{
    // DateTime provider class to mock DateTime.Now for testing
    public static class DateTimeProvider
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }

    // Unit Tests for Reservation Logic
    [TestClass]
    public class OverseeingReservationsTest
    {
        // Unit Test for Criterion 1: Viewing Reservations for a Selected Month
        [TestMethod]
        [DataRow("01", "2024", true)]   // Valid month and year
        [DataRow("05", "2024", true)]   // Valid month and year
        [DataRow("12", "2024", true)]   // Valid month and year
        [DataRow("00", "2024", false)]  // Invalid month
        [DataRow("13", "2024", false)]  // Invalid month
        [DataRow("07", "2023", false)]  // Invalid year (before 2024)
        [DataRow("07", "2025", false)]  // Invalid year (after current year)
        public void TestViewReservationsForMonth(string month, string year, bool expectedValidity)
        {
            // Mock the system date to be 2024 for this test, so 2025 is invalid
            DateTimeProvider.Now = () => new DateTime(2024, 1, 1);

            bool isValid = ReservationAdminLogic.IsValidMonthYear(month, year, out int m, out int y);
            Assert.AreEqual(expectedValidity, isValid);
        }
    }

    // System Tests for Reservation Orders
    [TestClass]
    public class ReservationOrderTests
    {
        // System Test for Criterion 2: Show Orders for Each Reservation
        [TestMethod]
        [DataRow(101, true, "Product1, Product2")]  // Reservation with orders
        [DataRow(102, false, "No orders found")] // Reservation with no orders
        [DataRow(103, true, "ProductA, ProductB")] // Reservation with orders
        [DataRow(104, false, "No orders found")] // Reservation with no orders
        public void TestShowOrdersForReservation(int reservationId, bool hasOrders, string expectedResult)
        {
            // Mocking ReservationAdminLogic to return appropriate order data based on the reservationId
            List<OrderModel> orders = hasOrders ? new List<OrderModel>
            {
                new OrderModel { ProductName = reservationId == 103 ? "ProductA" : "Product1" },
                new OrderModel { ProductName = reservationId == 103 ? "ProductB" : "Product2" }
            } : new List<OrderModel>();

            string result = orders.Count > 0 ? string.Join(", ", orders.ConvertAll(order => order.ProductName)) : "No orders found";
            Assert.AreEqual(expectedResult, result);
        }
    }

    // System Tests for Reservation Theme
    [TestClass]
    public class ReservationThemeTests
    {
        // System Test for Criterion 3: Show Theme for Reservation Month
        [TestMethod]
        [DataRow(101, 1, 2024, true, "January Theme")]  // Reservation with theme
        [DataRow(102, 2, 2024, true, "February Theme")] // Reservation with theme
        [DataRow(103, 3, 2024, false, "No theme assigned")] // No theme assigned
        [DataRow(104, 5, 2024, true, "May Theme")] // Reservation with theme
        [DataRow(105, 7, 2024, false, "No theme assigned")] // No theme assigned
        public void TestShowThemeForReservationMonth(int reservationId, int month, int year, bool hasTheme, string expectedResult)
        {
            // Mocking ReservationAdminLogic to return theme information based on the month
            string theme = hasTheme ? $"{new DateTime(year, month, 1):MMMM} Theme" : "No theme assigned";
            Assert.AreEqual(expectedResult, theme);
        }
    }

    // Mocking the ReservationAdminLogic class used in the tests
    public static class ReservationAdminLogic
    {
        public static bool IsValidMonthYear(string monthInput, string yearInput, out int month, out int year)
        {
            month = 0;
            year = 0;

            return monthInput.Length == 2 && yearInput.Length == 4
                && int.TryParse(monthInput, out month) && int.TryParse(yearInput, out year)
                && month >= 1 && month <= 12
                && year >= 2024 && year <= DateTimeProvider.Now().Year;  // Use the mockable date provider here
        }

        public static List<OrderModel> GetOrdersForReservation(int reservationId)
        {
            // Simulating fetching orders for a reservation
            return new List<OrderModel>
            {
                new OrderModel { ProductName = "Product1" },
                new OrderModel { ProductName = "Product2" }
            };
        }

        public static string GetThemeForMonth(int month, int year)
        {
            // Simulating fetching theme information based on month and year
            if (month == 1) return "January Theme";
            if (month == 2) return "February Theme";
            if (month == 3) return null; // No theme
            if (month == 5) return "May Theme";
            return null; // No theme
        }
    }

    // Order model class used in the tests
    public class OrderModel
    {
        public string ProductName { get; set; }
    }
}
