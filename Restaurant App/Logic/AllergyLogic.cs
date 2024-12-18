using Project.Presentation;

namespace Project.Logic;
internal class AllergyLogic
{
    public enum Type
    {
        User,
        Product
    }

    // I/O methods
    public struct Input()
    {
        public List<string>? Allergies;
    }

    public struct Output()
    {
        public List<string?> Allergies = new();
        public List<string?> Highlights = new();
    }
    private static Output _constr_output(int? id)
    {
        // model -> string
        var output = new Output();
        
        // add all allergies
        Access.Allergies.Read().ForEach(
            x => output.Allergies?.Add(x?.Name));

        // add those the user has
        foreach (AllerlinkModel? model in Access.Allerlinks.Read().Where(x => x.EntityID == id))
            output.Highlights.Add(Access.Allergies.GetBy<int?>("ID", model?.AllergyID)?.Name);

        return output;
    }

    // specialized methods
    private static List<AllergyModel> ToModels(List<string?>? allergies)
    {
        if (allergies == null) return new();

        var models = new List<AllergyModel>();
        foreach (var allergy in Access.Allergies.Read())
        {
            if (allergy != null && allergies.Contains(allergy.Name))
                models.Add(allergy);
        }
        
        return models;
    }

    public static void Start(Type type, int? id)
    {
        // value initialization
        var input = new Input();
        var output = _constr_output(id);

        // I/O swap
        AllergyPresent.Show(ref input, ref output);
        List<AllergyModel> models = ToModels(input.Allergies);

        // deletion of old data
        foreach (var allergy in Access.Allerlinks.GetAllBy<int>("EntityID", id ?? -1))
            Access.Allerlinks.Delete(allergy?.ID);

        foreach (var allergy in models)
        {
            // linking
            Access.Allerlinks.Write(
                new AllerlinkModel(
                    id, 
                    allergy.ID,
                    (type == Type.User) ? 1 : 0
                )
            );
        }
    }

}