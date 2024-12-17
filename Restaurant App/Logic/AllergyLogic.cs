using Project.Presentation;

namespace Project.Logic;
internal class AllergyLogic
{
    public struct Input()
    {
        public AllergyModel? selected;
    }

    public struct Output()
    {
        public IEnumerable<AllergyModel> allergies = Access.Allergies.Read();
    }

    public static void Start(IModel model)
    {
        // value initialization
        var input = new Input();
        var output = new Output();

        AllergyPresent.Show(ref input, ref output);

    }

}