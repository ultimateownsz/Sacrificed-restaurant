// this class handles all the logic for adding, updating, and deleting products

using Project;

static class ProductManager
{
    public static bool AddProduct(ProductModel product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        if (Access.Products.GetBy<int?>("ID", product.ID) != null)
        {
            // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} already exists.");
            return false;
        }
        Access.Products.Write(product);
        // Console.WriteLine($"Product: {product.ProductName}, with ID: {product.ProductId} added successfully.");

        return true;
    }

    //public static bool UpdateProductQuantity(ProductModel product, int newQuantity)
    //{

    //    //product.UpdateQuantity(newQuantity);
    //    ProductsAccess.Update(product);
    //    return true;
    //    // Console.WriteLine($"Updated '{product.ProductName} (ID: {product.ProductId}) to quantity {product.Quantity}.");
    //}

    public static bool DoesProductExist(int? productId)
    {
        if (productId == null) return false;
        return Access.Products.GetBy<int?>("ID", productId.Value) != null;
    }


    public static bool DeleteProductAndRelatedRequests(int? productId)
    {
        var requests = Access.Requests.Read().Where(r => r.ProductID == productId).ToList();
        foreach (var request in requests)
        {
            Access.Requests.Delete(request.ID);
        }

        if (Access.Products.GetBy<int?>("ID", productId) == null)
        {
            return false;
        }
        Access.Products.Delete(productId);
        return true;
    }

     public static List<ProductModel> GetAllProducts()
    {
        return Access.Products.Read().ToList();
    }

     public static List<string> GetAllProductInfo()
    {
        return Access.Products.Read()
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name
                    : "No theme";
                return $"{p.Name} - {p.Price}€ - {p.Course} - {themeName}";
            })
            .ToList();
    }



    public static ProductModel? ConvertStringChoiceToProductModel(string productInfo)
    {
        productInfo = productInfo.Replace("€", "");
        var parts = productInfo.Split(" - ");
        if (parts.Length != 4)
        {
            throw new ArgumentException($"Could not parse product info: '{productInfo}'");
        }

        decimal price;

        if (!decimal.TryParse(parts[1].TrimEnd(' '), out price))
        {
            throw new ArgumentException($"Could not parse price from product info: '{productInfo}'");
        }

    int? themeID = ThemeMenuManager.GetThemeIDByName(parts[3]);

        return Access.Products.Read()
            .FirstOrDefault(p =>
                p.Name == parts[0] &&
                p.Price == price &&
                p.Course == parts[2] &&
                p.ThemeID == themeID);
    }

    public static bool UpdateProduct(ProductModel oldProduct, ProductModel newProduct)
    {
        var existingProduct = Access.Products.GetBy<int?>("ID", oldProduct.ID);

        if (existingProduct == null)
        {
            return false;
        }

        newProduct.ID = oldProduct.ID;
        Access.Products.Update(newProduct);
        return true;
    }

    public static IEnumerable<ProductModel> GetAllWithinCategory(string category)
    {
        return Access.Products.GetAllBy<string>("Course", category);
    }


    
    public static void ProductEditValidator(ProductModel oldProduct, string type, bool themeEdit)
    {
        List<string> courseNames = new List<string>{"main", "dessert", "appetizer", "beverage"};
        string newProductEdit = "";
        int? ThemeID = null;

        if(themeEdit)
        {
            while(true)
            {
                string newThemeName = ThemeInputValidator.GetValidString();
                if(!ThemeMenuManager.DoesThemeExist(newThemeName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("This theme doesn't exist.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to retry or escape to go back");

                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                }
                else
                {
                    ThemeID = ThemeMenuManager.GetThemeIDByName(newThemeName);
                    newProductEdit = newThemeName;
                    Console.WriteLine(ThemeID);
                    Console.WriteLine(oldProduct.ThemeID);
                    Console.ReadKey();
                    break;
                }
            }
        }

        while(true && !themeEdit)
        {

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Enter new product {type}: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            newProductEdit = Console.ReadLine().ToLower();
            
            if(type == "price")
            {
                decimal temp;

                if 
                (
                    !string.IsNullOrWhiteSpace(newProductEdit)
                    && decimal.TryParse(newProductEdit, out temp)
                    && newProductEdit.Contains('.')
                    && newProductEdit.Trim().Split('.')[1].Length == 2
                    && !newProductEdit.Contains(' ')
                )
                {
                    break;
                }
            }
            else if(type == "course")
            {
                if (!string.IsNullOrWhiteSpace(newProductEdit) && !newProductEdit.Any(char.IsDigit) && courseNames.Contains(newProductEdit))
                {
                    break;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(newProductEdit) && !newProductEdit.Any(char.IsDigit))
                {
                    break;
                }
            }

            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter new product {type}: {newProductEdit}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nInvalid {type}...");
            Console.ReadKey();
        }

        ProductModel newProduct = new ProductModel
        {
            ID = oldProduct.ID,
            Name = type == "name" ? newProductEdit : oldProduct.Name,
            Price = type == "price" ? Convert.ToDecimal(newProductEdit) : oldProduct.Price,
            Course = type == "course" ? char.ToUpper(newProductEdit[0]) + newProductEdit.Substring(1) : oldProduct.Course,
            ThemeID = type == "theme" ? ThemeID : oldProduct.ThemeID,
        };

        Console.Clear();
        if(UpdateProduct(oldProduct, newProduct))
        {
            Console.WriteLine($"The {type} has been updated to {newProductEdit}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"Failed to update the {type}");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey(); 
        }
    }

}