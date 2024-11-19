using System;
using Presentation;

static class Menu
{
    static public void Start()
    {
        AccountModel acc = null;
        AccountsLogic accL = null;
        List<string> options = new List<string> { "Login", "Register", "Exit" };
        bool choosing = true;

        while (choosing)
        {
            int optionIndex = 0;
            bool choosingOption = true;

            while (choosingOption)
            {
                Console.Clear();
                System.Console.WriteLine("Choose what would you like to do: ");
                for (int i = 0; i < options.Count; i++)
                {
                    if (optionIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine($"-> {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        System.Console.WriteLine($" {options[i]}");
                    }
                }

                var keys = Console.ReadKey(intercept: true);
                switch (keys.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (optionIndex > 0) optionIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (optionIndex < options.Count - 1) optionIndex++;
                        break;
                    case ConsoleKey.Enter:
                        if (options[optionIndex] == "Login")
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
                        else if (options[optionIndex] == "Register")
                        {
                            RegisterUserAccount.CreateUserAccount();
                        }
                        else
                        {
                            System.Console.WriteLine("Exiting program...");
                        }
                        choosingOption = false;
                        choosing = false;
                        break;
                }
            }
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
