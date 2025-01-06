using Project.Logic;

namespace Project.Presentation;
internal class PairPresent
{
    public static void Show(ref PairLogic.Input input, ref PairLogic.Output output)
    {
        // get all selected pairs
        List<SelectionLogic.Selection> products = SelectionPresent.Show(
            output.Products, output.Highlights, "PAIR MENU", SelectionLogic.Mode.Multi);

        List<string?> stringified = new();
        foreach (var selection in products)
        {
            stringified.Add(selection.text);
        }

        input.Products = stringified;
    }

}
