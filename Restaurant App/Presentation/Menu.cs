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
            Console.Clear();
            switch (SelectionMenu.Show([ "reserve", "view reservations", "logout" ], "USER MENU\n\n"))
            {
                case "reserve":
                    try
                    {
                        DateTime selectedDate = CalendarPresentation.Show(DateTime.Now); // Use the modular calendar
                        MakingReservations.MakingReservation(acc, int.Parse(selectedDate.ToString("ddMMyyyy"))); // Pass the selected date
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Date selection canceled. Returning to user menu...");
                        Console.ReadKey();
                    }
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

    public static void End()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using the Reservation System. Goodbye!");
    }
}
