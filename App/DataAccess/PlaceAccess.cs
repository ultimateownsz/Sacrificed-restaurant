using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
public class PlaceAccess : DataAccess<PlaceModel>
{
    public PlaceAccess() : base(typeof(PlaceModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
