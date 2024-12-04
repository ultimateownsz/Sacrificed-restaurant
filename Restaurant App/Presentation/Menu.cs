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
            switch (SelectionPresent.Show(["Login", "Register\n", "Exit"], "MAIN MENU\n\n").text)
            {
                case "Login":

                    if (MenuLogic.Login() == "continue")
                        continue;

                    break;

                case "Register\n":
                    
                    RegisterUser.CreateAccount();
                    continue;

                case "Exit":
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
            switch (SelectionPresent.Show(["Make a reservation", "View reservations", "Logout"], "USER MENU\n\n").text)
            {
                case "Make a reservation":
                    MakingReservations.CalendarNavigation(acc);
                    break;

                case "View reservations":
                    MakingReservations.UserOverViewReservation(acc);
                    break;

                case "Logout":
                    return;

                default:
                    break;
            }
        }
    }
}
