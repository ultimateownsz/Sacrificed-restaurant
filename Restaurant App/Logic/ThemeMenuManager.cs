// this class handles all the logic for the themes, adding, deleting, and eventually activating themes

static class ThemeMenuManager
{
    public static bool AddTheme(ThemeMenuModel theme)
    {
        // if (theme == null)
        // {
        //     throw new ArgumentNullException(nameof(theme));
        // }

        if (ThemesAccess.GetById(theme.MenuId) != null)
        {
            return false;
        }
        ThemesAccess.Write(theme);
        return true;
    }

    public static bool DeleteTheme(ThemeMenuModel theme)
    {
        if (theme == null)
        {
            throw new ArgumentNullException(nameof(theme));
        }

        if (ThemesAccess.GetById(theme.MenuId) == null)
        {
            return false;
        }
        else
        {
            ThemesAccess.Delete(theme.MenuId);
        }
        return true;
    }

    public static List<ThemeMenuModel> GetAllThemes()
    {
        return ThemesAccess.GetAll().ToList();
    }
}

//     public static void ActiveTheme()
//     {
//         var themes = ThemesAccess.GetAll();
//         return themes != null && themes.Count() > 0;
//     }
// }

// old code to store products in a dictionary seperate from the database
//     public void AddProduct(ProductModel product)
//     {
//         if (products.ContainsKey(product.ProductName))
//         {
//             products[product.ProductName].Quantity += product.Quantity;
//         }
//         else
//         {
//             products.Add(product.ProductName, product);
//         }
//     }

//     public ProductModel GetProduct(string productName)
//     {
//         if (products.ContainsKey(productName))
//         {
//             return products[productName];
//         }
//         else
//         {
//             return null;
//         }
//     }

//     public void RemoveProduct(string productName)
//     {
//         if (products.ContainsKey(productName))
//         {
//             products.Remove(productName);
//         }
//     }

//     public Dictionary<string, ProductModel> GetAllProducts()
//     {
//         return products;
//     }
// }