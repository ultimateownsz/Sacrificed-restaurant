using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ProductEditingTests
{
    [TestMethod]
    public void TestAddProduct()
    {
        // Arrange the Models(the models are the end of this file)
        var product = new Product
        {
            ID = 1,
            Name = "Baklava",
            Price =  3.99m,
            Course = "Dessert",
            ThemeID = 11
        };
        var theme = new Theme { ID = 11, ThemeName = "Turkish" };

        // Act
        string result = SimulateAddProduct(product, theme);

        // Assert
        Assert.AreEqual("Product 'Baklava' has been added for into the Turkish theme.", result);
    }

    // [TestMethod]
    // public void TestEditProduct()
    // {
    //     var existingProduct = new Product { ID = 101, ProductName = "Turkish" };
    //     var schedule = new Schedule { Year = 2025, Month = 7, ProductID = 101 };
    //     var newProductName = "Japanese";

    //     string result = SimulateEditProduct(existingProduct, schedule, newProductName);

    //     Assert.AreEqual("Product updated to 'Japanese' for July 2025.", result);
    // }

    // [TestMethod]
    // public void TestDeleteProduct()
    // {
    //     var ProductToDelete = new Product { ID = 102, ProductName = "Japanese" };
    //     var schedule = new Schedule { Year = 2025, Month = 12, ProductID = 102 };

    //     string result = SimulateDeleteProduct(ProductToDelete, schedule);

    //     Assert.AreEqual("Product 'Japanese' has been deleted for July 2025.", result);
    // }

    // [TestMethod]
    // public void TestValidateProductName()
    // {
    //     string validProductName = "Valid Product";
    //     string invalidProductName = "Invalid123!";

    //     bool isValid1 = ValidateProductName(validProductName);
    //     bool isValid2 = ValidateProductName(invalidProductName);

    //     Assert.IsTrue(isValid1, "Expected the Product name to be valid.");
    //     Assert.IsFalse(isValid2, "Expected the Product name to be invalid.");
    // }
    // public void TestValidatePrice()
    // {
    //     string validPrice = "9.99";
    //     string invalidPrice = "Invalid123!";

    //     bool isValid1 = ValidateProductPrice(validPrice);
    //     bool isValid2 = ValidateProductPrice(invalidPrice);

    //     Assert.IsTrue(isValid1, "Expected the price to be valid.");
    //     Assert.IsFalse(isValid2, "Expected the price to be invalid.");
    // }
    
    // Simulated Methods for Testing
    //This Method simulates adding Products
    private string SimulateAddProduct(Product Product, Theme theme)
    {
        //calls ValidateProductName to check if it has symbols in it
        if (!ValidateProductName(Product.Name))
            return "Invalid Product name. Only letters and spaces are allowed.";
        if (!ValidateProductPrice(Product.Price))
            return "Invalid Product name. Only letters and spaces are allowed.";

        return $"Product '{Product.Name}' has been added for into the {theme.ThemeName} theme.";
    }

    // //This Method simulates editing Products
    // private string SimulateEditProduct(Product existingProduct)
    // {
    //     if (!ValidateProductName(newProductName))
    //         return "Invalid Product name. Only letters and spaces are allowed.";

    //     existingProduct.ProductName = newProductName;
    //     return $"Product updated to '{newProductName}' for {GetMonthName(schedule.Month)} {schedule.Year}.";
    // }

    // //This Method simulates Deleting Products
    // private string SimulateDeleteProduct(Product ProductToDelete)
    // {
    //     return $"Product '{ProductToDelete.ProductName}' has been deleted for {GetMonthName(schedule.Month)} {schedule.Year}.";
    // }

    //Validates the name of the Product
    private bool ValidateProductName(string ProductName)
    {
        return Regex.IsMatch(ProductName, "^[A-Za-z ]+$"); //This line(FROM GOOGLE NOT GPT) validates if the name has anything other than letters or spaces, then proceeds to return true/false
    }

    //Validates the price of a product
    private bool ValidateProductPrice(decimal? price)
    {
        string priceString = price?.ToString("F2") ?? "";
        var split = priceString.Split('.');
        return split.Length == 2 && split[1].Length == 2;
    }

    // Supporting Classes
    public class Theme
    {
        public int ID { get; set; }
        public string ThemeName { get; set; }
    }


    public class Product
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Course { get; set; }
        public int? ThemeID { get; set; }
    }

}
