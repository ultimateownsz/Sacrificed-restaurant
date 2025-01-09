using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
internal class ReservationAccess : DataAccess<ReservationModel>
{
    internal ReservationAccess() : base(typeof(ReservationModel).GetProperties().Select(p => p.Name).ToArray()) { }

    private protected new bool Delete(int? id)
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
