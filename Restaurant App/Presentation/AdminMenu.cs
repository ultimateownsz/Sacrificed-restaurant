using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {

        List<string> options = [
            "view",
            "filter",
            "update",
            "delete\n",
            "create (admin account)",
            "update (themes)\n",
            "back"
            ];

        while (true)
        {
            switch (SelectionMenu.Show(options, "ADMIN MENU\n\n(reservations)\n"))
            {
                case "view":
                    ShowAllReservations.Show();
                    break;
                case "filter":
                    FilterReservations.Show();
                    break;
                case "update":
                    UpdateReservation.Show();
                    break;
                case "delete\n":
                    DeleteReservation.Show();
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