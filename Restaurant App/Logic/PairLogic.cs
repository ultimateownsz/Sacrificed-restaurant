namespace Project.Logic;
using Project.Presentation;

internal class PairLogic
{
    // I/O methods
    public struct Input()
    {
        public List<string>? Products;
    }

    public struct Output()
    {
        public List<string?> Products = new();
        public List<string?> Highlights = new();
    }

    private static Output _constr_output(int? id)
    {
        // model -> string
        var output = new Output();

        // add all allergies
        List<string> labels = ["Beverage",];
        Access.Products.Read().Where(x => labels.Contains(x?.Course))
            .ToList().ForEach(x => output.Products.Add(x?.Name));

        // add those the product has
        foreach (var pair in Access.Pairs.Read().Where(x => x.FoodID == id))
            output.Highlights.Add(Access.Products.GetBy<int?>("ID", pair?.DrinkID)?.Name);

        return output;
    }

    // specialized methods
    private static List<ProductModel> ToModels(List<string?>? pairs)
    {
        if (pairs == null) return new();

        var models = new List<ProductModel>();
        foreach (var product in Access.Products.Read())
        {
            if (product != null && pairs.Contains(product.Name))
                models.Add(product);
        }

        return models;
    }

    public static void Start(int? foodID)
    {

        // <
        // THIS CODE SEGMENT HAS BEEN IMPLEMENTED TO PREVENT "HANI-FATIGUE"
        // REMOVE THIS LOGIC AFTER HANI HAS SPED THE FUCK UP
        List<string> labels = ["Beverage",];
        string _product = SelectionPresent.Show(Access.Products.Read().Where(
            x => !labels.Contains(x.Course)).Select(x => x.Name).ToList(), banner: "PRODUCT MENU").ElementAt(0).text;
        foodID = Access.Products.GetBy<string>("Name", _product).ID;
        // >

        // value initialization
        var input = new Input();
        var output = _constr_output(foodID);

        // I/O swap
        PairPresent.Show(ref input, ref output);
        List<ProductModel> models = ToModels(input.Products);

        // deletion of old data
        foreach (var pair in Access.Pairs.GetAllBy<int>("FoodID", foodID ?? -1))
            Access.Pairs.Delete(pair?.ID);

        foreach (var product in models)
        {
            // linking
            Access.Pairs.Write(
                new PairModel(
                    foodID,
                    product.ID
                )
            );
        }
    }

}