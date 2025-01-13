using App.DataModels.Utils;

namespace Restaurant;
public class PlaceModel : IModel
{
    public int? ID { get; set; }
    public int? Capacity { get; set; }
    public int? Active { get; set; }

    public PlaceModel() { }
    public PlaceModel(int? capacity, int? active, int? id = null)
    {
        ID = id;
        Active = active;
        Capacity = capacity;
    }
}
