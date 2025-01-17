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
