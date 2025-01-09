using App.DataAccess.Utils;
using App.DataModels.Product;

namespace App.DataAccess.Product;
public class ProductAccess : DataAccess<ProductModel>
{
    public ProductAccess() : base(typeof(ProductModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
