using App.DataAccess.Utils;
using App.DataModels.Product;
using Dapper;

namespace App.DataAccess.Product;
public class ProductAccess : DataAccess<ProductModel>
{
    public ProductAccess() : base(typeof(ProductModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public List<ProductModel> GetAll()
    {
        string sql = $"SELECT * FROM {_table}";
        return _db.Query<ProductModel>(sql, null).ToList();
    }
}
