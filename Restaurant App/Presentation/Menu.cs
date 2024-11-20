using Presentation;
using Project.Presentation;

static class Menu
{
    static public void Start()
    {
        
        // initialization
        AccountModel? acc = null;
        AccountsLogic? accL = null;

        while (true)
        {
            
            // put here for consistent terminal cleaning
            string? input = SelectionMenu.Show(["login", "register", "exit"]);
            Console.Clear();
            
            switch (input)
            {
                case "login":
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
                    else
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    break;

                case "register":
                    accL = new AccountsLogic();
                    accL.CreateUserAccount();
                    break;

                case "exit":
                    return;

                default:
                    continue;
            }

            // valid input has been provided at this point
            break;
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
