using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class AccountTests
    {
        private AccountLogic accountService;

        [TestInitialize]
        public void Setup()
        {
            accountService = new AccountLogic();
        }

        // Test for email validation
        [TestMethod]
        [DataRow("test@example.com", true)]       // Valid email
        [DataRow("user@domain.com", true)]       // Valid email
        [DataRow("noatsign.com", false)]         // Invalid email (missing @)
        [DataRow("missingdot@domain", false)]    // Invalid email (missing . in domain)
        public void TestIsEmailValid(string email, bool expectedResult)
        {
            bool result = accountService.IsEmailValid(email);
            Assert.AreEqual(expectedResult, result);
        }

        // Test for password validation
        [TestMethod]
        [DataRow("12345678", true)]              // Valid password length
        [DataRow("1234567890123456", true)]      // Valid password length
        [DataRow("1234567", false)]              // Too short
        [DataRow("12345678901234567", false)]    // Too long
        public void TestIsPasswordValid(string password, bool expectedResult)
        {
            bool result = accountService.IsPasswordValid(password);
            Assert.AreEqual(expectedResult, result);
        }

        // Test for phone number validation
        [TestMethod]
        [DataRow("12345678", true)]              // Valid phone number
        [DataRow("1234abcd", false)]             // Invalid (contains letters)
        [DataRow("123456789", false)]            // Too long
        [DataRow("1234567", false)]              // Too short
        public void TestIsPhoneNumberValid(string phoneNumber, bool expectedResult)
        {
            bool result = accountService.IsPhoneNumberValid(phoneNumber);
            Assert.AreEqual(expectedResult, result);
        }

        // Test for UserAccount creation
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
            var account = accountService.UserAccount(firstName, lastName, email, password, phoneNumber);

            // Assert
            Assert.AreEqual(firstName, account.FirstName);
            Assert.AreEqual(lastName, account.LastName);
            Assert.AreEqual(email, account.EmailAddress);
            Assert.AreEqual(password, account.Password);
            Assert.AreEqual(12345678, account.PhoneNumber);
            Assert.AreEqual(0, account.IsAdmin);  // Ensure it’s a user account (non-admin)
        }

        // Test for AdminAccount creation
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
            var account = accountService.AdminAccount(firstName, lastName, email, password, phoneNumber);

            // Assert
            Assert.AreEqual(firstName, account.FirstName);
            Assert.AreEqual(lastName, account.LastName);
            Assert.AreEqual(email, account.EmailAddress);
            Assert.AreEqual(password, account.Password);
            Assert.AreEqual(87654321, account.PhoneNumber);
            Assert.AreEqual(1, account.IsAdmin);  // Ensure it’s an admin account
        }
    }
}
