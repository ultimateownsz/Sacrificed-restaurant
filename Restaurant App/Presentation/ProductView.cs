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
                case "Delete product":
                    DeleteProduct(chosenProduct);
                    break;
                case "back" or "":
                    return;
            }
        }
    }

    public static void DeleteProduct(ProductModel chosenProduct)
    {

    }
}