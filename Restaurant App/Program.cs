using System;

class Program
{

    // Aub even niet aanzitten voor nu
    static void Main(string[] args)
    {
        InitializeDatabase();
        Menu.Start();
    }

    private static void InitializeDatabase()
    {

        // Create an Admin account
        // AccountModel admin = new AccountModel(1, "Admin", "User", "admin@example.com", "admin123", 123456789, 1);
        // AccountsAccess.Write(admin);

        // Create a User account
        // AccountModel user = new AccountModel(2, "User", "userName", "user@example.com", "user123", 987654321, 0);
        // AccountsAccess.Write(user);
        // AccountsAccess.Delete(1);
        // AccountsAccess.Delete(2);

    }
}