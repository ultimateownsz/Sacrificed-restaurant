namespace Project;
public class AllergyModel : IModel
{
    public int? ID { get; set; }
    public string? Name { get; set; }

    public AllergyModel() { }
    public AllergyModel(string? name, int? id = null)
    {
        ID = id;
        Name = name;
    }
}
