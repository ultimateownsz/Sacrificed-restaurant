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
            Console.Clear();
            switch (SelectionPresent.Show(["login", "register\n", "exit"], "MAIN MENU\n\n").text)
            {
                case "login":

                    if (MenuLogic.Login() == "continue")
                        continue;

                    break;

                case "register\n":
                    
                    RegisterUser.CreateUserAccount();
                    continue;

                case "exit":
                    return;

                default:
                    continue;
            }

            // valid input has been provided at this point
            break;
        }
    }

    public static void ShowUserMenu(UserModel acc)
    {
        while (true)
        {
            Console.Clear();
            switch (SelectionPresent.Show(["reserve", "view reservations", "logout"], "USER MENU\n\n").text)
            {
                case "reserve":
                    MakingReservations.CalendarNavigation(acc);
                    break;

                case "view reservations":
                    MakingReservations.UserOverViewReservation(acc);
                    break;

                case "logout":
                    return;

                default:
                    break;
            }
        }
    }
}
