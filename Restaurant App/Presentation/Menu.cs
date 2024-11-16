using System;
using Presentation;

static class Menu
{
    static private Tuple<T?, int> ReverseLookup<T, T2>(Dictionary<T, T2> dict, T2 target)
    {
        int i = 0;
        foreach ((T key, T2 value) in dict)
        {
            if (value.Equals(target))
                return new(key, i);
            
            i++;
        }
        
        // may cause errors
        return new(default(T), 0);
    }

    static private Index Next(int size, int current, bool reverse = false)
    {
        current += (reverse) ? -1 : 1;
        
        if (current >= size) current = 0;
        if (current < 0) current = size - 1;

        return current;
    }

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
            Console.Clear();
            foreach ((string text, bool selected) in selection)
            {
                Console.ForegroundColor = (selected) ? ConsoleColor.Yellow : ConsoleColor.White;
                Console.WriteLine(text, Console.ForegroundColor);
            }

            var current = ReverseLookup<string, bool>(selection, true);
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.DownArrow:   
                    
                    selection[current.Item1] = false;
                    selection[selection.ElementAt(Next(selection.Count, current.Item2)).Key] = true;
                    break;

                case ConsoleKey.UpArrow:

                    selection[current.Item1] = false;
                    selection[selection.ElementAt(Next(selection.Count, current.Item2, true)).Key] = true;
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
