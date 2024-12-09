using System;
using Project;
static class UserLogin
{
    private static UserLogic userLogic = new UserLogic(); 

    private static string? request_email()
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Mail: ", Console.ForegroundColor);
        
        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    private static string? request_password(string? email)
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");
        Console.WriteLine($"Mail: {email}");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Pass: ", Console.ForegroundColor);

        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    public static UserModel? Start()
    {

        string? email = request_email();
        string? password = request_password(email);

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
            Console.WriteLine($"Mail: {email}", Console.ForegroundColor);
            Console.WriteLine($"Pass: {password}", Console.ForegroundColor);
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nInvalid credentials, returning...");

            return null;
        }
    }
}
