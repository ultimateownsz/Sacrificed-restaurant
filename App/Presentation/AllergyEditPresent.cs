namespace Restaurant;
internal class AllergyEditPresent
{

    public static List<string?>? input_request()
    {
        string? input;
        while (true)
        {
            input = TerminableUtilsPresent.ReadLine("allergy name: ");
            if (input == null) return null;
            
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
    


    public static void Show(AllergyEditLogic.Mode mode, 
        ref AllergyEditLogic.Input input, ref AllergyEditLogic.Output output)
    {

        switch (mode)
        {
            case AllergyEditLogic.Mode.Create:
                input.Allergies = input_request();
                break;
            
            case AllergyEditLogic.Mode.Delete:
                input.Allergies = selection_request();
                break;
        }

    }

}
