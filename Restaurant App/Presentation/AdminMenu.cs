static class AdminMenu
{
    public static void AdminStart()
    {
        Console.WriteLine("");
        Console.WriteLine("Welcome to the Admin reservation page!");
        Console.WriteLine("1. View all reservations");
        Console.WriteLine("2. Filter reservations");
        Console.WriteLine("3. Update a reservation");
        Console.WriteLine("4. Delete a reservation");
        Console.WriteLine("Q. Go back to the main menu");
        Console.WriteLine("");

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