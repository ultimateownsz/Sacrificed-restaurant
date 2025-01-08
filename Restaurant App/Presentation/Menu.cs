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
            switch (SelectionPresent.Show(["login", "register\n", "exit"], banner: "MAIN MENU").ElementAt(0).text)
            {
                case "login":
                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "register\n":
                    RegisterUser.CreateAccount();
                    continue;

                case "exit":
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
            var options = new List<string> { 
                "reserve", "view reservations", "specify diet/allergies", "delete account\n", "logout" };
            var selection = SelectionPresent.Show(options, banner: "USER MENU").ElementAt(0).text;

            switch (selection)
            {
                case "reserve":
                    // Directly call MakingReservation without calendar in Menu
                    MakingReservations.MakingReservation(acc);
                    break;

                case "view reservations":
                    // MakingReservations.UserOverViewReservation(acc);
                    FuturePastResrvations.Show(acc, false); // using the new method - commented the old method just in case
                    break;

                case "specify diet/allergies":
                    AllergyLogic.Start(AllergyLogic.Type.User, acc.ID);
                    break;
                
                case "delete account\n":
                    DeleteAccountAsUser.DeleteAccount(acc);
                    break;

                case "logout":
                    return;

            }
        }
    }

}
