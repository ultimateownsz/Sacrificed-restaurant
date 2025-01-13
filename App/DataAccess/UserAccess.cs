using App.DataAccess.Utils;
using App.DataModels.Allergy;
using App.DataModels.Utils;

namespace Restaurant;
public class UserAccess: DataAccess<UserModel>
{
    public UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public new bool Delete(int? id)
    {

        // first remove all reservations
        foreach (var res in Access.Reservations.Read().Where(x => x.UserID == id))
            if (!Access.Reservations.Delete(res.ID))
                return false;

        // ... and then remove all allerlinks
        foreach (var lnk in Access.Allerlinks.Read().Where(x => x.EntityID == id && x.Personal == 1))
            if (!Access.Allerlinks.Delete(lnk.ID))
                return false;

        return base.Delete(id);
    }

}
