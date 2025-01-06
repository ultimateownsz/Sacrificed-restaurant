using Project.Presentation;

namespace Project.Logic;
internal class EditAllergyLogic
{

    public enum Mode
    {
        Create,
        Delete
    }

    // I/O methods
    public struct Input()
    {
        public List<string?> Allergies;
    }

    public struct Output()
    {
        public List<string?> Allergies = new();
    }
    private static Output _constr_output()
    {
        // model -> string
        var output = new Output();

        // add all allergies
        Access.Allergies.Read().ForEach(
            x => output.Allergies?.Add(x?.Name));

        return output;
    }

    // specialized methods

    public static void Start()
    {

        // value initialization
        var input = new Input();
        var output = _constr_output();

        // decide mode
        Mode mode = SelectionPresent.Show(["create", "delete"], banner: "EDIT MENU")
            .ElementAt(0).text == "create" ? Mode.Create : Mode.Delete;

        EditAllergyPresent.Show(mode, ref input, ref output);
        switch (mode)
        {
            case Mode.Create:
                
                // simplification
                string? extract = input.Allergies.ElementAt(0);
                
                // input validation
                if (Access.Allergies.Read().Select(
                    x => x.Name.ToLower()).Contains(extract.ToLower()))
                    break;

                if (extract == "")
                    break;
                
                // input is valid
                Access.Allergies.Write(new AllergyModel(
                        input.Allergies.ElementAt(0)));
                break;


            case Mode.Delete:
                
                // input can't be invalid
                foreach (string allergy in input.Allergies)
                    Access.Allergies.Delete(
                        Access.Allergies.GetBy<string>("Name", allergy).ID);
                break;
        }

    }

}