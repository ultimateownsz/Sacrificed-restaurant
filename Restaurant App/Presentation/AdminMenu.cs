using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {

        List<string> options = [
            "view",
            "create (admin account)",
            "update (themes)\n",
            "back"
            ];

        while (true)
        {
            switch (SelectionMenu.Show(options, "ADMIN MENU\n\n(reservations)\n"))
            {
                case "view":
                    ShowReservations.Show();
                    break;
                case "create (admin account)":
                    RegisterUser.CreateAdminAccount();
                    break;
                case "update (themes)\n":
                    ThemeView.SetOrUpdateTheme();
                    break;
                case "back":
                    return;
            }
        }
    }
}