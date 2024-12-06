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
                    RegisterUser.CreateAccount();
                    continue;

                case "exit":
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
            var options = new List<string> { "reserve", "view reservations", "logout" };
            var selection = SelectionPresent.Show(options, "USER MENU\n\n").text;

            switch (selection)
            {
                case "reserve":
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

                case "view reservations":
                    // MakingReservations.UserOverViewReservation(acc);
                    FuturePastResrvations.Show(acc, false);
                    break;

                case "logout":
                    return;

                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

}
