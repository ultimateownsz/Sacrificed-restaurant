using Project.Presentation;

namespace Project.Logic;
internal class LinkAllergyLogic
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
        public int? ID;
        public int? Guest;
        public Type Type;
        
        public List<string?> Allergies = new();
        public List<string?> Highlights = new();
    }
    private static Output _constr_output(Type type, int? id, int? guest)
    {
        // model -> string
        var output = new Output();
        output.Guest = guest;
        output.Type = type;
        output.ID = id;

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

    // helper method for code-simplification
    public static bool IsAllergic(int? userID, int? productID)
        => IsAllergic(
            Access.Users.GetBy<int?>("ID", userID), 
            Access.Products.GetBy<int?>("ID", productID)
            );
    public static bool IsAllergic(UserModel user, ProductModel product)
    {

        // obtain ids of allergies
        IEnumerable<int?> aIDs = Access.Allerlinks.Read().Where(
            x => x?.Personal == 1 && x.EntityID == user.ID).Select(x => x.AllergyID);

        // obtain ids of products that have these allergies
        IEnumerable<int?> pIDs = Access.Allerlinks.Read().Where(
            x => x?.Personal == 0 && aIDs.Contains(x.AllergyID)).Select(x => x.EntityID);

        return pIDs.Contains(product.ID);

    }
    
    public static void Start(Type type, int? id, int? guest = null)
    {
        
        // standalone implement
        if (type == Type.Product)
        {
            // gather self-products
            List<string> products = Access.Products.Read().Select(x => x.Name).ToList();
            string product = SelectionPresent.Show(
                products, banner: "PRODUCT MENU").ElementAt(0).text;
            if (product == "") return;

            // id override
            id = Access.Products.GetBy<string>("Name", product).ID;
        };

        // value initialization
        var input = new Input();
        var output = _constr_output(type, id, guest);

        // I/O swap
        LinkAllergyPresent.Show(ref input, ref output);
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