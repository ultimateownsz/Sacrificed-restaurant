using Project;
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
            switch (SelectionPresent.Show(options, "ADMIN MENU\n\n(reservations)\n").text)
            {
                case "view":
                    ShowReservations.Show();
                    break;
                case "create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "update (themes)\n":
                    ThemeView.YearAndMonthInputs();
                    break;
                case "back":
                    return;
            }
        }
    }
}