using App.DataAccess.Utils;
using App.DataModels.Product;

namespace App.DataAccess.Product;
public class PairAccess : DataAccess<PairModel>
{
    public PairAccess() : base(typeof(PairModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
