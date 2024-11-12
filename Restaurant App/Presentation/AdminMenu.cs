static class AdminMenu
{
    public static void AdminStart()
    {
        // Accounts logic instance
        AccountsLogic accL = null;
        accL = new AccountsLogic();

        System.Console.WriteLine(" ");
        System.Console.WriteLine("Welcome to the Admin page!");
        System.Console.WriteLine("1. View all reservations");
        System.Console.WriteLine("2. Filter reservations");
        System.Console.WriteLine("3. Update a reservation");
        System.Console.WriteLine("4. Delete a reservation");
        // NEW (admin is able to create another admin account)
        System.Console.WriteLine("5. Create a new admin account");
        System.Console.WriteLine("Q. Go back to the main menu");
        System.Console.WriteLine(" ");

        string choice = Console.ReadLine().ToLower();
        switch (choice)
        {
            case "1":
                ShowAllReservations.Show();
                break;
            case "2":
                FilterReservations.Show();
                break;
            case "3":
                UpdateReservation.Show();
                break;
            case "4":
                DeleteReservation.Show();
                break;
            case "5":
                accL.CreateAdminAccount();
                break;
            case "q":
                Menu.Start();
                break;
            default:
                Console.WriteLine("Invalid input. Try again.");
                AdminStart();
                break;
        }
    }
}