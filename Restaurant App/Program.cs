// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Welcome to this amazing program");
// Menu.Start();

class Program
{
    static void Main(string[] args)
    {
        InventoryLogic inventory = new InventoryLogic();

        ProductModel pizza = new ProductModel(1, "Pizza", 100, 12.99m, 1, ProductCategoryType.MainDishes);
        inventory.AddProduct(pizza);
        pizza.CustomAttributes["Theme"] = "Italian";
        ProductsAccess.Write(pizza);


        ProductModel pasta = new ProductModel(2, "Pasta", 100, 12.99m, 1, ProductCategoryType.MainDishes);
        inventory.AddProduct(pasta);
        pasta.CustomAttributes["Theme"] = "Italian";

        ProductModel moretti = new ProductModel(3, "Moretti", 100, 2.99m, 1, ProductCategoryType.AlcoholicBeverages);
        inventory.AddProduct(moretti);
        moretti.CustomAttributes["Theme"] = "Italian";

        // List<ProductModel> allProducts = new List<ProductModel> { pizza, pasta, moretti };

        ThemeLogic themeLogic = new ThemeLogic();
        themeLogic.SetTheme("Italian", new List<ProductCategoryType> { ProductCategoryType.MainDishes, ProductCategoryType.AlcoholicBeverages });

        // List<ProductModel> italianProducts = themeLogic.GetProductsByCurrentTheme(allProducts);

        foreach (var product in inventory.GetAllProducts().Values)
        {
            Console.WriteLine($"{product.ProductName} - Category: {product.CategoryType}, Theme: {product.CustomAttributes["Theme"]}");
        }
    }
}