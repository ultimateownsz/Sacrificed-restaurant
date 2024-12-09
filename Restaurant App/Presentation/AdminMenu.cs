using Project;
using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {

        List<string> options = [
            "view",
            "create (admin account)",
            "delete (accounts)",
            "update (themes)\n",
            "back"
            ];

        while (true) 
        {
            switch (SelectionPresent.Show(options, "ADMIN MENU\n\n(reservations)\n").text)
            {
                case "view":
                    ShowReservations.Show();
                    break;
                case "create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu();
                    break;
                case "update (themes)\n":
                    ThemeView.ThemedEditing();
                    break;
                case "back":
                    return;
            }
        }
    }
}