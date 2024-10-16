public class InventoryLogic
{
    private Dictionary<string, ProductModel> products = new();
    public void AddProduct(ProductModel product)
    {
        if (products.ContainsKey(product.ProductName))
        {
            products[product.ProductName].Quantity += product.Quantity;
        }
        else
        {
            products.Add(product.ProductName, product);
        }
    }

    public ProductModel GetProduct(string productName)
    {
        if (products.ContainsKey(productName))
        {
            return products[productName];
        }
        else
        {
            return null;
        }
    }

    public void RemoveProduct(string productName)
    {
        if (products.ContainsKey(productName))
        {
            products.Remove(productName);
        }
    }

    public Dictionary<string, ProductModel> GetAllProducts()
    {
        return products;
    }
}