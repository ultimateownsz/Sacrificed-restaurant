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
            "Choose products theme\n",
            "back"
        };

        while (true)
        {
            string? Name;
            Console.Clear();
            switch (SelectionPresent.Show(options, banner).text)
            {
                case "Add product":
                    
                    break;
                case "Show all products":
                    DisplayProducts();
                    break;
                case "Choose products theme":
                    Name = ProductManager.CourseOrThemeValidator("theme");
                    if (Name == null) break;
                    DisplayProducts("theme", Name);
                    break;
                case "Choose products course\n":
                    Name = ProductManager.CourseOrThemeValidator("course");
                    if (Name == null) break;
                    DisplayProducts("course", Name);
                    break;
                case "back" or "":
                    return;
            }
        }

    }

    // Display all products
    public static void DisplayProducts(string filterType = "", string Name = "")
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
            }
            else if (filterType == "theme")
            {
                banner = $"{Name}\n\nChoose a product to edit/delete:\n\n";
                products = ProductManager.GetAllWithinTheme(Name).ToList();
            }
            else
            {
                banner = "${Name}\n\nChoose a product to edit/delete:\n\n";
                products = ProductManager.GetAllWithinCategory(Name).ToList();
            }
                
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

    public static void AddProduct(ProductModel product)
    {

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
                    ProductManager.ProductEditValidator(chosenProduct, "name", false);
                    break;
                case "Edit price":
                    ProductManager.ProductEditValidator(chosenProduct, "price", false);
                    break;
                case "Edit course":
                    ProductManager.ProductEditValidator(chosenProduct, "course", false);
                    break;
                case "Edit theme":
                    ProductManager.ProductEditValidator(chosenProduct, "theme", true);
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
        if(ProductManager.DeleteProductAndRelatedRequests(chosenProduct.ID))
        {
            Console.WriteLine($"{chosenProduct.Name} has been deleted.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return true;
        }
        else
        {
            Console.WriteLine($"Failed to delete {chosenProduct.Name}.");
            Console.WriteLine("Press any key to continue..."); 
            Console.ReadKey();
            return false;
        }
    }
}