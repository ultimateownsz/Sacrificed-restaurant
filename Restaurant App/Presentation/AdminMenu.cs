using Project;
using Project.Presentation;
using Presentation;

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
            "update (products)",
            "(de)activate tables\n",
            "back"
        };

        while (true)
        {
            var selection = SelectionPresent.Show(options, banner: "ADMIN MENU").ElementAt(0).text;

            if (string.IsNullOrEmpty(selection)) return;

            switch (selection)
            {
                case "edit reservations":
                    ShowReservations.Show(acc);
                    break;
                case "create (admin account)":
                    // CreateAdminOptions.Options(acc);
                    RegisterUser.CreateAccount(true);
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
                case "update (products)":
                    ProductView.ProductMainMenu();
                    break;
                case "(de)activate tables\n":
                    AdminTableControlPresent.Show();
                    break;
                case "back":
                    return;
            }
        }
    }
}
