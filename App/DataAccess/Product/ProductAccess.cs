using App.DataAccess.Utils;
using App.DataModels.Product;

namespace App.DataAccess.Product;
internal class ProductAccess : DataAccess<ProductModel>
{
    internal ProductAccess() : base(typeof(ProductModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
