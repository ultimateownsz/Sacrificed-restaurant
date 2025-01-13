using App.DataAccess.Utils;
using App.DataModels.Allergy;

namespace Restaurant;
internal class UserAccess: DataAccess<UserModel>
{
    internal UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }

    internal new bool Delete(int? id)
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
