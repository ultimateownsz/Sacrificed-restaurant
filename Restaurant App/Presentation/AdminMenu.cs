using Project;
using Project.Presentation;
using Presentation;
using Project.Logic;

static class AdminMenu
{
    public static void AdminStart(UserModel acc)
    {
        List<string> options = new()
        {
            "edit reservations",
            "create (admin account)",
            "delete (accounts)",
            "update (themes)",
            "(de)activate tables",
            "edit (allergy/diet)",
            "add (allergy/diet to product)",
            "pair (drink to product)\n",
            "back"
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner: "ADMIN MENU").ElementAt(0).text)
            {
                case "edit reservations":
                    ShowReservations.Show(acc);
                    break;
                case "create (admin account)":
                    CreateAdminOptions.Options(acc);
                    break;
                case "delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu(acc);
                    break;
                case "update (themes)":
                    ThemeView.ThemedEditing();
                    break;
                case "(de)activate tables":
                    AdminTableControlPresent.Show();
                    break;
                case "edit (allergy/diet)":
                    EditAllergyLogic.Start();
                    break;
                case "add (allergy/diet to product)":
                    LinkAllergyLogic.Start(LinkAllergyLogic.Type.Product, 0);
                    break;
                case "pair (drink to product)\n":
                    PairLogic.Start(0);
                    break;
                case "back":
                    return;
            }
        }
    }
}
