using Project;
public static class LoginPresent
{
    private static string? _request_email()
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("mail: ", Console.ForegroundColor);
        
        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    private static string? _request_password(string? email)
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");
        Console.WriteLine($"mail: {email}");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"pass: ", Console.ForegroundColor);

        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    public static UserModel? Show()
    {

        string? email = _request_email();
        string? password = _request_password(email);

        UserModel? acc = UserLogic.CheckLogin(email?.ToLower(), password);
        if (acc != null)
        {
            return acc;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("LOGIN\n");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"mail: {email}", Console.ForegroundColor);
            Console.WriteLine($"pass: {password}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid credentials, returning...");

            return null;
        }
    }
}
