namespace Restaurant;

using App.Presentation.User;
using System.Globalization;

internal class MenuPresent
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
            switch (SelectionPresent.Show(["Login", "Register\n", "Exit"], banner: "MAIN MENU").ElementAt(0).text)
            {
                case "Login":
                    if (MenuLogic.Login() == "continue")
                        continue;
                    break;

                case "Register\n":
                    UserRegisterPresent.CreateAccount();
                    ControlHelpPresent.ResetToDefault();
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
}
