using App.Logic.Allergy;
using App.Presentation.Reservation;
using Restaurant;

namespace App.Presentation.User;

static class UserMenuPresent
{
    public static void ShowUserMenu(UserModel acc)
    {
        while (true)
        {
            Console.Clear();
            var options = new List<string> {
                "Make reservation", "View reservation", "Specify allergies", "Delete account\n", "Logout" };
            var selection = SelectionPresent.Show(options, banner: "USER MENU").ElementAt(0).text;

            switch (selection)
            {
                case "Make reservation":
                    // Directly call MakingReservation without calendar in Menu
                    ReservationMakePresent.MakingReservation(acc);
                    break;

                case "View reservation":
                    FuturePastResrvations.Show(acc, false); // using the new method - commented the old method just in case
                    break;

                case "Delete account\n":
                    UserDeleteAccountPresent.DeleteAccount(acc);
                    ControlHelpPresent.ResetToDefault(); // fallback to selection controls
                    break;

                case "Specify allergies":
                    AllergyLinkLogic.Start(AllergyLinkLogic.Type.User, acc.ID);
                    break;

                case "Logout":
                    return;

            }
        }
    }

}
