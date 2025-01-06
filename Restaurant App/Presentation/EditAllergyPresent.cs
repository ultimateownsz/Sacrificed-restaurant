using Project.Logic;

namespace Project.Presentation;

internal class EditAllergyPresent
{

    public static List<string?> input_request()
    {
        Console.Clear();
        Console.Write("allergy name: ");
        return new() { Console.ReadLine() };
    }

    public static List<string?> selection_request()
        => SelectionPresent.Show(
            options: Access.Allergies.Read().Select(x => x.Name).ToList(),
            banner: "ALLERGY/DIET MENU",
            mode: SelectionLogic.Mode.Multi).Select(x => x.text).ToList();
    


    public static void Show(EditAllergyLogic.Mode mode, 
        ref EditAllergyLogic.Input input, ref EditAllergyLogic.Output output)
    {

        switch (mode)
        {
            case EditAllergyLogic.Mode.Create:
                input.Allergies = input_request();
                return;
            
            case EditAllergyLogic.Mode.Delete:
                input.Allergies = selection_request();
                return;
        }    

    }

}
