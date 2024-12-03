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

    public static void ShowUserMenu(AccountModel acc)
    {
        while (true)
        {
            Console.Clear();
<<<<<<< Updated upstream
            var options = new List<string> { "reserve", "view reservations", "logout" };
            var selection = SelectionPresent.Show(options, "USER MENU\n\n").text;

            switch (selection)
=======
            switch (SelectionMenu.Show([ "reserve", "view reservations", "logout" ], "USER MENU\n\n"))
>>>>>>> Stashed changes
            {
                case "reserve":
                    try
                    {
<<<<<<< Updated upstream
                        // Step 1: Select a date using CalendarPresentation
                        DateTime selectedDate = CalendarPresentation.Show(DateTime.Now);
                        
                        // Step 2: Pass the selected date to MakingReservations
                        MakingReservations.MakingReservation(acc, selectedDate);
=======
                        DateTime selectedDate = CalendarPresentation.Show(DateTime.Now); // Use the modular calendar
                        MakingReservations.MakingReservation(acc, int.Parse(selectedDate.ToString("ddMMyyyy"))); // Pass the selected date
>>>>>>> Stashed changes
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Date selection canceled. Returning to user menu...");
                        Console.ReadKey();
                    }
<<<<<<< Updated upstream
=======
                    break;

                case "view reservations":
                    MakingReservations.UserOverViewReservation(acc);
>>>>>>> Stashed changes
                    break;

                case "logout":
                    return;

                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    Console.ReadKey();
                    break;
            }
                default:
                    break;
            }
        }
    }
}
