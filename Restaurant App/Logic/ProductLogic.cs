// this class handles all the logic for adding, updating, and deleting products
using System.Diagnostics;
using Project;

static class ProductLogic
{
    //This Method is used to add products
    public static bool AddProduct(ProductModel product)
    {
        if (product == null)
        {
            Console.WriteLine("Cant add empty products.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        if (Access.Products.GetBy<string?>("Name", product.Name) != null)
        {
            Console.WriteLine($"{product.Name} already exists.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return false;
        }

        Access.Products.Write(product);
        return true;
    }

    //This Method is used to validate and ask the user for a price or a name for the product
    public static string GetValidNameOrPrice(string type)
    {
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Enter {type}: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            var productInfo = Console.ReadLine();
            if(type == "name")
            {
                if (!string.IsNullOrWhiteSpace(productInfo) && !productInfo.Any(char.IsDigit)) // validate the name
                {
                    productInfo = char.ToUpper(productInfo[0]) + productInfo.Substring(1);
                    return productInfo;
                }
            }
            else if(type == "price")
            {
                productInfo = productInfo.Replace(',', '.');
                decimal temp;
                if (                                    // validate the price
                    !string.IsNullOrWhiteSpace(productInfo)
                    && productInfo.Contains('.')
                    && decimal.TryParse(productInfo, out temp)
                    && productInfo.Trim().Split('.')[1].Length == 2
                    && !productInfo.Contains(' '))
                {
                    return productInfo;
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter {type}: {productInfo}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nInvalid {type}...");
            if(type == "price") Console.WriteLine($"\nThe product price must have 2 decimals and no letters or symbols...");
            else Console.WriteLine($"\nThe product name must have no number or symbols...");
            Console.WriteLine("Press any key to retry or ESCAPE to go back"); // give the user the option to exit after making a mistake
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Escape || key.Key == ConsoleKey.B)
            {
                return null;
            }
        }
    }

    public static bool DoesProductExist(int? productId)
    {
        if (productId == null) return false;
        return Access.Products.GetBy<int?>("ID", productId.Value) != null;
    }

    //This Method deletes a product and all the related requests
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

    //This Method is gets the info of all the products and formalets them in a nice way
     public static List<string> GetAllProductInfo()
    {
        return Access.Products.Read()
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? p.ThemeID == 0 ? "No theme" : Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name //If the themeID is 0 then instead of leaving it empty, it writes "No theme"
                    : null;
                return $"{p.Name,-18}   {p.Course,-15}   {themeName,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    //This Method is gets the info of all the products within a course and formalets them in a nice way
    public static List<string> GetAllWithinCourse(string course)
    {
        return Access.Products.GetAllBy("Course", course)
            .Select(p => {
                var themeName = p.ThemeID.HasValue
                    ? p.ThemeID == 0 ? "No theme" : Access.Themes.GetBy<int?>("ID", p.ThemeID.Value)?.Name //If the themeID is 0 then instead of leaving it empty, it writes "No theme"
                    : null;
                return $"{p.Name,-18}   {themeName,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    //This Method is gets the info of all the products within a theme and formalets them in a nice way
    public static List<string> GetAllWithinTheme(string? theme)
    {
        int? themeID;
        
        if (theme != "0")
        {
            themeID = ThemeMenuLogic.GetThemeIDByName(theme);
        }
        else
        {
            themeID = 0;
        }
        return Access.Products.GetAllBy("ThemeID", themeID)
            .Select(p => {
                return $"{p.Name,-18}   {p.Course,-15}   €{p.Price:F2}";
            })
            .ToList();
    }

    //This Method converts the chosen product by the user into a ProductModel
    public static ProductModel? ConvertStringChoiceToProductModel(string productInfo, string type, string name)
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
        if(name == "0") themeID = 0;
        else
        {
            themeID = parts.Count() switch
            {
                4 => ThemeMenuLogic.GetThemeIDByName(parts[2]),
                3 => ThemeMenuLogic.GetThemeIDByName(parts[1]),
                _ => null
            };
        }
        if(themeID == null) themeID = 0;

        return Access.Products.Read()
            .FirstOrDefault(p =>
                p.Name == parts[0] &&
                p.Price == price &&
                (type == "course" ? p.Course == name : p.Course == parts[1]) &&
                (type == "theme" && name != "0" ? p.ThemeID == ThemeMenuLogic.GetThemeIDByName(name) : p.ThemeID == themeID));
    }

    //This Method updates a product in the db
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

    //This Method validates the edit the user wants to make to a product based on the type of edit
    public static void ProductEditValidator(ProductModel oldProduct, string type)
    {
        string newProductEdit;
        int? ThemeID = null;

        Console.Clear();
        if(type == "theme")
        {
            newProductEdit = ThemeInputValidator.GetValidThemeMenu();
            if(newProductEdit == "0")
            {
                newProductEdit = null;
            }
            else if(newProductEdit == null)
            {
                return;
            }
            else
            {
                ThemeID = ThemeMenuLogic.GetThemeIDByName(newProductEdit);
            }
        }
        else if(type == "price")
        {
            newProductEdit = GetValidNameOrPrice("price");
        }
        else if(type == "course")
        {
            newProductEdit = CourseLogic.GetValidCourse();
        }
        else
        {
            newProductEdit = GetValidNameOrPrice("name");
        }

        Console.Clear();
        if(type != "theme" && newProductEdit == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to update the {type}");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return;
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The {type} has been updated to {newProductEdit}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to update the {type}");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    //This Method validates all the info of a new product and then puts that info in a ProductModel
    public static ProductModel? ProductValidator()
    {
        ProductModel newProduct = new();

        string name = GetValidNameOrPrice("name"); //ask for the name
        if(name == null)
        {
            return null;
        }
        newProduct.Name = name;

        string course = CourseLogic.GetValidCourse(); //ask for the course
        if(course == null)
        {
            return null;
        }
        newProduct.Course = course;

        string theme = ThemeInputValidator.GetValidThemeMenu(); //ask for the theme
        if(theme == null)
        {
            return null;
        }
        if(theme == "0")
        {
            newProduct.ThemeID = 0;
        }
        else
        {
            newProduct.ThemeID = ThemeMenuLogic.GetThemeIDByName(theme);
        }

        string price = GetValidNameOrPrice("price"); //ask for the price
        decimal temp;
        if (decimal.TryParse(price, out temp))
        {
            newProduct.Price = decimal.Parse(price);
        }
        else
        {
            return null;
        }

        return newProduct; //return the new model
    }

    //This Method Deletes a product from the db
    public static bool DeleteProduct(int productId)
    {
        if (productId < 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(productId)} Product ID must be greater than 0.");
            // return ;
        }

        if (Access.Products.GetBy<int>("ID", productId) == null)
        {
            return false;
        }
        Access.Products.Delete(productId);
        return true;
    }
}