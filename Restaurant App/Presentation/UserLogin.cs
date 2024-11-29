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
        Console.Write("mail: ", Console.ForegroundColor);
        
        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    private static string? request_password(string email)
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");
        Console.WriteLine($"mail: {email}");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"pass: ", Console.ForegroundColor);

        Console.ForegroundColor = ConsoleColor.White;
        return Console.ReadLine();
    }

    public static UserModel Start()
    {

        string? email = request_email();
        string? password = request_password(email);

        UserModel? acc = UserLogic.CheckLogin(email, password);
        if (acc != null)
        {
            //Console.WriteLine("Welcome back " + acc.FirstName + " " + acc.LastName);
            //Console.WriteLine("Your email is: " + acc.EmailAddress);
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
