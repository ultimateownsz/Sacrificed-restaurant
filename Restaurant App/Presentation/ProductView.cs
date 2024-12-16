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
            if(productSelection == "")
            {
                return;
            }
        }


    }

    public static void DeleteOrEditChoice()
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
                    EditProductName();
                    break;
                case "Edit price":
                    EditProductPrice();
                    break;
                case "Edit course":
                    EditProductCourse();
                    break;
                case "Edit theme":
                    EditProductTheme();
                    break;
                case "Delete product":
                    DeleteProduct();
                    break;
                case "back" or "":
                    return;
            }
        }
    }

    public static void EditProductName()
    {
        
    }

    public static void EditProductPrice()
    {

    }

    public static void EditProductCourse()
    {

    }

    public static void EditProductTheme()
    {

    }

    public static void DeleteProduct()
    {

    }
}