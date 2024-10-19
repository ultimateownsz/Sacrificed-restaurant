// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Welcome to this amazing program");
// Menu.Start();

// class Program
// {
//     static void Main(string[] args)
//     {
//         // create a list of themes
//         // List<ThemeMenuModel> themes = new List<ThemeMenuModel> { };

//         // create a theme menu
//         ThemeMenuModel italianMenu = new ThemeMenuModel(1, "Italian");

//         // themes.Add(new ThemeMenuModel(1, "Italian"));
        
//         // create product categories, with a theme, and a product name
//         ProductCategory pizzaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pizza");
//         ProductCategory pastaMainDishCategory = new ProductCategory("MainDishes", "Italian", "Pasta");
//         ProductCategory craftBeerAlchoholCategory = new ProductCategory("Alcoholic Beverages", "Italian", "Craft beer");
        
//         // create products, with a quantity, a price, a menu ID, and a category
//         ProductModel pizza = new ProductModel(1, "Pizza", 1, 12.99m, italianMenu.MenuId, pizzaMainDishCategory);
//         ProductModel pasta = new ProductModel(2, "Pasta", 1, 12.99m, italianMenu.MenuId, pastaMainDishCategory);
//         ProductModel moretti = new ProductModel(3, "Moretti", 1, 2.99m, italianMenu.MenuId, craftBeerAlchoholCategory);

//         // Write products to the database
//         // ProductController.AddProduct(pizza);
//         // ProductController.AddProduct(pasta);
//         // ProductController.AddProduct(moretti);


//         // delete products out of the database
//         ProductController.DeleteProduct(1);
//         ProductController.DeleteProduct(2);
//         ProductController.DeleteProduct(3);

//         // ThemeView.AddTheme(italianMenu);

//         ThemeView.DeleteTheme(italianMenu);

//         // display all products and themes
//         ThemeView.DisplayAllThemes();
//         ProductView.DisplayAllProducts();
//     }
// }