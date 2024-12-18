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
        public List<string>? selected;
    }

    public struct Output()
    {
        public List<string?> allergies = new();
    }
    private static Output _constr_output()
    {
        // model -> string
        var output = new Output();
        Access.Allergies.Read().ForEach(
            m => output.allergies?.Add(m?.Name));
        
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
        var output = _constr_output();

        // I/O swap
        AllergyPresent.Show(ref input, ref output);
        List<AllergyModel> models = ToModels(input.selected);

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