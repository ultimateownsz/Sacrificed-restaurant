using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant_App_Unittesting
{
    // Unit Test for Criteria 1: Past Reservations are Saved
    [TestClass]
    public class PastReservations
    {
        [TestMethod]
        [DataRow(1, "John", "Doe", "password123", 1)] // User exists and past reservations are intact
        [DataRow(2, "Piet", "Pieter", "password123", 0)] // User does not exist
    }

    // Unit Test for Criteria 4: Personal Information is Made Anonymous
    [TestClass]
    public class AnonymizeInfo
    {

    }

    // Unit Test for Criteria 5: Ask User for Password Confirmation When Deleting Account
    [TestClass]
    public class PasswordConfirmation
    {

    }

    // Mocking data access layer for testing users
    public class MockDataAccess
    {
        private readonly List<UserModel> _users;

        public MockDataAccess(List<UserModel> users)
        {
            _users = users;
        }

        public IEnumerable<UserModel> GetAllBy(string columnName, string value)
        {
            return columnName switch
            {
                "ID" => _users.Where(u => u.ID.ToString() == value),
                _ => Enumerable.Empty<UserModel>()
            };
        }

        public bool Update(UserModel user)
        {
            var existingUser = _users.FirstOrDefault(u => u.ID == user.ID);
            if (existingUser == null) return false;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Password = user.Password;
            return true;
        }
    }

    // Mocking data access layer for testing reservations
    public class MockReservationAccess
    {
        private readonly List<Reservation> _reservations;

        public MockReservationAccess(List<Reservation> reservations)
        {
            _reservations = reservations;
        }

        public IEnumerable<Reservation> GetAllBy(string columnName, string value)
        {
            return columnName switch
            {
                "UserID" => _reservations.Where(r => r.UserID.ToString() == value),
                _ => Enumerable.Empty<Reservation>()
            };
        }
    }

     // User model class
    public class UserModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    // Reservation model class
    public class Reservation
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }
    }
}