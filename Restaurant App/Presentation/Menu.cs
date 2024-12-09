using Presentation;
using Project.Logic;
using Project.Presentation;

namespace Project;
static class Menu
{
    static public void Start()
    {
        TableSelection.MaximizeConsoleWindow();
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
                    try
                    {
                        // Directly call MakingReservation without calendar in Menu
                        MakingReservations.MakingReservation(acc);
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Reservation process canceled. Returning to user menu...");
                        Console.ReadKey();
                    }
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
