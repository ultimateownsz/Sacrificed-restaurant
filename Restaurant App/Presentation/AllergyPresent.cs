using Project.Logic;

namespace Project.Presentation;
internal class AllergyPresent
{
    public static void Show(ref AllergyLogic.Input input, ref AllergyLogic.Output output)
    {
        // get all selected allergies
        List<SelectionLogic.Selection> allergies = SelectionPresent.Show(
            output.Allergies, output.Highlights, banner: "DIET/ALLERGIES", SelectionLogic.Mode.Multi);

        List<string?> stringified = new();
        foreach (var selection in allergies)
        {
            stringified.Add(selection.text);
        }

        input.Allergies = stringified;
    }
       
}
