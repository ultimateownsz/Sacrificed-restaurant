using Presentation;
using Project.Logic;
using Project.Presentation;

namespace Project;
static class Menu
{
    static public void Start()
    {

        while (true)
        {
            // Console.Clear();
            if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
            
            var userChoice = SelectionPresent.Show(
                ["login", "register\n", "exit"],
                "MAIN MENU\n\n").text;
            switch (userChoice)
            {
                case "login":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    if (MenuLogic.Login() == "continue")
                        continue;

                    break;

                case "register\n":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    RegisterUser.CreateAccount();
                    continue;

                case "exit":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    return;

                default:
                    continue;
            }

            break;
        }
    }

    public static void ShowUserMenu(UserModel acc)
    {
        while (true)
        {
            if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
            Console.Clear();
            var userChoice = SelectionPresent.Show(
                ["reserve", "view reservations", "logout"],
                "USER MENU\n\n").text;
            switch (userChoice)
            {
                case "reserve":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    MakingReservations.CalendarNavigation(acc);
                    break;

                case "view reservations":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    MakingReservations.UserOverViewReservation(acc);
                    break;

                case "logout":
                    if (SelectionPresent.EscapeKeyPressedWithConfirmation()) return;
                    return;

                default:
                    break;
            }
        }
    }
}
