using System;
using Presentation;

static class Menu
{
    //static private T2 ReverseLookup<T, T2>(Dictionary<T, T2> dict, T2 target)
    //{
    //    if ta
    //    foreach ((T key, T2 value) in dict)
    //    {
    //        if (value is target)
    //            return value;
    //    }
    //}

    static public void Start()
    {
        AccountModel? acc = null;
        AccountsLogic? accL = null;

        Dictionary<string, bool> selection = new()
        {
            {"Login", true},
            {"Register", false},
            {"Exit", false},
        };

        while (true)
        {
            foreach ((string text, bool selected) in selection)
            {
                Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.WriteLine(text, Console.ForegroundColor);
            }

            switch (Console.ReadKey().Key)
            {

                case ConsoleKey.DownArrow:
                    ; ;
                    break;
            }
        }


        //while (true)
        //{
        //    // register input without having to press enter
        //    switch (Console.ReadKey().ToString())
        //    {
        //        case "l":

        //            AccountModel loggedInAccount = acc = UserLogin.Start();
        //            if (loggedInAccount != null)
        //            {
        //                if (loggedInAccount.IsAdmin == 1)
        //                {
        //                    AdminMenu.AdminStart();  // directs to Admin menu if the account is an admin
        //                }
        //                else
        //                {
        //                    ShowUserMenu(acc);  // directs to User menu if the account is a regular user
        //                }
        //            }
        //            break;

        //        case "r":

        //            accL = new AccountsLogic();
        //            accL.CreateUserAccount();

        //            break;

        //        case "e":
        //            return;

        //        default:
        //            continue;
        //    }
        //}
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
