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
            // ISSUE: HERE!!
            switch (SelectionPresent.Show(options, banner: $"PRODUCT MENU").ElementAt(0).text)
            {
                case "Add product":
                    // not the best way of fixing a visual bug but this will do
                    ControlHelpPresent.Clear();
                    ControlHelpPresent.AddOptions("Escape", "<escape>");
                    ControlHelpPresent.ShowHelp();
                    AddProduct();
                    ControlHelpPresent.ResetToDefault();
                    break;

                case "Show all products":
                    DisplayProducts();
                    break;
                case "Add allergy to product":
                    AllergyLinkLogic.Start(AllergyLinkLogic.Type.Product, -1);
                    break;
                case "Pair drink with food":
                    PairLogic.Start();
                    break;
                case "Choose products course":
                    while (true)
                    {
                        name = CourseLogic.GetValidCourse();
                        if (name == "REQUEST_PROCESS_EXIT" || name == null)
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
                        if (name == "REQUEST_PROCESS_EXIT" || name == null)
                        {
                            Console.WriteLine("");
                            break;
                        }
                        DisplayProducts("theme", name);
                    }
                    break;

                case null:
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
                    ControlHelpPresent.DisplayFeedback($"There are no products in the restaurant");
                    ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
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
                    ControlHelpPresent.DisplayFeedback($"There are no products in {name}");
                    ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
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
                    ControlHelpPresent.DisplayFeedback($"There are no products in the {name} theme", "bottom", "error");
                    ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
            }

            string productSelection = SelectionPresent.Show(products, banner: banner).ElementAt(0).text;

            if (productSelection == null)
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
            switch (SelectionPresent.Show(options, banner: "Choose what to do with the product:").ElementAt(0).text)
            {
                case "Edit name":
                    ControlHelpPresent.Clear();
                    ControlHelpPresent.AddOptions("Escape", "<escape>");
                    ControlHelpPresent.ShowHelp();
                    ProductLogic.ProductEditValidator(chosenProduct, "name");
                    ControlHelpPresent.ResetToDefault();
                    break;
                case "Edit price":
                    ControlHelpPresent.Clear();
                    ControlHelpPresent.AddOptions("Escape", "<escape>");
                    ControlHelpPresent.ShowHelp();
                    ProductLogic.ProductEditValidator(chosenProduct, "price");
                    ControlHelpPresent.ResetToDefault();
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
                case null:
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
        if (selection == "No" || selection == null)
        {
            Console.WriteLine("fix");
            return false;
        }
        Console.Clear();
        if (ProductLogic.DeleteProductAndRelatedRequests(chosenProduct.ID))
        {
            // Console.ForegroundColor = ConsoleColor.Green;
            ControlHelpPresent.DisplayFeedback($"{chosenProduct.Name} has been deleted.");
            ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
            Console.ReadKey();
            // Console.ForegroundColor = ConsoleColor.White;
            return true;
        }
        else
        {
            // Console.ForegroundColor = ConsoleColor.Red;
            ControlHelpPresent.DisplayFeedback($"Failed to delete {chosenProduct.Name}.");
            ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip"); 
            Console.ReadKey();
            // Console.ForegroundColor = ConsoleColor.White;
            return false;
        }
    }

    public static void AddProduct()
    {
        ProductModel? newProduct = ProductLogic.ProductValidator();

        Console.Clear();
        if (newProduct != null && newProduct.ID == -1)
            return;

        if (newProduct == null)
        {
            // Console.ForegroundColor = ConsoleColor.Red;
            ControlHelpPresent.DisplayFeedback($"Invalid product info.");
            ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
            Console.ReadKey();
            // Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        else if (ProductLogic.AddProduct(newProduct))
        {
            // Console.ForegroundColor = ConsoleColor.Green;
            ControlHelpPresent.DisplayFeedback($"{newProduct.Name} has been added.", "bottom", "success");
            ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip");
            Console.ReadKey();
            // Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        else
        {
            // Console.ForegroundColor = ConsoleColor.Red;
            ControlHelpPresent.DisplayFeedback($"Failed to add {newProduct.Name}.");
            ControlHelpPresent.DisplayFeedback("Press any key to continue...", "bottom", "tip"); 
            Console.ReadKey();
            // Console.ForegroundColor = ConsoleColor.White;
            return;
        }
    }
}
