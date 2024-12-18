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
            "(de)activate tables\n",
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
                    RegisterUser.CreateAccount(true);
                    break;
                case "delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu(acc);
                    break;
                case "update (themes)":
                    ThemeView.ThemedEditing();
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
