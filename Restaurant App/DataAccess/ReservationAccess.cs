namespace Project;
public class ReservationAccess : DataAccess<ReservationModel>
{
    public ReservationAccess() : base(typeof(ReservationModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
