using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace App.DataAccess.Allergy;
internal class AllergyAccess : DataAccess<AllergyModel>
{
    internal AllergyAccess() : base(typeof(AllergyModel).GetProperties().Select(p => p.Name).ToArray()) { }

    private protected new bool Delete(int? id)
    {
        IEnumerable<AllerlinkModel> links =
            Access.Allerlinks.Read().Where(lnk => lnk.AllergyID == id);

        foreach (var link in links)
        {
            if (!Access.Allerlinks.Delete(link.ID))
                return false;
        }

        return base.Delete(id);
    }

}
