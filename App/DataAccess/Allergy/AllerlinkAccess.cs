using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace App.DataAccess.Allergy;
public class AllerlinkAccess : DataAccess<AllerlinkModel>
{
    public AllerlinkAccess() : base(typeof(AllerlinkModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
