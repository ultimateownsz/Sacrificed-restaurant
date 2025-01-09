using App.DataModels.Utils;

namespace App.DataModels.Allergy;
public class AllerlinkModel : IModel
{
    public int? ID { get; set; }
    public int? EntityID { get; set; }
    public int? AllergyID { get; set; }
    public int? Personal { get; set; }

    public AllerlinkModel() { }
    public AllerlinkModel(int? entityID, int? allergyID, int? personal, int? id = null)
    {
        ID = id;
        EntityID = entityID;
        AllergyID = allergyID;
        Personal = personal;
    }
}
