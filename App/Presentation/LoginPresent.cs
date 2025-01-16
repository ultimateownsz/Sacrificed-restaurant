namespace Restaurant;
public static class LoginPresent
{
    private static SelectionPresent.Palette palette = new();

    private static string? _request_email(string? email)
    {
        Console.Clear();
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        string prefix = "LOGIN MENU\n\n";
        prefix += $"E-mail: ";
        
        Console.ForegroundColor = palette.Base;
        return TerminableUtilsPresent.ReadLine(prefix, email ?? "", colour: Console.ForegroundColor);
        
    }

    private static string? _request_password(string? email)
    {
        Console.Clear();
        string prefix = "LOGIN MENU\n\n";
        prefix += $"E-mail: {email}\nPassword: ";
        
        Console.ForegroundColor = palette.Base;
        return TerminableUtilsPresent.ReadLine(prefix, colour: Console.ForegroundColor);
    }

    public static UserModel? Show()
    {

        string? email = null;
        string? password = null;
        
        while (true)
        {
            // request email
            email = _request_email(email);
            if (email == null)
            {
                ControlHelpPresent.ResetToDefault();
                return null;
            }

            // request password
            password = _request_password(email);
            if (password == null) continue;

            // user didn't try to terminate
            break;
        }

        var loginResult = LoginLogic.CheckLogin(email, password);
        ControlHelpPresent.ResetToDefault();

        if (loginResult.user != null)
        {
            return loginResult.user;
        }    
        else
        {
            Console.Clear();
            ControlHelpPresent.ResetToDefault();
            Console.WriteLine("LOGIN MENU\n");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"E-mail : {email}", Console.ForegroundColor);
            Console.WriteLine($"Password : {password}", Console.ForegroundColor);
            
            Console.ForegroundColor = palette.Base;
            Console.WriteLine("\nInvalid credentials, returning...");

            Thread.Sleep(1000);
            return null;
        }
    }

}
