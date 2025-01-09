namespace Restaurant;
internal class AllergyLinkPresent
{
    public static void Show(ref AllergyLinkLogic.Input input, ref AllergyLinkLogic.Output output)
    {
        // get all selected allergies
        string banner = (output.ID == -1) ? $"(GUEST {output.Guest})" : "";
        List<SelectionLogic.Selection>  allergies = SelectionPresent.Show(
            output.Allergies, output.Highlights, banner: $"DIET/ALLERGIES MENU {banner}", SelectionLogic.Mode.Multi);

        List<string?> stringified = new();
        foreach (var selection in allergies)
        {
            stringified.Add(selection.text);
        }

        input.Allergies = stringified;
    }
       
}
