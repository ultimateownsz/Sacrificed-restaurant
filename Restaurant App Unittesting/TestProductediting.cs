using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ProductEditingTests
{
        Product existingProduct = new Product
        {
            ID = 1,
            Name = "Baklava",
            Price =  3.99m,
            Course = "Dessert",
            ThemeID = 11
        };

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

    [TestMethod]
    public void TestNameEditProduct()
    {

        var theme = new Theme { ID = 11, ThemeName = "Turkish" };
        var newProductName = "Simit";

        string result = SimulateEditProduct(existingProduct, newProductName);

        Assert.AreEqual("Product updated to 'Simit' from 'Baklava'.", result);
    }
    [TestMethod]
    public void TestPriceEditProduct()
    {

        var theme = new Theme { ID = 11, ThemeName = "Turkish" };
        var newProductPrice = 6.99m;

        string result = SimulateEditProduct(existingProduct, newProductPrice);

        Assert.AreEqual("Product updated to '6.99' from '3.99'.", result);
    }

    [TestMethod]
    public void TestCourseEditProduct()
    {

        var theme = new Theme { ID = 11, ThemeName = "Turkish" };
        var newProductCourse = "Appetizer";

        string result = SimulateEditProductCourse(existingProduct, newProductCourse);

        Assert.AreEqual("Product updated to 'Appetizer' from 'Dessert'.", result);
    }

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

    //This Method simulates editing Products
    private string SimulateEditProduct(Product existingProduct, string newProductName)
    {
        if (!ValidateProductName(newProductName))
            return "Invalid Product name. Only letters and spaces are allowed.";


        string OldProductName = existingProduct.Name;
         existingProduct.Name = newProductName;
        return $"Product updated to '{existingProduct.Name}' from '{OldProductName}'.";
    }

    private string SimulateEditProduct(Product existingProduct, decimal newProductPrice)
    {
        if (!ValidateProductPrice(existingProduct.Price))
            return "Invalid Product name. Only letters and spaces are allowed.";


        decimal? OldProductPrice = existingProduct.Price;
        existingProduct.Price = newProductPrice;
        return $"Product updated to '{existingProduct.Price}' from '{OldProductPrice}'.";
    }

    private string SimulateEditProductCourse(Product existingProduct, string newProductCourse)
    {
        if (!ValidateProductCourse(newProductCourse))
            return "Invalid Product course. Only letters and spaces are allowed.\nAnd only one of these Courses Main, Appetizer, Dessert or Beverage.";


        string OldProductCourse = existingProduct.Course;
         existingProduct.Course = newProductCourse;
        return $"Product updated to '{existingProduct.Course}' from '{OldProductCourse}'.";
    }

    //Validates the name of the Product
    private bool ValidateProductName(string ProductName)
    {
        return Regex.IsMatch(ProductName, "^[A-Za-z ]+$"); //This line(FROM GOOGLE NOT GPT) validates if the name has anything other than letters or spaces, then proceeds to return true/false
    }

    private bool ValidateProductCourse(string ProductCourseName)
    {
        List<string> courses = new List<string> { "Main", "Appetizer", "Dessert", "Beverage" };
        if(Regex.IsMatch(ProductCourseName, "^[A-Za-z ]+$") && courses.Contains(ProductCourseName))
        {
            return true;
        }
        return false;
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
