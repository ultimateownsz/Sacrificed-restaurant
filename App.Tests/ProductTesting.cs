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

            ProductAccess productAccess = new ProductAccess();

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


        }



    }
}
