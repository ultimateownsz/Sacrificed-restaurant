using Presentation;
using Project.Logic;
using Project.Presentation;

namespace Project;
static class Menu
{
    static public void Start()
    {        
        do
        {
            Console.Clear();
            dynamic selection = SelectionPresent.Show([
                "Login",
                "Register\n",
                "Exit"
            ], "MAIN MENU\n\n");

            if (selection.text == null)
            {
                Console.Clear();
                Environment.Exit(0);
                return;
            }

            switch(selection.text)
            {
                case "Login":
                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;
                
                case "Register":
                    RegisterUser.CreateAccount();
                    continue;
                
                case "Exit":
                    Console.Clear();
                    Environment.Exit(0);
                    return;
                
                default:
                    continue;
            }
        
        } while (true);
    }

    public static void ShowUserMenu(UserModel acc)
    {
        do
        {
            Console.Clear();
            dynamic selection = SelectionPresent.Show(["Make a reservation", "View reservations", "Logout"], "USER MENU\n\n");
            
            if (selection.text == null)
            {
                Console.WriteLine(" Return to main menu");
                // Thread.Sleep(1500);  // wait 1,5 seconds before you return to main menu
                return;
            }
            
            switch (selection.text)
            {
                case "Make a reservation":
                    // Directly call MakingReservation without calendar in Menu
                    MakingReservations.MakingReservation(acc);
                    break;

                case "View reservations":
                    // MakingReservations.UserOverViewReservation(acc);
                    FuturePastReservations.Show(acc, false); // using the new method - commented the old method just in case
                    break;

                case "Logout":
                    return;
            }
        } while (true);
    }
}
