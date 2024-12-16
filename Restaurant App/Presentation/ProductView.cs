using Project;

static class ProductView
{
    public static void ProductMainMenu()
    {
        string banner = $"PRODUCT MENU\n\n";
            List<string> options = new()
        {
            "Add product",
            "Show all products",
            "Choose products course",
            "Choose products theme",
            "Choose products in month\n",
            "back"
        };

        while (true)
        {
            Console.Clear();
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Add product":
                    
                    break;
                case "Show all products":
                    DisplayAllProducts();
                    break;
                case "Choose products theme":
                    
                    break;
                case "Choose products in month":
                    
                    break;
                case "Choose products course":
                    
                    break;
                case "back" or "":
                    return;
            }
        }

    }
    // Display all products
    public static void DisplayAllProducts()
    {
        ProductModel? chosenProduct;
        string banner = "Choose a product to edit/delete:\n\n";
        while (true)
        {
            List<string> products = ProductManager.GetAllProductInfo().ToList();
            string productSelection = SelectionPresent.Show(products, banner).text;

            if(productSelection == "")
            {
                return;
            }

            chosenProduct = ProductManager.ConvertStringChoiceToProductModel(productSelection);

            if(chosenProduct == null)
            {
                return;
            }
            DeleteOrEditChoice(chosenProduct);
        }
    }

    public static void DeleteOrEditChoice(ProductModel chosenProduct)
    {
        string banner = "Choose a what to do with the product:\n\n";
            List<string> options = new()
        {
            "Edit name",
            "Edit price",
            "Edit course",
            "Edit theme",
            "Delete product\n",
            "back"
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Edit name":
                    EditProductName(chosenProduct);
                    break;
                case "Edit price":
                    EditProductPrice(chosenProduct);
                    break;
                case "Edit course":
                    EditProductCourse(chosenProduct);
                    break;
                case "Edit theme":
                    EditProductTheme(chosenProduct);
                    break;
                case "Delete product":
                    DeleteProduct(chosenProduct);
                    break;
                case "back" or "":
                    return;
            }
        }
    }

    public static void EditProductName(ProductModel oldProduct)
    {
        string newProductName;
        while(true)
        {        
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter new product name: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            newProductName = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newProductName) && !newProductName.Any(char.IsDigit))
            {
                break;
            }
            
            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter new product name: {newProductName}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid product name...");
            Console.ReadKey();
        }
        ProductModel newProduct = new ProductModel
        {
            ID = oldProduct.ID,
            Name = newProductName,
            Price = oldProduct.Price,
            Course = oldProduct.Course,
            ThemeID = oldProduct.ThemeID
        };

        if(ProductManager.UpdateProduct(oldProduct, newProduct))
        {
            Console.WriteLine($"Name has been updated from {oldProduct.Name} to {newProductName}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"Name failed to update");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey(); 
        }

        return;
    }

    public static void EditProductPrice(ProductModel oldProduct)
    {
        string newProductPriceString;
        while(true)
        {        
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Enter new product price: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;
            newProductPriceString = Console.ReadLine();

            decimal temp;

            if 
            (
                !string.IsNullOrWhiteSpace(newProductPriceString)
                && decimal.TryParse(newProductPriceString, out temp)
                && newProductPriceString.Contains('.')
                && newProductPriceString.Trim().Split('.')[1].Length == 2
                && !newProductPriceString.Contains(' ')
            )
            {
                break;
            }
            
            Console.Clear();            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enter new product price: {newProductPriceString}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid product price...");
            Console.ReadKey();
        }

        decimal newProductPriceDecimal = Convert.ToDecimal(newProductPriceString);
        ProductModel newProduct = new ProductModel
        {
            ID = oldProduct.ID,
            Name = oldProduct.Name,
            Price = newProductPriceDecimal,
            Course = oldProduct.Course,
            ThemeID = oldProduct.ThemeID
        };

        if(ProductManager.UpdateProduct(oldProduct, newProduct))
        {
            Console.WriteLine($"Price has been updated from {oldProduct.Price}€ to {newProductPriceDecimal}€");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"Price failed to update");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey(); 
        }
        
        return;
    }

    public static void EditProductCourse(ProductModel oldProduct)
    {

    }

    public static void EditProductTheme(ProductModel oldProduct)
    {

    }

    public static void DeleteProduct(ProductModel chosenProduct)
    {

    }
}