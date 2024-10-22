static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to enter the reservation menu"); 
        AccountModel acc = null;
        string input = Console.ReadLine();
        if (input == "1")
        {
            acc = UserLogin.Start();
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
            Start();
        }
    }
}