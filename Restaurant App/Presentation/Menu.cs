using Presentation;
using Project.Logic;
using Project.Presentation;

namespace Project;
static class Menu
{
    static public void Start()
    {
        // TableSelection.MaximizeConsoleWindow();
        while (true)
        {
            Console.Clear();
            switch (SelectionPresent.Show(["Login", "Register\n", "Exit application"], "MAIN MENU\n\n").text)
            {
                case "Login":

                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "Register\n":
                    
                    RegisterUser.CreateAccount();
                    continue;

                case "Exit application":
                    Environment.Exit(0);
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
                    // Directly call MakingReservation without calendar in Menu
                    MakingReservations.MakingReservation(acc);
                    break;

                case "View reservations":
                    // MakingReservations.UserOverViewReservation(acc);
                    FuturePastResrvations.Show(acc, false); // using the new method - commented the old method just in case
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
