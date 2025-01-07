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
        var product = new ProductModel
        {
            ID = 1,
            Name = "Baklava",
            ProductName = "Turkish",
            Course = "Dessert",
            Price = 3.99m
        };
        var Product = new Product { ID = 11, ProductName = "Turkish" };

        // Act
        string result = SimulateAddProduct(Product, ProductModel);

        // Assert
        Assert.AreEqual("Product 'Turkish' has been added for June 2025.", result);
    }


    [TestMethod]
    public void TestValidateProductName()
    {
        string validProductName = "Valid Product";
        string invalidProductName = "Invalid123!";

        bool isValid1 = ValidateProductName(validProductName);
        bool isValid2 = ValidateProductName(invalidProductName);

        Assert.IsTrue(isValid1, "Expected the Product name to be valid.");
        Assert.IsFalse(isValid2, "Expected the Product name to be invalid.");
    }
    public void TestValidatePrice()
    {
        string validPrice = "9.99";
        string invalidPrice = "Invalid123!";

        bool isValid1 = ValidateProductPrice(validPrice);
        bool isValid2 = ValidateProductPrice(invalidPrice);

        Assert.IsTrue(isValid1, "Expected the price to be valid.");
        Assert.IsFalse(isValid2, "Expected the price to be invalid.");
    }
    
    // Simulated Methods for Testing
    //This Method simulates adding Products
    private string SimulateAddProduct(Product Product)
    {
        //calls ValidateProductName to check if it has symbols in it
        if (!ValidateProductName(ProductName))
            return "Invalid Product name. Only letters and spaces are allowed.";
        if (!ValidateProductPrice(ProductName))
            return "Invalid Product name. Only letters and spaces are allowed.";

        return $"Product '{ProductName}' has been added for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //This Method simulates editing Products
    private string SimulateEditProduct(Product existingProduct)
    {
        if (!ValidateProductName(newProductName))
            return "Invalid Product name. Only letters and spaces are allowed.";

        existingProduct.ProductName = newProductName;
        return $"Product updated to '{newProductName}' for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //This Method simulates Deleting Products
    private string SimulateDeleteProduct(Product ProductToDelete)
    {
        return $"Product '{ProductToDelete.ProductName}' has been deleted for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //Validates the name of the Product
    private bool ValidateProductName(string ProductName)
    {
        return Regex.IsMatch(ProductName, "^[A-Za-z ]+$"); //This line(FROM GOOGLE NOT GPT) validates if the name has anything other than letters or spaces, then proceeds to return true/false
    }

    //Validates the price of a product
    private bool ValidateProductPrice(decimal price)
    {
        string priceString = price.ToString("F2");
        var split = priceString.Split('.');
        return split.Length == 2 && split[1].Length == 2;
    }

    // gets the month name
    private string GetMonthName(int month)
    {
        return new DateTime(1, month, 1).ToString("MMMM");
    }

    // Supporting Classes
    public class Product
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
    }


    public class Product
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Course { get; set; }
        public int? ProductID { get; set; }
    }

}
