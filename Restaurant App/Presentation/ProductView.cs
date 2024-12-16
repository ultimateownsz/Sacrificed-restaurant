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

        string selection = SelectionPresent.Show(options, banner).text;
    }

    public static void EditProduct()
    {

    }

    public static void DeleteProduct()
    {

    }
}