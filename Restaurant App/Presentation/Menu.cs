static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role

    //Enter 2 will be admin login option. it will send the user (if admin) to the admin Menu
    //Create new file AdminMenu.cs. it will be for now empty but it the admin will be sent to this menu.
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to do something else in the future");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "2")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}