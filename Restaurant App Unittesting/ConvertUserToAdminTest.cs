using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant_App_Unittesting
{
    // Unit Test for Criteria 5: Database Update
    [TestClass]
    public class ConvertUserToAdminTest
    {
        [TestMethod]
        [DataRow("John", "Doe", false, 1)] // User exists but is already an admin
        [DataRow("Jane", "Doe", true, 1)] // User exists and is promoted
        [DataRow("Piet", "Pieter", false, 0)] // User does not exist
        public void TestUpdateDatabaseForPromotion(string firstName, string lastName, bool expectedSuccess, int expectedAdminStatus)
        {
            // Mocking database of users
            var mockUsers = new List<UserModel>
            {
                new UserModel { FirstName = "Jane", LastName = "Doe", Admin = 0 }, // Non-admin user
                new UserModel { FirstName = "John", LastName = "Doe", Admin = 1 } // Already an admin
            };

            var userAccess = new MockDataAccess(mockUsers);

            // Act: simulate fetching and updating the user
            var user = userAccess.GetAllBy("FirstName", firstName)
                .FirstOrDefault(u => string.Equals(u.LastName, lastName, System.StringComparison.OrdinalIgnoreCase));
            
            bool result = false;
            if (user != null && user.Admin == 0)
            {
                user.Admin = 1; // Promote to admin
                result = userAccess.Update(user);
            }

            int actualAdminStatus = mockUsers.FirstOrDefault(u => u.FirstName == firstName && u.LastName == lastName)?.Admin ?? 0;

            // Assert: verify the outcome
            Assert.AreEqual(expectedSuccess, result);
            Assert.AreEqual(expectedAdminStatus, actualAdminStatus);
        }
    }

    // Mocking data access layer for testing
    public class MockDataAccess_ConvertUser
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
                "FirstName" => _users.Where(u => u.FirstName == value),
                "LastName" => _users.Where(u => u.LastName == value),
                _ => Enumerable.Empty<UserModel>()
            };
        }

        public bool Update(UserModel user)
        {
            var existingUser = _users.FirstOrDefault(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
            if (existingUser == null) return false;

            existingUser.Admin = user.Admin;
            return true;
        }
    }

    // User model class
    public class UserModel_ConvertUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Admin { get; set; } // 0 = Not Admin, 1 = Admin
    }
}