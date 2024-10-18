// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Welcome to this amazing program");
// Menu.Start();

class Program
{
    static void Main(string[] args)
    {
        // add products to store in the a dictionary
        ThemeMenuLogic menu = new ThemeMenuLogic(1, new ThemeModel());

        ThemeMenuModel italianMenu = new ThemeMenuModel(1, "Italian");
        
        // create product categories, with a theme, and a product name
        ProductCategory pizzaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pizza");
        ProductCategory pastaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pasta");
        ProductCategory craftBeerAlchoholCategory = new ProductCategory("Alcoholic Beverages", "Italian", "Craft beer");
        
        // create products, with a quantity, a price, a menu ID, and a category
        ProductModel pizza = new ProductModel(1, "Pizza", 1, 12.99m, italianMenu.MenuId, pizzaMainDishCategory);
        ProductModel pasta = new ProductModel(2, "Pasta", 1, 12.99m, italianMenu.MenuId, pastaMainDishCategory);
        ProductModel moretti = new ProductModel(3, "Moretti", 1, 2.99m, italianMenu.MenuId, craftBeerAlchoholCategory);
        
        // add products to the dictionary
        menu.AddProduct(pizza);
        menu.AddProduct(pasta);
        menu.AddProduct(moretti);

        // Write products to the database
        // ProductController.AddProduct(pizza);
        // ProductController.AddProduct(pasta);
        // ProductController.AddProduct(moretti);

        ProductView.DisplayAllProducts();

        // delete products out of the database
        ProductController.DeleteProduct(1);
        ProductController.DeleteProduct(2);
        ProductController.DeleteProduct(3);

        ThemesAccess.Write(italianMenu);
        ThemeView.DisplayActiveTheme(italianMenu);
        
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