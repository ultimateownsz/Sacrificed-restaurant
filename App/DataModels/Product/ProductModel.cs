using App.DataModels.Utils;

namespace App.DataModels.Product;
public class ProductModel : IModel
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Course { get; set; }
    public int? ThemeID { get; set; }

    public ProductModel() { }
}
