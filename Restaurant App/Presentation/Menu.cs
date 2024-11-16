using System;
using Presentation;
using Project.Presentation;

static class Menu
{
    static public void Start()
    {
        AccountModel? acc = null;
        AccountsLogic? accL = null;

        switch (SelectionMenu.Show(["login", "register", "exit"]))
        {
            case "login":

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
                break;

            case "register":

                accL = new AccountsLogic();
                accL.CreateUserAccount();

                break;

            case "exit":
                return;
        }

    }

    private static void ShowUserMenu(AccountModel acc)
    {
        Console.WriteLine("Welcome to the User menu.");
        Console.WriteLine("Enter 1 to enter the reservation menu");
        string input = Console.ReadLine();
        if (input == "1" && acc is not null)
        {
            UserLogin.Start();
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
