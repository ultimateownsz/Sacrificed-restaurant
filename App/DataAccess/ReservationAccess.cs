using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
public class ReservationAccess : DataAccess<ReservationModel>
{
    public ReservationAccess() : base(typeof(ReservationModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public new bool Delete(int? id)
    {
        IEnumerable<RequestModel> requests =
            Access.Requests.Read().Where(req => req.ReservationID == id);

        foreach (var request in requests)
        {
            if (!Access.Requests.Delete(request.ID))
                return false;
        }

        return base.Delete(id);
    }

}
