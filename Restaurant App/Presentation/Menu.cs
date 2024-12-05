using System.Runtime.CompilerServices;
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
            var userChoiceResult = SelectionPresent.Show(
                    new List<string> { "login", "register", "exit" },
                    "MAIN MENU\n\n"
            );

            if (userChoiceResult == null)
            {
                Console.WriteLine("Returning to the previous menu...");
                return;
            }

            var userChoice = userChoiceResult.text;

            switch (userChoice)
            {
                case "login":
                    SelectionPresent.HandleEscape(() => MenuLogic.Login());
                    // MenuLogic.Login();
                    break;

                case "register":
                    SelectionPresent.HandleEscape(() => 
                    {
                        RegisterUser.CreateAccount();
                        return null;
                    });
                    // RegisterUser.CreateAccount();
                    break;

                case "exit":
                    return;

                default:
                    break;
            }
        }
    }

    public static void ShowUserMenu(UserModel acc)
    {
        while (true)
        {
            Console.Clear();
            var userChoice = SelectionPresent.HandleEscape(() => SelectionPresent.Show(
                new List<string> { "reserve", "view reservations", "logout" },
                "USER MENU\n\n"
                )
            );

            if (userChoice == null)
            {
                Console.WriteLine("Returning to the previous menu...");
                return;
            }

            switch (userChoice.text)
            {
                case "reserve":
                   
                    MakingReservations.CalendarNavigation(acc);
                    break;

                case "view reservations":
                    
                    MakingReservations.UserOverViewReservation(acc);
                    break;

                case "logout":
                    Console.WriteLine("Logging out...");
                    return;

                default:
                    break;
            }
        }
    }
}
