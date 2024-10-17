public class ProductLogic
{
    private FoodMenuLogic menu = new FoodMenuLogic(1, new ThemeLogic());

    public void AddProductToMenu(ProductModel product)
    {
        menu.AddProduct(product);
        // SaveInventoryToDB();
    }

    public void SaveInventoryToDB()
    {
        Dictionary<string, ProductModel> products = menu.GetAllProducts();
        // Save products to database
        // ProductAccess.SaveProducts(products);
    }
}