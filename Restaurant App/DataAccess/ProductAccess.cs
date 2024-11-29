namespace Project;
public class ProductAccess : DataAccess<ProductModel>
{
    public ProductAccess() : base(typeof(ProductModel).GetProperties().Select(p => p.Name).ToArray()) { }
}
