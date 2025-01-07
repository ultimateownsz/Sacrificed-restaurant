using Project.Logic;

namespace Project.Presentation;
internal class LinkAllergyPresent
{
    public static void Show(ref LinkAllergyLogic.Input input, ref LinkAllergyLogic.Output output)
    {
        // get all selected allergies
        string banner = (output.Type == LinkAllergyLogic.Type.User) ? $"(GUEST {output.Guest})" : "";
        List<SelectionLogic.Selection> allergies = SelectionPresent.Show(
            output.Allergies, output.Highlights, banner: $"DIET/ALLERGIES MENU {banner}", SelectionLogic.Mode.Multi);

        List<string?> stringified = new();
        foreach (var selection in allergies)
        {
            stringified.Add(selection.text);
        }

        input.Allergies = stringified;
    }
       
}
