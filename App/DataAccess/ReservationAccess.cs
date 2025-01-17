using App.DataAccess.Utils;
using App.DataModels.Allergy;
using Restaurant;

namespace App.DataAccess;
public class ReservationAccess : DataAccess<ReservationModel>
{
    public ReservationAccess() : base(typeof(ReservationModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public override bool Purge(int? id)
    {
        IEnumerable<RequestModel?> requests =
        (
            from val in Access.Requests.Read()
            where val?.ID == id
            select val
        );

        foreach (var request in requests)
        {
            if (!Access.Requests.Delete(request?.ID))
                return false;
        }

        return true;
    }

    public new bool Delete(int? id)
        => Purge(id) && base.Delete(id);

}
