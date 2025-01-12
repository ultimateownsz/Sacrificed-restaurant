using App.Logic.Allergy;
using App.Presentation.Product;
using App.Presentation.Reservation;
using App.Presentation.Table;
using Restaurant;

namespace App.Presentation.Admin;

static class AdminMenuPresent
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
            var selection = SelectionPresent.Show(options, banner: "ADMIN MENU").ElementAt(0).text;

            if (string.IsNullOrEmpty(selection)) return;

            switch (selection)
            {
                case "Update  reservations":
                    ReservationShowPresent.Show(acc);
                    break;
                case "Create  admin":
                    AdminCreateOptionsPresent.Options(acc);
                    break;
                case "Delete  account":
                    AdminDeleteAccountPresent.ShowDeleteAccountMenu(acc);
                    break;
                case "Update  themes":
                    AdminThemePresent.ThemedEditing();
                    break;
                case "Display orders\n":
                    ReservationDetailsPresent.ShowOrders(acc);
                    break;
                case "Update  products":
                    ProductViewPresent.ProductMainMenu();
                    break;
                case "Update  table availability":
                    TableControlPresent.Show();
                    break;
                case "Update  allergies":
                    AllergyEditLogic.Start();
                    break;
                case "Logout":
                    return;
            }
        }
    }
}