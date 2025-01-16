using App.Logic;
using App.DataModels.Product;
using App.DataAccess.Utils;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using System.Linq;


namespace App.Restaurant
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TestIsEmailValid_ValidEmail()
        {
            var result = LoginLogic.IsEmailValid("test@example.com");
            Assert.IsTrue(result.isValid, "The email validation should pass for a valid email address.");
            Assert.IsNull(result.message, "The validation message should be null for a valid email.");
        }

        [TestMethod]
        public void TestIsEmailValid_InvalidFormat()
        {
            var result = LoginLogic.IsEmailValid("invalid-email");
            Assert.IsFalse(result.isValid, "The email validation should fail for an invalid email format.");
            Assert.AreEqual("\nThe email format is invalid. Please enter a valid email address (e.g., example@domain.com).\n\n", result.message);
        }

        [TestMethod]
        public void TestIsEmailValid_EmailAlreadyExists()
        {
            // Simulate existing user in database
            Access.Users.Add(new UserModel { Email = "existing@example.com" });
            
            var result = LoginLogic.IsEmailValid("existing@example.com");
            Assert.IsFalse(result.isValid, "The email validation should fail if the email already exists.");
            Assert.AreEqual("\nThis email already exists. Please use a different email.\n\n", result.message);
        }

        [TestMethod]
        public void TestIsPasswordValid_ValidPassword()
        {
            var result = LoginLogic.IsPasswordValid("Passw0rd");
            Assert.IsTrue(result.isValid, "The password validation should pass for a valid password.");
            Assert.IsNull(result.error, "The validation error should be null for a valid password.");
        }

        [TestMethod]
        public void TestIsPasswordValid_EmptyPassword()
        {
            var result = LoginLogic.IsPasswordValid("");
            Assert.IsFalse(result.isValid, "The password validation should fail for an empty password.");
            Assert.AreEqual("\nPassword cannot be empty. Please try again.\n\n", result.error);
        }

        [TestMethod]
        public void TestIsPasswordValid_MissingLetter()
        {
            var result = LoginLogic.IsPasswordValid("12345678");
            Assert.IsFalse(result.isValid, "The password validation should fail if no letters are included.");
            Assert.AreEqual("\nPassword must contain at least one letter. Please try again.\n\n", result.error);
        }

        [TestMethod]
        public void TestIsPasswordValid_MissingNumber()
        {
            var result = LoginLogic.IsPasswordValid("Password");
            Assert.IsFalse(result.isValid, "The password validation should fail if no numbers are included.");
            Assert.AreEqual("\nPassword must contain at least one number. Please try again.\n\n", result.error);
        }

        [TestMethod]
        public void TestIsPhoneNumberValid_ValidPhoneNumber()
        {
            var result = LoginLogic.IsPhoneNumberValid("0612345678");
            Assert.IsTrue(result.isValid, "The phone number validation should pass for a valid phone number.");
            Assert.IsNull(result.error, "The validation error should be null for a valid phone number.");
        }

        [TestMethod]
        public void TestIsPhoneNumberValid_InvalidLength()
        {
            var result = LoginLogic.IsPhoneNumberValid("06123");
            Assert.IsFalse(result.isValid, "The phone number validation should fail if the number is not 10 digits long.");
            Assert.AreEqual("\nPhone number must be exactly 10 digits long. Please try again.\n\n", result.error);
        }

        [TestMethod]
        public void TestIsPhoneNumberValid_InvalidStart()
        {
            var result = LoginLogic.IsPhoneNumberValid("0712345678");
            Assert.IsFalse(result.isValid, "The phone number validation should fail if the number does not start with '06'.");
            Assert.AreEqual("\nPhone number must start with '06'. Please try again.\n\n", result.error);
        }

        [TestMethod]
        public void TestUserAccountCreation()
        {
            var user = LoginLogic.UserAccount("John", "Doe", "john.doe@example.com", "Passw0rd", "0612345678");
            Assert.AreEqual("John", user.FirstName);
            Assert.AreEqual("Doe", user.LastName);
            Assert.AreEqual("john.doe@example.com", user.Email);
            Assert.AreEqual("Passw0rd", user.Password);
            Assert.AreEqual("0612345678", user.Phone);
            Assert.AreEqual(0, user.Admin, "The Admin property should be set to 0 for a normal user.");
        }

        [TestMethod]
        public void TestAdminAccountCreation()
        {
            var admin = LoginLogic.AdminAccount("Admin", "User", "admin.user@example.com", "Admin123", "0612345678");
            Assert.AreEqual("Admin", admin.FirstName);
            Assert.AreEqual("User", admin.LastName);
            Assert.AreEqual("admin.user@example.com", admin.Email);
            Assert.AreEqual("Admin123", admin.Password);
            Assert.AreEqual("0612345678", admin.Phone);
            Assert.AreEqual(1, admin.Admin, "The Admin property should be set to 1 for an admin user.");
        }
    }
}

// [TestClass]
// public class AccountCreateTest
// {
//     // First name validation tests
//     [TestMethod]
//     [DataRow("Jo", true)]                   // Valid first name
//     [DataRow("Liam", true)]                 // Valid first name
//     [DataRow("", false)]                    // Invalid first name, name has no characters
//     [DataRow("A", false)]                   // Invalid first name, it has only one character
//     public void TestIsFirstNameValid(string firstName, bool expectedResult)
//     {
//         bool result = SimulateIsNameValid(firstName);

//         Assert.AreEqual(expectedResult, result);
//     }

