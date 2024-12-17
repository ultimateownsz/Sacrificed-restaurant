// this class handles all the logic for adding, updating, and deleting products
using System.Diagnostics;
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

    public static IEnumerable<ProductModel> GetAllWithinCategory(string category)
    {
            return Access.Products.GetAllBy<string>("Course", category);
    }

     public static List<string> GetAllProductInfo()
    {
        return Access.Products.Read()
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name
                    : "No theme";
                return $"{p.Name,-18}   {p.Course,-15}   {themeName,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    public static List<string> GetAllWithinCategoryNew(string course)
    {
        return Access.Products.GetAllBy("Course", course)
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name
                    : "No theme";
                return $"{p.Name,-18}   {themeName,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    public static List<string> GetAllWithinTheme(string theme)
    {
        int? themeID = ThemeMenuManager.GetThemeIDByName(theme);
        return Access.Products.GetAllBy("ThemeID", themeID)
            .Select(p => {
                return $"{p.Name,-18}   {p.Course,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    public static ProductModel? ConvertStringChoiceToProductModel(string productInfo, string type, string Name)
    {
        productInfo = productInfo.Replace("€", "");
        var parts = productInfo.Split(new[] { "   " }, StringSplitOptions.None);
        parts = parts.Select(p => p.Trim()).Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        decimal price;
        int? themeID;

        if (!decimal.TryParse(parts.Last().TrimEnd(' '), out price))
        {
            throw new ArgumentException($"Could not parse price from product info: '{productInfo}'");
        }
        themeID = parts.Count() switch
        {
            4 => ThemeMenuManager.GetThemeIDByName(parts[2]),
            3 => ThemeMenuManager.GetThemeIDByName(parts[1]),
            _ => null
        };

        return Access.Products.Read()
            .FirstOrDefault(p =>
                p.Name == parts[0] &&
                p.Price == price &&
                (type == "course" ? p.Course == Name : p.Course == parts[1]) &&
                (type == "theme" ? p.ThemeID == ThemeMenuManager.GetThemeIDByName(Name) : p.ThemeID == themeID));
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
                if(newThemeName == null)
                {
                    Console.WriteLine("The theme has not been updated.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    return;
                }
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
                newProductEdit = newProductEdit.Replace('.', ',');
                decimal temp;

                if 
                (
                    !string.IsNullOrWhiteSpace(newProductEdit)
                    && newProductEdit.Contains(',')
                    && decimal.TryParse(newProductEdit, out temp)
                    && newProductEdit.Trim().Split(',')[1].Length == 2
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
            Price = type == "price" ? decimal.Parse(newProductEdit) : oldProduct.Price,
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

    public static string? CourseOrThemeValidator(string type)
    {
        string Name;
        if (type == "course")
        {
            Name = CourseLogic.GetValidString();
            if(Name == null)
            {
                Console.WriteLine("Failed to filter based on course");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }
            return Name;
        }
        else if (type == "theme")
        {
            Name = ThemeInputValidator.GetValidString();
            if(Name == null)
            {
                Console.WriteLine("Failed to filter based on theme");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }
            if(!ThemeMenuManager.DoesThemeExist(Name))
            {
                return null;
            }
            return Name;

        }
        else
        {
            return null;
        }
    }

    public static bool DeleteProduct(int productId)
    {
        if (productId < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(productId)} Product ID must be greater than 0.");
            // return ;
        }

        if (Access.Products.GetBy<int>("ID", productId) == null)
        {
            // Console.WriteLine($"Database does not contain a product with ID: {productId}.");
            return false;
        }
        Access.Products.Delete(productId);
        // Console.WriteLine($"Product with ID: {productId} deleted successfully.");
        return true;
    }
}