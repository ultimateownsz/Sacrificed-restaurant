namespace Restaurant;
public class PairAccess : DataAccess<PairModel>
{
    public PairAccess() : base(typeof(PairModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
