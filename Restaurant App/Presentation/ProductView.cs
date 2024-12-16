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
        string banner = "Choose a product to edit/delete:\n\n";
        while (true)
        {
            List<string> products = ProductManager.GetAllProductInfo().ToList();
            string productSelection = SelectionPresent.Show(products, banner).text;

            ProductModel? chosenProduct = ProductManager.ConvertStringChoiceToProductModel(productSelection);

            if(productSelection == "")
            {
                return;
            }
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

    public static void EditProductName(ProductModel chosenProduct)
    {

    }

    public static void EditProductPrice(ProductModel chosenProduct)
    {

    }

    public static void EditProductCourse(ProductModel chosenProduct)
    {

    }

    public static void EditProductTheme(ProductModel chosenProduct)
    {

    }

    public static void DeleteProduct(ProductModel chosenProduct)
    {

    }
}