using Project;
public static class LoginPresent
{
    private static SelectionPresent.Palette palette = new();

    private static string? _request_email()
    {
        Console.Clear(); 
        Console.WriteLine("LOGIN MENU\n");

        Console.ForegroundColor = palette.Primary;
        Console.Write("E-mail : ", Console.ForegroundColor);
        
        Console.ForegroundColor = palette.Base;
        return Console.ReadLine();
    }

    private static string? _request_password(string? email)
    {
        Console.Clear();
        Console.WriteLine("LOGIN MENU\n");
        Console.WriteLine($"E-mail : {email}");
        
        Console.ForegroundColor = palette.Primary;
        Console.Write($"Password : ", Console.ForegroundColor);

        Console.ForegroundColor = palette.Base;
        return Console.ReadLine();
    }

    public static UserModel? Show()
    {

        string? email = _request_email();
        string? password = _request_password(email);

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
            Console.WriteLine($"Password :     {password}", Console.ForegroundColor);
            
            Console.ForegroundColor = palette.Base;
            Console.WriteLine("\nInvalid credentials, returning...");

            return null;
        }
    }
}
