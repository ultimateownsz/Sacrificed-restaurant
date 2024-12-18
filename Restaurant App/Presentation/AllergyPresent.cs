using Project.Logic;

namespace Project.Presentation;
internal class AllergyPresent
{
    public static List<string> Show(ref AllergyLogic.Input input, ref AllergyLogic.Output output)
        => input.selected = SelectionPresent.Show(
            Access.Allergies.Read().Select(x => x.Name).ToList(),
                "DIET/ALLERGIES\n\n", SelectionLogic.Mode.Multi
            ).text;

}
