namespace Project;
public class ProductModel: IModel
{
    public int? ID { get; set; }
    public string? Name { get; set; }
    public float? Price { get; set; }
    public string? Course { get; set; }
    public int? ThemeID { get; set; }

    public ProductModel() { }
    public ProductModel(string? name, float? price, 
        string? course, int? themeID, int? id = null)
    {
        ID = id;
        Name = name;
        Price = price;
        Course = course;
        ThemeID = themeID;
    }
}
