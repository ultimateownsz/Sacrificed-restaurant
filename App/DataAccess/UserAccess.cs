using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace Restaurant;
public class UserAccess: DataAccess<UserModel>
{
    public UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }


    public override bool Purge(int? id)
    {

        IEnumerable<AllerlinkModel?> allerlinks =
        (
            from val in Access.Allerlinks.Read()
            where val?.ID == id
            select val
        );

        IEnumerable<ReservationModel?> reservations =
        (
            from val in Access.Reservations.Read()
            where val?.ID == id
            select val
        );

        foreach (var lnk in allerlinks)
        {
            if (!Access.Allerlinks.Delete(lnk?.ID))
                return false;
        }

        foreach (var res in reservations)
        {
            if (!Access.Reservations.Delete(res?.ID))
                return false;
        }

        return true;
    }

    public new bool Delete(int? id)
        => Purge(id) && base.Delete(id);


}
