using System;

class Program
{
    static void Main(string[] args)
    {
        InitializeDatabase();
        RunApplication();
    }

    private static void InitializeDatabase()
    {

        // Create an Admin account
        AccountModel admin = new AccountModel(3, "Admin", "User", "admin@example.com", "admin123", 123456789, 1);
        // AccountsAccess.Write(admin);
        // AccountsAccess.Delete(1);
        // AccountsAccess.Delete(2);

        // Create a User account
        AccountModel user = new AccountModel(4, "User", "userName", "user@example.com", "user123", 987654321, 0);
        // AccountsAccess.Write(user);
        // AccountsAccess.Delete(1);
        // AccountsAccess.Delete(2);


    }

    private static void RunApplication()
    {
        Console.WriteLine("Log in");
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Wachtwoord: ");
        string password = Console.ReadLine();

        var account = new AccountsLogic().CheckLogin(email, password);

        if (account != null)
        {
            if (account.IsAdmin == 1)
            {
                ShowAdminMenu();
            }
            else
            {
                ShowUserMenu();
            }
        }
        else
        {
            Console.WriteLine("Ongeldige inloggegevens.");
        }
    }

    private static void ShowAdminMenu()
    {
        Console.WriteLine("Welkom bij het Admin menu.");
        AdminMenu.AdminStart();
    }

    private static void ShowUserMenu()
    {
        Console.WriteLine("Welkom bij het Gebruikers menu.");
    }
}
