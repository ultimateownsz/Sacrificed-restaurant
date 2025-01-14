namespace Restaurant;
public static class LoginPresent
{
    private static SelectionPresent.Palette palette = new();

    private static string? _request_email(string? email)
    {
        Console.Clear();
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
            if (email == null) return null;

            // request password
            password = _request_password(email);
            if (password == null) continue;

            // user didn't try to terminate
            break;
        }

        UserModel? acc = LoginLogic.CheckLogin(email, password);
        if (acc != null)
        {
            return acc;
        }    
        else
        {
            Console.Clear();
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
