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
            switch (SelectionPresent.Show(["Login", "Register\n", "Exit program"], "MAIN MENU\n\n").text)
            {
                case "Login":

                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "Register\n":
                    
                    RegisterUser.CreateAccount();
                    continue;

                case "Exit program":
                    return;

                default:
                    continue;
            }
            break; // Valid input provided, break the loop
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
                    MakingReservations.MakingReservation(acc);
                    break;

                case "View reservations":
                    MakingReservations.UserOverViewReservation(acc);
                    break;

                case "Logout":
                    return;

                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

}
