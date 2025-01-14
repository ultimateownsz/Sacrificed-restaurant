using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace App.DataAccess.Allergy;
internal class AllerlinkAccess : DataAccess<AllerlinkModel>
{
    internal AllerlinkAccess() : base(typeof(AllerlinkModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
