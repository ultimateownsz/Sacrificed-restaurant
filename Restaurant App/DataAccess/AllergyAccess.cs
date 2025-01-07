namespace Project;
public class AllergyAccess : DataAccess<AllergyModel>
{
    public AllergyAccess() : base(typeof(AllergyModel).GetProperties().Select(p => p.Name).ToArray()) { }

    public new bool Delete(int? id)
    {
        IEnumerable<AllerlinkModel> links =
            Access.Allerlinks.Read().Where(lnk => lnk.ID == id);

        foreach (var link in links)
        {
            if (!Access.Allerlinks.Delete(link.ID))
                return false;
        }

        return base.Delete(id);
    }

}
