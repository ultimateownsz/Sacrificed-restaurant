using Presentation;
using Project.Logic;
using Project.Presentation;
using System.Globalization;

namespace Project;
static class Menu
{

    public static void Init()
    {
        // console initialization

        // support uni-code character such as "$" in strings
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        // format date information in english (US)
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture =
            new System.Globalization.CultureInfo("en-US");

        // globally modify decimal values with doubless
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)
            System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ",";

        // Customize the NumberFormatInfo to always display two decimal places
        customCulture.NumberFormat.NumberDecimalDigits = 2;

        CultureInfo.DefaultThreadCurrentCulture = customCulture;
        CultureInfo.DefaultThreadCurrentUICulture = customCulture;
    }
    static public void Start()
    {
        // TableSelection.MaximizeConsoleWindow();
        while (true)
        {
            Console.Clear();
            switch (SelectionPresent.Show(["Login", "Register", "Controls\n", "Exit"], banner: "MAIN MENU").ElementAt(0).text)
            {
                case "Login":
                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "Register":
                    RegisterUser.CreateAccount();
                    continue;

                case "Controls\n":
                    SelectionPresent._controls();
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

            }
        }
    }

}
