using App.Presentation.Allergy;
using Restaurant;

namespace App.Logic.Allergy;
internal class AllergyEditLogic
{

    public enum Mode
    {
        Create,
        Delete,
        Terminate
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

        while (true)
        {

            // decide mode
            string choice = SelectionPresent.Show(
                ["create", "delete"], banner: "EDIT MENU").ElementAt(0).text;

            Mode mode = choice == "create"
                ? Mode.Create : choice == "delete"
                ? Mode.Delete : Mode.Terminate;

            AllergyEditPresent.Show(mode, ref input, ref output);
            switch (mode)
            {
                case Mode.Create:

                    if (input.Allergies == null)
                        continue;

                    // simplification
                    string? extract = input.Allergies.ElementAt(0);

                    // input validation
                    if (Access.Allergies.Read().Select(
                        x => x.Name.ToLower()).Contains(extract.ToLower()))
                        break;

                    // input is valid
                    Access.Allergies.Write(new AllergyModel(
                            input.Allergies.ElementAt(0)));
                    break;


                case Mode.Delete:

                    if (input.Allergies == null)
                        continue;

                    foreach (string allergy in input.Allergies)
                        Access.Allergies.Delete(
                            Access.Allergies.GetBy("Name", allergy).ID);

                    break;

                case Mode.Terminate:
                    return;
            }

        }

    }

}