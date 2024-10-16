public class ProductLogic
{
    private InventoryLogic inventory = new InventoryLogic();

    public void AddProductToInventory(ProductModel product)
    {
        inventory.AddProduct(product);
        // SaveInventoryToDB();
    }

    public void SaveInventoryToDB()
    {
        Dictionary<string, ProductModel> products = inventory.GetAllProducts();
        // Save products to database
        // ProductAccess.SaveProducts(products);
    }
}