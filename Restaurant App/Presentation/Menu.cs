using System;
using Presentation;

static class Menu
{
    static public void Start()
    {
        Console.WriteLine("Enter [1] to login: ");
        Console.WriteLine("Enter [2] to register a new account: ");
        Console.WriteLine("Enter [3] to exit the program: ");
        
        AccountModel acc = null;
        AccountsLogic accL = null;
        string input = Console.ReadLine();

        if (input == "1")
        {
            acc = UserLogin.Start();
            if (acc != null)
            {
                if (acc.IsAdmin == 1)
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
            accL.CreateUserAccount();
        }
        else if (input == "3")
        {
            Console.WriteLine("Exiting the program...");
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
        Console.WriteLine("Enter 'q' to quit");

        string input = Console.ReadLine();
        if (input == "1" && acc is not null)
        {
            MakingReservations.CalendarNavigation();  // Start calendar navigation to select a date
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
