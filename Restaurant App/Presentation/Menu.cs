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
            string? input = SelectionMenu.Show(["login", "register", "exit"], "MAIN MENU\n\n");
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
                            continue;
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
                    continue;

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
        while (true)
        {
            switch (SelectionMenu.Show(["reserve", "logout"], "USER MENU\n\n"))
            {
                case "reserve":
                    MakingReservations.ReservationMenu(acc);
                    break;

                case "logout":
                    return;

                default:
                    break;
            }
        }
    }
}
