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
            switch (SelectionPresent.Show(["Login", "Register\n", "Exit"], banner: "MAIN MENU").ElementAt(0).text)
            {
                case "Login":
                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "Register\n":
                    RegisterUser.CreateAccount();
                    continue;

                case "Exit":
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
                "Make reservation", "View reservation", "Specify allergies", "Delete account\n", "Logout" };
            var selection = SelectionPresent.Show(options, banner: "USER MENU").ElementAt(0).text;

            if (string.IsNullOrEmpty(selection)) return;

            switch (selection)
            {
                case "Make reservation":
                    // Directly call MakingReservation without calendar in Menu
                    MakingReservations.MakingReservation(acc);
                    break;

                case "View reservation":
                    // MakingReservations.UserOverViewReservation(acc);
                    FuturePastResrvations.Show(acc, false); // using the new method - commented the old method just in case
                    break;
                
                case "Delete account\n":
                    DeleteAccountAsUser.DeleteAccount(acc);
                    break;

                case "Specify allergies":
                    LinkAllergyLogic.Start(LinkAllergyLogic.Type.User, acc.ID);
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
