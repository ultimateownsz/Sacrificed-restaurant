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
            "show (reservation) orders",
            "(de)activate tables",
            "edit (allergy/diet)\n",
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
                case "show (reservation) orders":
                    ReservationDetails.ShowOrders(acc);
                    break;
                case "(de)activate tables":
                    AdminTableControlPresent.Show();
                    break;
                case "edit (allergy/diet)\n":
                    EditAllergyLogic.Start();
                    break;
                case "back":
                    return;
            }
        }
    }
}
