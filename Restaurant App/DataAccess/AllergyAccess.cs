namespace Project;
public class AllergyAccess : DataAccess<AllergyModel>
{
    public AllergyAccess() : base(typeof(AllergyModel).GetProperties().Select(p => p.Name).ToArray()) { }

}