using App.DataAccess.Utils;
using App.DataModels.Product;

namespace Restaurant;

internal class PairLogic
{
    // I/O methods
    public struct Input()
    {
        public string? Product;
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
    private static ProductModel? ToModel(string? pair)
    {
        if (pair == null) return new();

        foreach (var product in Access.Products.Read())
        {
            if (product != null && pair == product.Name)
                return product;
        }

        return null;
    }

    public static int? Start()
    {
        // standalone implement
        List<string?> products = Access.Products.Read().Where(
            x => x.Course == "Main").Select(x => x?.Name).ToList();
        
        // find ID
        string? choice = SelectionPresent.Show(products, banner: "PRODUCT MENU").ElementAt(0).text;
        if (choice == "") return -1;

        int? foodID = Access.Products.GetBy<string>("Name", choice)?.ID;

        // value initialization
        var input = new Input();
        var output = _constr_output(foodID);

        // I/O swap
        PairPresent.Show(ref input, ref output);
        if (input.Product == "") return -1;
        
        ProductModel? model = ToModel(input.Product);

        // deletion of old data
        foreach (var pair in Access.Pairs.GetAllBy<int>("FoodID", foodID ?? -1))
            Access.Pairs.Delete(pair?.ID);

        // linking
        Access.Pairs.Write(
            new PairModel(
                foodID,
                model.ID
            )
        );

        return null;

    }

}