using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {

        List<string> options = [
            "reservation view",
            "reservation filter",
            "reservation update",
            "reservation delete",
            "create admin account",
            "update themes",
            "back"
            ];

        while (true)
        {
            switch (SelectionMenu.Show(options, "ADMIN MENU\n\n"))
            {
                case "reservation view":
                    ShowAllReservations.Show();
                    break;
                case "reservation filter":
                    FilterReservations.Show();
                    break;
                case "reservation update":
                    UpdateReservation.Show();
                    break;
                case "reservation delete":
                    DeleteReservation.Show();
                    break;
                case "create admin account":
                    RegisterUser.CreateAdminAccount();
                    break;
                case "update themes":
                    ThemeView.SetOrUpdateTheme();
                    break;
                case "back":
                    return;
            }
        }
    }
}