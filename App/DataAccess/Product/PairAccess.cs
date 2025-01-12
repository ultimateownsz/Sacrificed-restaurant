using App.DataAccess.Utils;
using App.DataModels.Product;

namespace App.DataAccess.Product;
internal class PairAccess : DataAccess<PairModel>
{
    internal PairAccess() : base(typeof(PairModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
