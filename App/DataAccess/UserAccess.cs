using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace Restaurant;
public class UserAccess: DataAccess<UserModel>
{
    public UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public new bool Delete(int? id)
    {
        IEnumerable<ReservationModel> reservations =
            Access.Reservations.Read().Where(res => res.UserID == id);

        foreach (var reservation in reservations)
        {
            if (!Access.Reservations.Delete(reservation.ID))
                return false;
        }

        return base.Delete(id);
    }

}
