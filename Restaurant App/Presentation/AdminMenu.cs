using Project;
using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {

        List<string> options = [
            "View reservations by date",
            "Create (admin account)",
            "Delete (accounts) (TODO)",
            "Update (themes)\n",
            "Back"
            ];

        while (true) 
        {
            switch (SelectionPresent.Show(options, "ADMIN MENU\n\n").text)
            {
                case "View reservations by date":
                    ShowReservations.Show();
                    break;
                case "Create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "Delete (accounts)":
                //  todo: @Dyl4n01 is hiermee bezig
                    break;
                case "Update (themes)\n":
                    ThemeView.SetOrUpdateTheme();
                    break;
                case "Back":
                    return;
            }
        }
    }
}