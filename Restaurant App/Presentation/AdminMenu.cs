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
            "Update  table availability",
            "Update  reservations",
            "Update  allergies",
            "Update  products",
            "Update  themes",
            "Create  admin",
            "Delete  account",
            "Display orders\n",
            "Logout"
        };

        while (true)
        {
            switch (SelectionPresent.Show(options, banner: "ADMIN MENU").ElementAt(0).text)
            {
                case "Update  reservations":
                    ShowReservations.Show(acc);
                    break;
                case "Create  admin":
                    CreateAdminOptions.Options(acc);
                    break;
                case "Delete  account":
                    DeleteAccount.ShowDeleteAccountMenu(acc);
                    break;
                case "Update  themes":
                    ThemeView.ThemedEditing();
                    break;
                case "Display orders\n":
                    ReservationDetails.ShowOrders(acc);
                    break;
                case "Update  products":
                    ProductView.ProductMainMenu();
                    break;
                case "Update  table availability":
                    AdminTableControlPresent.Show();
                    break;
                case "Update  allergies":
                    EditAllergyLogic.Start();
                    break;
                case "Logout":
                    return;
            }
        }
    }
}
