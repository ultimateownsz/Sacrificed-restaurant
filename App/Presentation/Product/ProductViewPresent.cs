using App.DataModels.Product;
using App.Logic.Allergy;
using App.Logic.Theme;
using Restaurant;

namespace App.Presentation.Product;
static class ProductViewPresent
{

    //This is the main menu for product managment, its nothing special
    public static void ProductMainMenu()
    {
        List<string> options = new()
        {
            "Add product",
            "Show all products",
            "Add allergy to product",
            "Pair drink with food",
            "Choose products course",
            "Choose products theme",
        };

        while (true)
        {
            string? name;
            int? ret;
            // ISSUE: HERE!!
            switch (SelectionPresent.Show(options, banner: $"PRODUCT MENU").ElementAt(0).text)
            {
                case "Add product":
                    AddProduct();
                    break;

                case "Show all products":
                    DisplayProducts();
                    break;
                case "Add allergy to product":
                    ret = AllergyLinkLogic.Start(AllergyLinkLogic.Type.Product, -1);
                    if (ret == -1)
                        continue;
                    else
                        return;
                case "Pair drink with food":
                    ret = PairLogic.Start();
                    if (ret == -1)
                        continue;
                    else
                        return;
                case "Choose products course":
                    while (true)
                    {
                        name = CourseLogic.GetValidCourse();
                        if (name == "REQUEST_PROCESS_EXIT")
                        {
                            Console.WriteLine("");
                            break;
                        }
                        DisplayProducts("course", name);
                    }
                    break;

                case "Choose products theme":
                    while (true)
                    {
                        name = ThemeValidateLogic.GetValidThemeMenu();
                        if (name == "REQUEST_PROCESS_EXIT")
                        {
                            Console.WriteLine("");
                            break;
                        }
                        DisplayProducts("theme", name);
                    }
                    break;

                case "":
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
            if (filterType == "")
            {
                banner = "Choose a product to edit/delete:";
                products = ProductLogic.GetAllProductInfo().ToList();
                if (products.Count == 0)
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
                banner = $"Choose a product to edit/delete:\n\n{name}:";
                products = ProductLogic.GetAllWithinCourse(name).ToList();
                if (products.Count == 0)
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
                if (name == "0")
                {
                    banner = $"Choose a product to edit/delete:\n\nNo theme:";
                }
                else
                {
                    banner = $"Choose a product to edit/delete:\n\n{name}:";
                }
                products = ProductLogic.GetAllWithinTheme(name).ToList();
                if (products.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"There are no products in the {name} theme");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }

            string productSelection = SelectionPresent.Show(products, banner: banner).ElementAt(0).text;

            if (productSelection == "")
            {
                Console.WriteLine("skibidi"); //no idea why but this fixes a lil bug somehow
                return;
            }

            chosenProduct = ProductLogic.ConvertStringChoiceToProductModel(productSelection, filterType, name);
            if (chosenProduct == null)
            {
                Console.WriteLine("rizz");
                return;
            }
            DeleteOrEditChoice(chosenProduct);
        }
    }

    public static void DeleteOrEditChoice(ProductModel chosenProduct)
    {
        List<string> options = new()
        {
            "Edit name",
            "Edit price",
            "Edit course",
            "Edit theme",
            "Delete product",
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner: "Choose a what to do with the product:").ElementAt(0).text)
            {
                case "Edit name":
                    ProductLogic.ProductEditValidator(chosenProduct, "name");
                    break;
                case "Edit price":
                    ProductLogic.ProductEditValidator(chosenProduct, "price");
                    break;
                case "Edit course":
                    ProductLogic.ProductEditValidator(chosenProduct, "course");
                    break;
                case "Edit theme":
                    ProductLogic.ProductEditValidator(chosenProduct, "theme");
                    break;
                case "Delete product":
                    if (DeleteProduct(chosenProduct))
                        return;
                    break;
                case "":
                    return;
            }
        }
    }

    public static bool DeleteProduct(ProductModel chosenProduct)
    {
        Console.Clear();
        string banner = $"Do you want to delete {chosenProduct.Name}";
        List<string> options = new List<string> { "Yes", "No" };
        string selection = SelectionPresent.Show(options, banner: banner).ElementAt(0).text;
        if (selection == "No" || selection == "")
        {
            Console.WriteLine("fix");
            return false;
        }
        Console.Clear();
        if (ProductLogic.DeleteProductAndRelatedRequests(chosenProduct.ID))
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
        ProductModel? newProduct = ProductLogic.ProductValidator();

        if (newProduct != null && newProduct.ID == -1)
            return;



        Console.Clear();
        if (newProduct == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Invalid product info.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        else if (ProductLogic.AddProduct(newProduct))
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
