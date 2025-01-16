using App.DataAccess;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Logic;
using App.DataAccess.Product;
using App.DataModels.Product;

namespace App.Tests
{
    [TestClass]
    public class ProductEditTest
    {

        [TestMethod]
        public void Test_AddProduct()
        {
            // Arrange

            ProductModel newProduct = new ProductModel { ID = null, Name ="Meatballs", Price = 6.99m, Course = "Appetizer", ThemeID = 1 };
            ProductModel existingProduct = new ProductModel { ID = null, Name ="Pizza", Price = 6.99m, Course = "Appetizer", ThemeID = 1 };

            ProductAccess productAccess = new ProductAccess();
            PairAccess pairAccess = new PairAccess();
            RequestAccess requestAccess = new RequestAccess();

            foreach (var pair in pairAccess.GetAll())
            {
                pairAccess.Delete(pair.ID);
            }

            foreach (var request in requestAccess.GetAll())
            {
                requestAccess.Delete(request.ID);
            }

            foreach (var product in productAccess.GetAll())
            {
                productAccess.Delete(product.ID);
            }
            
            productAccess.Write(new ProductModel { ID = 1, Name ="Pizza", Price = 12.99m, Course = "Main", ThemeID = 1 });
            productAccess.Write(new ProductModel { ID = 2, Name ="Spaghetti", Price = 10.99m, Course = "Main", ThemeID = 1 });
            productAccess.Write(new ProductModel { ID = 3, Name ="Cake", Price = 5.99m, Course = "Dessert", ThemeID = 1 });
            productAccess.Write(new ProductModel { ID = 4, Name ="Beer", Price = 2.99m, Course = "Beverage", ThemeID = 1 });
            productAccess.Write(new ProductModel { ID = 5, Name ="Wine", Price = 4.99m, Course = "Beverage", ThemeID = 1 });
            productAccess.Write(new ProductModel { ID = 6, Name ="Salad", Price = 9.99m, Course = "Appetizer", ThemeID = 1 });


            // Act
            ProductLogic.AddProduct(newProduct);
            ProductLogic.AddProduct(existingProduct);

            // Assert: Ensure the new theme and schedule are added
            Assert.AreEqual(7, productAccess.GetAll().Count, "Themes count mismatch");
            Assert.AreEqual("Meatballs", productAccess.GetAll().Last().Name, "Theme name mismatch");
        }

        // [TestMethod]
        // public void Test_EditProduct()
        // {
        //     // Arrange
        //     int month = 12;
        //     int year = 2025;
        //     string themeName = "British";

        //     ThemeAccess themeAccess = new ThemeAccess();
        //     ScheduleAccess scheduleAccess = new ScheduleAccess();

        //     foreach (var theme in themeAccess.GetAll())
        //     {
        //         themeAccess.Delete(theme.ID);
        //     }
            
        //     foreach (var schedule in scheduleAccess.GetAll())
        //     {
        //         scheduleAccess.Delete(schedule.ID);
        //     }

        //     foreach (var theme in themeAccess.GetAll())
        //     {
        //         themeAccess.Delete(theme.ID);
        //     }
            
        //     themeAccess.Write(new ThemeModel { ID = 1, Name = "Japanese" });
        //     themeAccess.Write(new ThemeModel { ID = 2, Name = "Italian" });
        //     themeAccess.Write(new ThemeModel { ID = 3, Name = "British" });
        //     themeAccess.Write(new ThemeModel { ID = 4, Name = "Turkish" });
        //     themeAccess.Write(new ThemeModel { ID = 5, Name = "Irish" });
        //     themeAccess.Write(new ThemeModel { ID = 6, Name = "American" });

        //     scheduleAccess.Write(new ScheduleModel { ID = 1, Year = 2025, Month = 12, ThemeID = 6 });
        //     scheduleAccess.Write(new ScheduleModel { ID = 2, Year = 2025, Month = 10, ThemeID = 2 });

        //     // Act
        //     ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);

        //     // Assert: Ensure no new data is added and existing schedule is updated
        //     Assert.AreEqual(6, themeAccess.GetAll().Count);
        //     Assert.AreEqual(2, scheduleAccess.GetAll().Count);

        //     var existingSchedule = scheduleAccess.GetAll().First();
        //     Assert.AreEqual(3, existingSchedule.ThemeID);
        // }

    }
}
