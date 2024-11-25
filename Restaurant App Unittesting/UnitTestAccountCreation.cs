// namespace Restaurant_App_Unittesting;

// [TestClass]
// public class AccountTests
// {
//     private AccountLogic accountService;

//     [TestInitialize]
//     public void Setup()
//     {
//         accountService = new AccountLogic();
//     }

//     // Test for email validation
//     [TestMethod]
//     public void TestIsEmailValid_ValidEmail_ReturnsTrue()
//     {
//         Assert.IsTrue(accountService.IsEmailValid("test@example.com"));
//         Assert.IsTrue(accountService.IsEmailValid("user@domain.com"));
//     }

//     [TestMethod]
//     public void TestIsEmailValid_InvalidEmail_ReturnsFalse()
//     {
//         Assert.IsFalse(accountService.IsEmailValid("noatsign.com"));
//         Assert.IsFalse(accountService.IsEmailValid("missingdot@domain"));
//     }

//     // Test for password validation
//     [TestMethod]
//     public void TestIsPasswordValid_ValidPasswordLength_ReturnsTrue()
//     {
//         Assert.IsTrue(accountService.IsPasswordValid("12345678"));
//         Assert.IsTrue(accountService.IsPasswordValid("1234567890123456"));
//     }

//     [TestMethod]
//     public void TestIsPasswordValid_InvalidPasswordLength_ReturnsFalse()
//     {
//         Assert.IsFalse(accountService.IsPasswordValid("1234567"));  // Too short
//         Assert.IsFalse(accountService.IsPasswordValid("12345678901234567"));  // Too long
//     }

//     // Test for phone number validation
//     [TestMethod]
//     public void TestIsPhoneNumberValid_ValidPhoneNumber_ReturnsTrue()
//     {
//         Assert.IsTrue(accountService.IsPhoneNumberValid("12345678"));
//     }

//     [TestMethod]
//     public void TestIsPhoneNumberValid_InvalidPhoneNumber_ReturnsFalse()
//     {
//         Assert.IsFalse(accountService.IsPhoneNumberValid("1234abcd"));  // Not a number
//         Assert.IsFalse(accountService.IsPhoneNumberValid("123456789"));  // Too long
//         Assert.IsFalse(accountService.IsPhoneNumberValid("1234567"));  // Too short
//     }

//     // Test for UserAccount creation
//     [TestMethod]
//     public void TestUserAccountCreation_CreatesUserAccountWithCorrectDetails()
//     {
//         var account = accountService.UserAccount("John", "Doe", "john.doe@example.com", "password123", "12345678");

//         Assert.AreEqual("John", account.FirstName);
//         Assert.AreEqual("Doe", account.LastName);
//         Assert.AreEqual("john.doe@example.com", account.EmailAddress);
//         Assert.AreEqual("password123", account.Password);
//         Assert.AreEqual(12345678, account.PhoneNumber);
//         Assert.AreEqual(0, account.IsAdmin);  // Ensure it’s a user account (non-admin)
//     }

//     // Test for AdminAccount creation
//     [TestMethod]
//     public void TestAdminAccountCreation_CreatesAdminAccountWithCorrectDetails()
//     {
//         var account = accountService.AdminAccount("Jane", "Smith", "jane.smith@example.com", "securePass1", "87654321");

//         Assert.AreEqual("Jane", account.FirstName);
//         Assert.AreEqual("Smith", account.LastName);
//         Assert.AreEqual("jane.smith@example.com", account.EmailAddress);
//         Assert.AreEqual("securePass1", account.Password);
//         Assert.AreEqual(87654321, account.PhoneNumber);
//         Assert.AreEqual(1, account.IsAdmin);  // Ensure it’s an admin account
//     }
// }
