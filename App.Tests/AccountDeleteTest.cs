// using App.Logic;
// // using App.DataModels.User;
// using App.DataAccess.Utils;
// using Restaurant;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace App.Restaurant
// {
//     [TestClass]
//     public class DeleteAccountTests
//     {
//         [TestMethod]
//         public void TestGetPage()
//         {
//             // Arrange
//             var accounts = Enumerable.Range(1, 25)
//                                      .Select(i => new UserModel { ID = i, FirstName = $"User{i}", LastName = $"Last{i}" })
//                                      .ToList();
//             int page = 1;
//             int itemsPerPage = 10;

//             // Act
//             var result = DeleteAccountLogic.GetPage(accounts, page, itemsPerPage);

//             // Assert
//             Assert.AreEqual(10, result.Count, "Each page should contain 10 items.");
//             Assert.AreEqual("User11", result.First().FirstName, "The first account on the second page should be User11.");
//         }

//         [TestMethod]
//         public void TestFormatAccount()
//         {
//             // Arrange
//             var user = new UserModel { FirstName = "John", LastName = "Doe", Admin = 1 };

//             // Act
//             var result = DeleteAccountLogic.FormatAccount(user);

//             // Assert
//             Assert.AreEqual("John Doe (Admin)", result, "The formatted account string should indicate the admin status.");
//         }

//         [TestMethod]
//         public void TestConfirmAndDelete_ConfirmDeletion()
//         {
//             // Arrange
//             var user = new UserModel { ID = 1, FirstName = "John", LastName = "Doe" };

//             // Mock selection logic
//             SelectionPresent.Show(options, banner: "Are you sure?") = (options, banner) =>
//             {
//                 return new List<(string text, int index)> { ("Yes", 0) }; // Simulate user selecting "Yes"
//             };

//             // Mock Access.Users.Delete
//             Access.Users.Delete = id =>
//             {
//                 Assert.AreEqual(1, id, "The correct user ID should be passed for deletion.");
//             };

//             // Act
//             var result = DeleteAccountLogic.ConfirmAndDelete(user);

//             // Assert
//             Assert.IsTrue(result, "The account deletion should return true if confirmed.");
//         }

//         [TestMethod]
//         public void TestConfirmAndDelete_CancelDeletion()
//         {
//             // Arrange
//             var user = new UserModel { ID = 1, FirstName = "John", LastName = "Doe" };

//             // Mock selection logic
//             SelectionPresent.Show = (options, banner) =>
//             {
//                 return new List<(string text, int index)> { ("No", 1) }; // Simulate user selecting "No"
//             };

//             // Act
//             var result = DeleteAccountLogic.ConfirmAndDelete(user);

//             // Assert
//             Assert.IsFalse(result, "The account deletion should return false if not confirmed.");
//         }

//         [TestMethod]
//         public void TestGetActiveAccounts()
//         {
//             // Arrange
//             Access.Users.Read = () => new List<UserModel>
//             {
//                 new UserModel { FirstName = "John", LastName = "Doe" },
//                 new UserModel { FirstName = "Inactive", LastName = "User" }
//             };

//             // Act
//             var result = DeleteAccountLogic.GetActiveAccounts();

//             // Assert
//             Assert.AreEqual(1, result.Count, "Only active accounts should be included.");
//             Assert.AreEqual("John", result.First().FirstName, "The active account should be John Doe.");
//         }

//         [TestMethod]
//         public void TestGenerateMenuOptions()
//         {
//             // Arrange
//             var accounts = Enumerable.Range(1, 10)
//                                      .Select(i => new UserModel { FirstName = $"User{i}", LastName = $"Last{i}" })
//                                      .ToList();
//             int currentPage = 0;
//             int totalPages = 2;

//             // Act
//             var result = DeleteAccountLogic.GenerateMenuOptions(accounts, currentPage, totalPages);

//             // Assert
//             Assert.AreEqual(11, result.Count, "Menu options should include accounts and 'Next Page' option.");
//             Assert.IsTrue(result.Contains("Next Page >>"), "The menu should include 'Next Page' when there are more pages.");
//         }

//         [TestMethod]
//         public void TestDeleteAccount_ValidDeletion()
//         {
//             // Arrange
//             var currentUser = new UserModel { ID = 1, FirstName = "Current", LastName = "User" };
//             var sortedAccounts = Enumerable.Range(2, 5)
//                                            .Select(i => new UserModel { ID = i, FirstName = $"User{i}", LastName = $"Last{i}" })
//                                            .ToList();
//             string selectedText = "User2 Last2 (User)";
//             int currentPage = 0;

//             // Mock GetPage
//             DeleteAccountLogic.GetPage = (accounts, page, itemsPerPage) => sortedAccounts;

//             // Mock FormatAccount
//             DeleteAccountLogic.FormatAccount = (account) => $"{account.FirstName} {account.LastName} (User)";

//             // Mock ConfirmAndDelete
//             DeleteAccountLogic.ConfirmAndDelete = (account) =>
//             {
//                 Assert.AreEqual(2, account.ID, "The correct account should be selected for deletion.");
//                 return true;
//             };

//             // Act
//             var result = DeleteAccountLogic.DeleteAccount(currentUser, currentPage, sortedAccounts, selectedText);

//             // Assert
//             Assert.IsTrue(result, "The account should be successfully deleted.");
//         }

//         [TestMethod]
//         public void TestDeleteAccount_InvalidDeletion_SelfAccount()
//         {
//             // Arrange
//             var currentUser = new UserModel { ID = 1, FirstName = "Current", LastName = "User" };
//             var sortedAccounts = new List<UserModel> { currentUser };
//             string selectedText = "Current User (User)";
//             int currentPage = 0;

//             // Mock GetPage
//             DeleteAccountLogic.GetPage = (accounts, page, itemsPerPage) => sortedAccounts;

//             // Mock FormatAccount
//             DeleteAccountLogic.FormatAccount = (account) => $"{account.FirstName} {account.LastName} (User)";

//             // Act
//             var result = DeleteAccountLogic.DeleteAccount(currentUser, currentPage, sortedAccounts, selectedText);

//             // Assert
//             Assert.IsFalse(result, "The current user should not be able to delete their own account.");
//         }
//     }
// }
