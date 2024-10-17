using SQLitePCL;

static class AdminMenu
{
    // static private AccountsLogic accountsLogic = new AccountsLogic();

    static public void AdminStart()
    {
        System.Console.WriteLine("Welcome to the Admin page!");
        System.Console.WriteLine("This page is for now empty.");
        System.Console.WriteLine("To go back to the menu press Q:");
        string choice = Console.ReadLine().ToLower();
        switch (choice)
        {
            case "q":
                Menu.Start();
                break;
        }
    }


}