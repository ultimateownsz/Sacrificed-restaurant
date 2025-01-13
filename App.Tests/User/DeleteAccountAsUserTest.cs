namespace App.Tests.User;
{
    // Unit Test for Criteria 1: Past Reservations are Saved
    [TestClass]
    public class PastReservations_DeleteAccountUser
    {
        [TestMethod]
        [DataRow(1, "John", "Doe", "password123", 2)] // User exists and past reservations are intact
        [DataRow(2, "Piet", "Pieter", "password123", 0)] // User does not exist
        public void TestPastReservationsAreSaved(int userId, string firstName, string lastName, string password, int expectedPastReservationCount)
        {
            // Mocking database of users and reservations
            var mockUsers = new List<UserModel_DeleteAccountUser>
            {
                new UserModel_DeleteAccountUser { ID = 1, FirstName = "John", LastName = "Doe", Password = "password123" },
                new UserModel_DeleteAccountUser { ID = 2, FirstName = "Piet", LastName = "Pieter", Password = "password123" }
            };

            var mockReservations = new List<Reservation_DeleteAccountUser>
            {
                new Reservation_DeleteAccountUser { UserID = 1, Date = DateTime.Now.AddDays(-1) },
                new Reservation_DeleteAccountUser { UserID = 1, Date = DateTime.Now.AddDays(-2) }
            };

            var userAccess = new MockDataAccess_DeleteAccountUser(mockUsers);
            var reservationAccess = new MockReservationAccess_DeleteAccountUser(mockReservations);

            // Act: Simulate user deletion
            var user = userAccess.GetAllBy("ID", userId.ToString()).FirstOrDefault();
            bool result = false;
            int initialReservationCount = reservationAccess.GetAllBy("UserID", userId.ToString()).Count();

            if (user != null && user.Password == password)
            {
                // Simulating the ConfirmAndDelete logic within the test
                result = ConfirmAndDelete(user);  // Should not modify reservations
            }

            // Assert: Ensure past reservations are saved (count should remain the same)
            int actualReservationCount = reservationAccess.GetAllBy("UserID", userId.ToString()).Count();
            Assert.AreEqual(expectedPastReservationCount, actualReservationCount);  // Ensure reservations stay intact
        }

        // Simulated ConfirmAndDelete logic
        private bool ConfirmAndDelete(UserModel_DeleteAccountUser user)
        {
            if (user == null) return false;
            // In reality, you would delete the user here, but we only anonymize info in this test.
            user.FirstName = "Inactive";
            user.LastName = "Inactive";
            
            // Ensure reservations are not deleted or altered in this test
            return true;
        }
    }

    // Unit Test for Criteria 4: Personal Information is Made Anonymous
    [TestClass]
    public class AnonymizeInfo_DeleteAccountUser
    {
        [TestMethod]
        [DataRow(1, "John", "Doe", "password123", "Inactive")] // User info should be anonymized
        public void TestPersonalInformationIsMadeAnonymous(int userId, string firstName, string lastName, string password, string expectedAnonymizedInfo)
        {
            // Mocking database of users
            var mockUsers = new List<UserModel_DeleteAccountUser>
            {
                new UserModel_DeleteAccountUser { ID = 1, FirstName = "John", LastName = "Doe", Password = "password123" }
            };

            var userAccess = new MockDataAccess_DeleteAccountUser(mockUsers);

            // Act: Simulate user deletion
            var user = userAccess.GetAllBy("ID", userId.ToString()).FirstOrDefault();
            bool result = false;

            if (user != null && user.Password == password)
            {
                // Simulating the ConfirmAndDelete logic within the test
                result = ConfirmAndDelete(user);
            }

            // Assert: Ensure personal information is anonymized
            Assert.AreEqual(expectedAnonymizedInfo, user.FirstName);
            Assert.AreEqual(expectedAnonymizedInfo, user.LastName);
        }

        // Simulated ConfirmAndDelete logic
        private bool ConfirmAndDelete(UserModel_DeleteAccountUser user)
        {
            if (user == null) return false;
            // Anonymize user information
            user.FirstName = "Inactive";
            user.LastName = "Inactive";
            return true;
        }
    }

    // Unit Test for Criteria 5: Ask User for Password Confirmation When Deleting Account
    [TestClass]
    public class PasswordConfirmation_DeleteAccountUser
    {
        [TestMethod]
        [DataRow("password123", true)] // Correct password
        [DataRow("wrongpassword", false)] // Incorrect password
        public void TestPasswordConfirmation(string enteredPassword, bool expectedResult)
        {
            // Mocking user data
            var user = new UserModel_DeleteAccountUser { ID = 1, FirstName = "John", LastName = "Doe", Password = "password123" };

            // Act: Simulate password confirmation process
            bool result = DeleteAccount(user, enteredPassword);

            // Assert: Verify password confirmation result
            Assert.AreEqual(expectedResult, result);

            if (expectedResult) 
            {
                // Additional assert to check if user information was anonymized
                Assert.AreEqual("Inactive", user.FirstName);
                Assert.AreEqual("Inactive", user.LastName);
            }
        }

        // Simulated DeleteAccount logic
        private bool DeleteAccount(UserModel_DeleteAccountUser user, string enteredPassword)
        {
            if (user.Password == enteredPassword)
            {
                // Simulating deletion logic
                user.FirstName = "Inactive";
                user.LastName = "Inactive";
                return true;
            }
            return false;
        }
    }

    // Mocking data access layer for testing users
    public class MockDataAccess_DeleteAccountUser
    {
        private readonly List<UserModel_DeleteAccountUser> _users;

        public MockDataAccess_DeleteAccountUser(List<UserModel_DeleteAccountUser> users)
        {
            _users = users;
        }

        public IEnumerable<UserModel_DeleteAccountUser> GetAllBy(string columnName, string value)
        {
            return columnName switch
            {
                "ID" => _users.Where(u => u.ID.ToString() == value),
                _ => Enumerable.Empty<UserModel_DeleteAccountUser>()
            };
        }

        public bool Update(UserModel_DeleteAccountUser user)
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
    public class MockReservationAccess_DeleteAccountUser
    {
        private readonly List<Reservation_DeleteAccountUser> _reservations;

        public MockReservationAccess_DeleteAccountUser(List<Reservation_DeleteAccountUser> reservations)
        {
            _reservations = reservations;
        }

        public IEnumerable<Reservation_DeleteAccountUser> GetAllBy(string columnName, string value)
        {
            return columnName switch
            {
                "UserID" => _reservations.Where(r => r.UserID.ToString() == value),
                _ => Enumerable.Empty<Reservation_DeleteAccountUser>()
            };
        }
    }

    // User model class
    public class UserModel_DeleteAccountUser
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }

    // Reservation model class
    public class Reservation_DeleteAccountUser
    {
        public int UserID { get; set; }
        public DateTime Date { get; set; }
    }
}
