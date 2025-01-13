namespace App.Tests.User;

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
            var mockUsers = new List<UserModel_Convert>
            {
                new UserModel_Convert { FirstName = "Jane", LastName = "Doe", Admin = 0 }, // Non-admin user
                new UserModel_Convert { FirstName = "John", LastName = "Doe", Admin = 1 } // Already an admin
            };

            var userAccess = new MockDataAccess_Convert(mockUsers);

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
    public class MockDataAccess_Convert
    {
        private readonly List<UserModel_Convert> _users;

        public MockDataAccess_Convert(List<UserModel_Convert> users)
        {
            _users = users;
        }

        public IEnumerable<UserModel_Convert> GetAllBy(string columnName, string value)
        {
            return columnName switch
            {
                "FirstName" => _users.Where(u => u.FirstName == value),
                "LastName" => _users.Where(u => u.LastName == value),
                _ => Enumerable.Empty<UserModel_Convert>()
            };
        }

        public bool Update(UserModel_Convert user)
        {
            var existingUser = _users.FirstOrDefault(u => u.FirstName == user.FirstName && u.LastName == user.LastName);
            if (existingUser == null) return false;

            existingUser.Admin = user.Admin;
            return true;
        }
    }

    // Unique User model class for testing purposes
    public class UserModel_Convert
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Admin { get; set; } // 0 = Not Admin, 1 = Admin
    }
}