//     // Last name validation tests
//     [TestMethod]
//     [DataRow("Li", true)]                   // Valid last name
//     [DataRow("Wayne", true)]                // Valid last name
//     [DataRow("", false)]                    // Invalid last name, name has no characters
//     [DataRow("B", false)]                   // Invalid last name, name has only one character
//     public void TestIsLastNameValid(string lastName, bool expectedResult)
//     {
//         bool result = SimulateIsNameValid(lastName);

//         Assert.AreEqual(expectedResult, result);
//     }

//     // Email validation tests
//     [TestMethod]
//     [DataRow("test@example.com", true)]      // Valid email
//     [DataRow("user@domain.com", true)]       // Valid email
//     [DataRow("noatsign.com", false)]         // Invalid email (missing @)
//     [DataRow("missingdot@domain", false)]    // Invalid email (missing . in domain)
//     public void TestIsEmailValid(string email, bool expectedResult)
//     {
//         // Act
//         bool result = SimulateIsEmailValid(email);

//         // Assert
//         Assert.AreEqual(expectedResult, result);
//     }

//     // Password validation tests
//     [TestMethod]
//     [DataRow("12345678", true)]              // Valid password length
//     [DataRow("1234567890123456", true)]      // Valid password length
//     [DataRow("1234567", false)]              // Too short
//     [DataRow("12345678901234567", false)]    // Too long
//     public void TestIsPasswordValid(string password, bool expectedResult)
//     {
//         // Act
//         bool result = SimulateIsPasswordValid(password);

//         // Assert
//         Assert.AreEqual(expectedResult, result);
//     }

//     // Phone number validation tests
//     [TestMethod]
//     [DataRow("12345678", true)]              // Valid phone number
//     [DataRow("1234abcd", false)]             // Invalid (contains letters)
//     [DataRow("123456789", false)]            // Too long
//     [DataRow("1234567", false)]              // Too short
//     public void TestIsPhoneNumberValid(string phoneNumber, bool expectedResult)
//     {
//         // Act
//         bool result = SimulateIsPhoneNumberValid(phoneNumber);

//         // Assert
//         Assert.AreEqual(expectedResult, result);
//     }

//     // User account creation test
//     [TestMethod]
//     public void TestUserAccountCreation()
//     {
//         // Arrange
//         string firstName = "John";
//         string lastName = "Doe";
//         string email = "john.doe@example.com";
//         string password = "password123";
//         string phoneNumber = "12345678";

//         // Act
//         var account = SimulateCreateUserAccount(firstName, lastName, email, password, phoneNumber);

//         // Assert
//         Assert.AreEqual(firstName, account.FirstName);
//         Assert.AreEqual(lastName, account.LastName);
//         Assert.AreEqual(email, account.EmailAddress);
//         Assert.AreEqual(password, account.Password);
//         Assert.AreEqual(12345678, account.PhoneNumber);
//         Assert.AreEqual(0, account.IsAdmin); // User account
//     }

//     // Admin account creation test
//     [TestMethod]
//     public void TestAdminAccountCreation()
//     {
//         // Arrange
//         string firstName = "Jane";
//         string lastName = "Smith";
//         string email = "jane.smith@example.com";
//         string password = "securePass1";
//         string phoneNumber = "87654321";

//         // Act
//         var account = SimulateCreateAdminAccount(firstName, lastName, email, password, phoneNumber);

//         // Assert
//         Assert.AreEqual(firstName, account.FirstName);
//         Assert.AreEqual(lastName, account.LastName);
//         Assert.AreEqual(email, account.EmailAddress);
//         Assert.AreEqual(password, account.Password);
//         Assert.AreEqual(87654321, account.PhoneNumber);
//         Assert.AreEqual(1, account.IsAdmin); // Admin account
//     }

//     // Simulated methods for validation and account creation logic

//     private bool SimulateIsEmailValid(string email)
//     {
//         return email.Contains("@") && email.Contains(".");
//     }

//     private bool SimulateIsPasswordValid(string password)
//     {
//         return password.Length >= 8 && password.Length <= 16;
//     }

//     private bool SimulateIsPhoneNumberValid(string phoneNumber)
//     {
//         return phoneNumber.Length == 8 && int.TryParse(phoneNumber, out _);
//     }

//     private bool SimulateIsNameValid(string name)
//     {
//         return name.Length >= 2;
//     }

//     private AccountModel SimulateCreateUserAccount(string firstName, string lastName, string email, string password, string phoneNumber)
//     {
//         return new AccountModel
//         {
//             FirstName = firstName,
//             LastName = lastName,
//             EmailAddress = email,
//             Password = password,
//             PhoneNumber = int.Parse(phoneNumber),
//             IsAdmin = 0
//         };
//     }

//     private AccountModel SimulateCreateAdminAccount(string firstName, string lastName, string email, string password, string phoneNumber)
//     {
//         return new AccountModel
//         {
//             FirstName = firstName,
//             LastName = lastName,
//             EmailAddress = email,
//             Password = password,
//             PhoneNumber = int.Parse(phoneNumber),
//             IsAdmin = 1
//         };
//     }

//     // Supporting Account Model class
//     public class AccountModel
//     {
//         public string FirstName { get; set; }
//         public string LastName { get; set; }
//         public string EmailAddress { get; set; }
//         public string Password { get; set; }
//         public int PhoneNumber { get; set; }
//         public int IsAdmin { get; set; } // 0 = User, 1 = Admin
//     }
// }
