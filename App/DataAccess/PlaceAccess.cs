using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
internal class PlaceAccess : DataAccess<PlaceModel>
{
    internal PlaceAccess() : base(typeof(PlaceModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
