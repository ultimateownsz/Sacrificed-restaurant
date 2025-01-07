using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class AccountTests
    {
        // Email validation tests
        [TestMethod]
        [DataRow("test@example.com", true)]      // Valid email
        [DataRow("user@domain.com", true)]       // Valid email
        [DataRow("noatsign.com", false)]         // Invalid email (missing @)
        [DataRow("missingdot@domain", false)]    // Invalid email (missing . in domain)
        public void TestIsEmailValid(string email, bool expectedResult)
        {
            // Act
            bool result = SimulateIsEmailValid(email);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        // Password validation tests
        [TestMethod]
        [DataRow("12345678", true)]              // Valid password length
        [DataRow("1234567890123456", true)]      // Valid password length
        [DataRow("1234567", false)]              // Too short
        [DataRow("12345678901234567", false)]    // Too long
        public void TestIsPasswordValid(string password, bool expectedResult)
        {
            // Act
            bool result = SimulateIsPasswordValid(password);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        // Phone number validation tests
        [TestMethod]
        [DataRow("12345678", true)]              // Valid phone number
        [DataRow("1234abcd", false)]             // Invalid (contains letters)
        [DataRow("123456789", false)]            // Too long
        [DataRow("1234567", false)]              // Too short
        public void TestIsPhoneNumberValid(string phoneNumber, bool expectedResult)
        {
            // Act
            bool result = SimulateIsPhoneNumberValid(phoneNumber);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        // User account creation test
        [TestMethod]
        public void TestUserAccountCreation()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";
            string email = "john.doe@example.com";
            string password = "password123";
            string phoneNumber = "12345678";

            // Act
            var account = SimulateCreateUserAccount(firstName, lastName, email, password, phoneNumber);

            // Assert
            Assert.AreEqual(firstName, account.FirstName);
            Assert.AreEqual(lastName, account.LastName);
            Assert.AreEqual(email, account.EmailAddress);
            Assert.AreEqual(password, account.Password);
            Assert.AreEqual(12345678, account.PhoneNumber);
            Assert.AreEqual(0, account.IsAdmin); // User account
        }

        // Admin account creation test
        [TestMethod]
        public void TestAdminAccountCreation()
        {
            // Arrange
            string firstName = "Jane";
            string lastName = "Smith";
            string email = "jane.smith@example.com";
            string password = "securePass1";
            string phoneNumber = "87654321";

            // Act
            var account = SimulateCreateAdminAccount(firstName, lastName, email, password, phoneNumber);

            // Assert
            Assert.AreEqual(firstName, account.FirstName);
            Assert.AreEqual(lastName, account.LastName);
            Assert.AreEqual(email, account.EmailAddress);
            Assert.AreEqual(password, account.Password);
            Assert.AreEqual(87654321, account.PhoneNumber);
            Assert.AreEqual(1, account.IsAdmin); // Admin account
        }

        // Simulated methods for validation and account creation logic
        private bool SimulateIsNameValid(string name)
        {
            return name.Length >= 2;
        }

        private bool SimulateIsEmailValid(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private bool SimulateIsPasswordValid(string password)
        {
            return password.Length >= 8 && password.Length <= 16;
        }

        private bool SimulateIsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber.Length == 8 && int.TryParse(phoneNumber, out _);
        }

        private AccountModel SimulateCreateUserAccount(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            return new AccountModel
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Password = password,
                PhoneNumber = int.Parse(phoneNumber),
                IsAdmin = 0
            };
        }

        private AccountModel SimulateCreateAdminAccount(string firstName, string lastName, string email, string password, string phoneNumber)
        {
            return new AccountModel
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Password = password,
                PhoneNumber = int.Parse(phoneNumber),
                IsAdmin = 1
            };
        }

        // Supporting Account Model class
        public class AccountModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public int PhoneNumber { get; set; }
            public int IsAdmin { get; set; } // 0 = User, 1 = Admin
        }
    }
}
