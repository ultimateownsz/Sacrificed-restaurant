using System;

static class UserLogin
{
    private static AccountsLogic accountsLogic = new AccountsLogic();
    public static AccountModel Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine(" ");
        while (true)
        {
            Console.Write("Please enter your email address: ");
            string email = Console.ReadLine();
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine();

            AccountModel acc = accountsLogic.CheckLogin(email, password);
            if (acc != null)
            {
                Console.WriteLine("Welcome back " + acc.FirstName + " " + acc.LastName);
                Console.WriteLine("Your email is: " + acc.EmailAddress);
                return acc;
            }
            Console.WriteLine("No account found with that email and password, please try again.");
            Console.WriteLine(" ");
        }
    }
}
