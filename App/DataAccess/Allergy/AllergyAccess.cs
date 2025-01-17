using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace App.DataAccess.Allergy;
public class AllergyAccess : DataAccess<AllergyModel>
{
    public AllergyAccess() : base(typeof(AllergyModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public override bool Purge(int? id)
    {
        IEnumerable<AllerlinkModel?> links =
        (
            from lnk in Access.Allerlinks.Read()
            where lnk?.AllergyID == id
            select lnk
        );

        foreach (var link in links)
        {
            if (!Access.Allerlinks.Delete(link?.ID))
                return false;
        }

        return true;
    }

    public new bool Delete(int? id)
        => Purge(id) && base.Delete(id);
    

}
