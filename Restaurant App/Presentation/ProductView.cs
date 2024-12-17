using Project;

static class ProductView
{

    //This is the main menu for product managment, its nothing special
    public static void ProductMainMenu()
    {
        string banner = $"PRODUCT MENU\n\n";
            List<string> options = new()
        {
            "Add product",
            "Show all products",
            "Choose products course",
            "Choose products theme\n",
            "back"
        };

        while (true)
        {
            string? name;
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Add product":
                    AddProduct();
                    break;
                case "Show all products":
                    DisplayProducts();
                    break;
                case "Choose products course":
                    while(true)
                    {                   
                        name = CourseLogic.GetValidCourse();
                        if (name == null)
                        {
                            Console.WriteLine("");
                            break;
                        }
                        DisplayProducts("course", name);
                    }
                    break;
                case "Choose products theme\n":
                    while(true)
                    {
                        name = ThemeInputValidator.GetValidThemeMenu();
                        if (name == null)
                        {
                            Console.WriteLine("");
                            break;
                        }
                        DisplayProducts("theme", name);
                    }
                    break;
                case "back" or "":
                    return;
            }
        }

    }

    // Display all products based on filter type
    public static void DisplayProducts(string filterType = "", string name = "")
    {
        ProductModel? chosenProduct;
        string banner;
        List<string> products;
        while (true)
        {
            if(filterType == "")
            {
                banner = "Choose a product to edit/delete:\n\n";
                products = ProductManager.GetAllProductInfo().ToList();
                if(products.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There are no products in the resturaunt");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                } 
            }
            else if (filterType == "course")
            {
                banner = $"Choose a product to edit/delete:\n\n{name}:\n\n";
                products = ProductManager.GetAllWithinCourse(name).ToList();
                if(products.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There are no products in {name}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                } 
            }
            else
            {
                if(name == "0")
                {
                    banner = $"Choose a product to edit/delete:\n\nNo theme:\n\n";
                }
                else
                {
                    banner = $"Choose a product to edit/delete:\n\n{name}:\n\n";
                }
                products = ProductManager.GetAllWithinTheme(name).ToList();
                if(products.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There are no products in the {name} theme");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                } 
            }

            string productSelection = SelectionPresent.Show(products, banner).text;

            if(productSelection == "")
            {
                Console.WriteLine("skibidi"); //no idea why but this fixes a lil bug somehow
                return;
            }

            chosenProduct = ProductManager.ConvertStringChoiceToProductModel(productSelection, filterType, name);
            if(chosenProduct == null)
            {
                Console.WriteLine("rizz");
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
                    ProductManager.ProductEditValidator(chosenProduct, "name");
                    break;
                case "Edit price":
                    ProductManager.ProductEditValidator(chosenProduct, "price");
                    break;
                case "Edit course":
                    ProductManager.ProductEditValidator(chosenProduct, "course");
                    break;
                case "Edit theme":
                    ProductManager.ProductEditValidator(chosenProduct, "theme");
                    break;
                case "Delete product\n":
                    if(DeleteProduct(chosenProduct))
                        return;
                    break;
                case "back" or "":
                    return;
            }
        }
    }

    public static bool DeleteProduct(ProductModel chosenProduct)
    {
        Console.Clear();
        string banner = $"Do you want to delete {chosenProduct.Name}\n\n";
        List<string> options = new List<string>{"Yes", "No"};
        string selection = SelectionPresent.Show(options, banner, false).text;
        if(selection == "No" || selection == "")
        {
            Console.WriteLine("fix");
            return false;
        }
        Console.Clear();
        if(ProductManager.DeleteProductAndRelatedRequests(chosenProduct.ID))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{chosenProduct.Name} has been deleted.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to delete {chosenProduct.Name}.");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }
    }

    public static void AddProduct()
    {
        ProductModel? newProduct = ProductManager.ProductValidator();
        Console.Clear();
        if(newProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid product info.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        else if(ProductManager.AddProduct(newProduct))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{newProduct.Name} has been Added.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to Add {newProduct.Name}.");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
    } 
}
