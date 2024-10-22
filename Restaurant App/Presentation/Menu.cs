using System;

static class Menu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to enter the reservation menu"); 
        AccountModel acc = null;
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
                    ShowUserMenu();  // directs to User menu if the account is a regular user
                }
            }
        }
        else if (input == "2" && acc is not null)
        {
            MakingReservations.Start(acc);
        }
        else if (input == "2" && acc is null)
        {
            Console.WriteLine("Please log in first");
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
