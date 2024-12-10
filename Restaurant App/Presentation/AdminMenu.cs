using Project;
using Project.Presentation;

static class AdminMenu
{
    public static void AdminStart()
    {
        List<string> options = new()
        {
            "view",
            "create (admin account)",
            "delete (accounts)",
            "update (themes)\n",
            // "(de)activate tables",
            "back"
        };

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
                // case "(de)activate tables":
                //     DateTime selectedDate = CalendarPresent.Show(DateTime.Now, true);
                //     AdminTableControlPresent.Show(selectedDate);
                //     break;
                case "back":
                    return;
            }
        }
    }
}
