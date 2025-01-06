namespace Project;
public class AllerlinkAccess : DataAccess<AllerlinkModel>
{
    public AllerlinkAccess() : base(typeof(AllerlinkModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
