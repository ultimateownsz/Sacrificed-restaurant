using Presentation;
using Project.Logic;
using Project.Presentation;

static class Menu
{
    static public void Start()
    {

        while (true)
        {
            Console.Clear();
            switch (SelectionMenu.Show(["login", "register\n", "exit"], "MAIN MENU\n\n"))
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

    public static void ShowUserMenu(AccountModel acc)
    {
        while (true)
        {
            switch (SelectionMenu.Show(["reserve", "logout"], "USER MENU\n\n"))
            {
                case "reserve":
                    MakingReservations.CalendarNavigation();
                    break;

                case "logout":
                    return;

                default:
                    break;
            }
        }
    }

    public static void End()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using the Reservation System. Goodbye!");
    }
}
