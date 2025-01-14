using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace Restaurant;
public class UserAccess: DataAccess<UserModel>
{
    public UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public new bool Delete(int? id)
    {

        // collect information
        IEnumerable<AllerlinkModel?> allerlinks =
            Access.Allerlinks.Read().Where(lnk => lnk?.Personal == 1 && lnk.ID == id);

        IEnumerable<ReservationModel?> reservations =
            Access.Reservations.Read().Where(res => res?.UserID == id);

        // delete all entries
        foreach (var allerlink in allerlinks)
            if (!Access.Allerlinks.Delete(allerlink?.ID))
                return false;

        foreach (var reservation in reservations)
            if (!Access.Reservations.Delete(reservation?.ID))
                return false;
        
        return base.Delete(id);
    }

}
