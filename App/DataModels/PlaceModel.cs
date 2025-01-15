using App.DataModels.Utils;

namespace Restaurant;
public class PlaceModel : IModel
{
    public int? ID { get; set; }
    public int? Capacity { get; set; }
    public int? Active { get; set; }
}
