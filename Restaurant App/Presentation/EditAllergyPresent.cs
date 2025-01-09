using Project.Logic;

namespace Project.Presentation;

internal class EditAllergyPresent
{

    public static List<string?>? input_request()
    {
        string? input;
        while (true)
        {
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Exit", "<escape>");
            ControlHelpPresent.ShowHelp();
            input = Terminable.ReadLine("allergy name: ");
            if (input == null)
            {
                ControlHelpPresent.ResetToDefault();
                return null;
            }
            
            break;
        }

        return new() { input };
    }

    public static List<string?>? selection_request()
    {
        string banner = "ALLERGY/DIET MENU";
        List<string> options = Access.Allergies.Read().Select(x => x.Name).ToList();
        List<SelectionLogic.Selection> allergies = SelectionPresent.Show(
            options, banner: banner, mode: SelectionLogic.Mode.Multi);

        if (allergies.Count == 1 && allergies.First().index == -1)
            return null;
        
        return allergies.Select(x => x.text).ToList();
    }
    


    public static void Show(EditAllergyLogic.Mode mode, 
        ref EditAllergyLogic.Input input, ref EditAllergyLogic.Output output)
    {

        switch (mode)
        {
            case EditAllergyLogic.Mode.Create:
                input.Allergies = input_request();
                break;
            
            case EditAllergyLogic.Mode.Delete:
                input.Allergies = selection_request();
                break;
        }

    }

}
