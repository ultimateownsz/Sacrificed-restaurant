using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restaurant;
using System;
using System.Collections.Generic;

// This unit test isnt for a user story, just wanted to practice
[TestClass]
public class DeleteAccountLogicTests
{
    // Test GetPage method
    [TestMethod]
    public void TestGetPage()
    {
        // Arrange: Create a list of UserModel instances (dummy accounts)
        var accounts = new List<UserModel>
        {
            new UserModel { ID = 1, FirstName = "John", LastName = "Doe" },
            new UserModel { ID = 2, FirstName = "Jane", LastName = "Doe" },
            new UserModel { ID = 3, FirstName = "Mario", LastName = "Mario" }, // Fun fact:
            new UserModel { ID = 4, FirstName = "Luigi", LastName = "Mario" }, // Dit zijn hun echte
            new UserModel { ID = 5, FirstName = "Link", LastName = "Link" }    // achternamen
        };

        int page = 1; // Test for page 1 (indexing starts at 0)
        int itemsPerPage = 2; // Items per page

        // Act: Call GetPage to get the second page of accounts (index 1)
        var result = DeleteAccountLogic.GetPage(accounts, page, itemsPerPage);

        // Assert: Verify that the correct accounts are returned for page 1 (index 1)
        Assert.AreEqual(2, result.Count); // There should be 2 accounts on page 1

        // Check that the correct users are returned (accounts 3 and 4 for page 1)
        Assert.AreEqual(3, result[0].ID); // Mario Mario should be the first account on page 1
        Assert.AreEqual(4, result[1].ID); // Luigi Luigi should be the second account on page 1
    }

    [TestMethod]
    public void TestGetPage_FirstPage()
    {
        // Arrange: Create a list of UserModel instances (dummy accounts)
        var accounts = new List<UserModel>
        {
            new UserModel { ID = 1, FirstName = "John", LastName = "Doe" },
            new UserModel { ID = 2, FirstName = "Jane", LastName = "Doe" },
            new UserModel { ID = 3, FirstName = "Mario", LastName = "Mario" },
            new UserModel { ID = 4, FirstName = "Luigi", LastName = "Mario" },
            new UserModel { ID = 5, FirstName = "Link", LastName = "Link" }
        };

        int page = 0; // First page (index 0)
        int itemsPerPage = 2; // Items per page

        // Act: Call GetPage to get the first page of accounts (index 0)
        var result = DeleteAccountLogic.GetPage(accounts, page, itemsPerPage);

        // Assert: Verify that the correct accounts are returned for page 0 (index 0)
        Assert.AreEqual(2, result.Count); // There should be 2 accounts on page 0

        // Check that the correct users are returned (accounts 1 and 2 for page 0)
        Assert.AreEqual(1, result[0].ID); // John Doe should be the first account on page 0
        Assert.AreEqual(2, result[1].ID); // Jane Doe should be the second account on page 0
    }

    [TestMethod]
    public void TestGetPage_LastPage()
    {
        // Arrange: Create a list of UserModel instances (dummy accounts)
        var accounts = new List<UserModel>
        {
            new UserModel { ID = 1, FirstName = "John", LastName = "Doe" },
            new UserModel { ID = 2, FirstName = "Jane", LastName = "Doe" },
            new UserModel { ID = 3, FirstName = "Mario", LastName = "Mario" },
            new UserModel { ID = 4, FirstName = "Luigi", LastName = "Mario" },
            new UserModel { ID = 5, FirstName = "Link", LastName = "Link" }
        };

        int page = 2; // Last page (index 2)
        int itemsPerPage = 2; // Items per page

        // Act: Call GetPage to get the last page of accounts (index 2)
        var result = DeleteAccountLogic.GetPage(accounts, page, itemsPerPage);

        // Assert: Verify that the correct accounts are returned for page 2 (index 2)
        Assert.AreEqual(1, result.Count); // There should be 1 account on page 2

        // Check that the correct user is returned (account 5 for page 2)
        Assert.AreEqual(5, result[0].ID); // Link Link should be the only account on page 2
    }
}
