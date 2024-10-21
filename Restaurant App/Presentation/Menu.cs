using System;

static class Menu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        // in the future we'll add different numbers to do something else, hence 1 to login

        string input = Console.ReadLine();

        if (input == "1")
        {
            AccountModel loggedInAccount = UserLogin.Start();
            if (loggedInAccount != null)
            {
                if (loggedInAccount.IsAdmin == 1)
                {
                    AdminMenu.AdminStart();  // directs to Admin menu if the account is an admin
                }
                else
                {
                    ShowUserMenu();  // directs to User menu if the account is a regular user
                }
            }
        }
        else if (input == "2")
        {
            Console.WriteLine("This feature is not yet implemented.");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();  // restart the menu if input is invalid
        }
    }

    private static void ShowUserMenu()
    {
        Console.WriteLine("Welcome to the User menu.");
    }
}
