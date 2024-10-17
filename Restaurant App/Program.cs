// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Welcome to this amazing program");
// Menu.Start();

class Program
{
    static void Main(string[] args)
    {
        FoodMenuLogic menu = new FoodMenuLogic(1, new ThemeLogic());

        // ThemeLogic themeLogic = new ThemeLogic();
        // themeLogic.SetTheme("Italian", new List<ProductCategory> { "MainDishes", "Alcoholic Beverages" });

        MenuModel italianMenu = new MenuModel(1, "Italian");
        
        ProductCategory pizzaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pizza");
        ProductCategory pastaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pasta");
        ProductCategory craftBeerAlchoholCategory = new ProductCategory("Alcoholic Beverages", "Italian", "Craft beer");
        
        
        ProductModel pizza = new ProductModel(1, "Pizza", 1, 12.99m, italianMenu.MenuId, pizzaMainDishCategory);
        ProductModel pasta = new ProductModel(2, "Pasta", 1, 12.99m, italianMenu.MenuId, pastaMainDishCategory);
        ProductModel moretti = new ProductModel(3, "Moretti", 1, 2.99m, italianMenu.MenuId, craftBeerAlchoholCategory);
        
        menu.AddProduct(pizza);
        menu.AddProduct(pasta);
        menu.AddProduct(moretti);

        // ProductManager
        ProductController.AddProduct(pizza);
        ProductController.AddProduct(pasta);
        ProductController.AddProduct(moretti);

        ProductView.DisplayAllProducts();

        // ProductController.DeleteProduct(1);
        // ProductController.DeleteProduct(2);
        // ProductController.DeleteProduct(3);

        // ThemesAccess.Write(italianMenu);
        
        // ThemesAccess.Delete(1);

        // List<ProductModel> allProducts = new List<ProductModel> { pizza, pasta, moretti };

        // foreach (var product in allProducts)
        // {
        //     var categoryType = product.Category.GetCategoryType();
        //     var theme = product.Category.Theme;
            // Console.WriteLine($"{product.ProductName} - Category: {categoryType}, Theme: {theme}");
        // }


    }
}