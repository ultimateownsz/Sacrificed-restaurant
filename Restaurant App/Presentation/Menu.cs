using System;

static class Menu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login:");
        Console.WriteLine("Enter 2 to register a new account:");
        AccountModel acc = null;
        AccountsLogic accL = null;
        string input = Console.ReadLine();

        if (input == "1")
        {
            AccountModel loggedInAccount = acc = UserLogin.Start();
            if (loggedInAccount != null)
            {
                if (loggedInAccount.IsAdmin == 1)
                {
                    AdminMenu.AdminStart();  // directs to Admin menu if the account is an admin
                }
                else
                {
                    ShowUserMenu(acc);  // directs to User menu if the account is a regular user
                }
            }
        }
        else if (input == "2")
        {
            accL = new AccountsLogic();
            accL.CreateAccount();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();  // restart the menu if input is invalid
        }
    }

    private static void ShowUserMenu(AccountModel acc)
    {
        Console.WriteLine("Welcome to the User menu.");
        Console.WriteLine("Enter 1 to enter the reservation menu");
        string input = Console.ReadLine();
        if (input == "1" && acc is not null)
        {
            MakingReservations.Start(acc);
        }
        else if (input == "1" && acc is null)
        {
            Console.WriteLine("Please log in first");
        }
        else if (input == "q")
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid input");
            ShowUserMenu(acc);  // restart the menu if input is invalid
        }
    }
}
