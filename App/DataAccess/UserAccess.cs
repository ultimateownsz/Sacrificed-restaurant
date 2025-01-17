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
            (
                from   lnk in Access.Allerlinks.Read()
                where (lnk.Personal == 1) && (lnk.ID == id)
                select lnk
            );

        IEnumerable<ReservationModel?> reservations =
            (
                from   res in Access.Reservations.Read()
                where (res.ID == id)
                select res
            );

        // delete all entries
        foreach (var allerlink in allerlinks)
        {
            if (!Access.Allerlinks.Delete(allerlink?.ID))
                return false;
        }

        foreach (var reservation in reservations)
        {
            if (!Access.Reservations.Delete(reservation?.ID))
                return false;
        }
        
        return base.Delete(id);
    }

}
