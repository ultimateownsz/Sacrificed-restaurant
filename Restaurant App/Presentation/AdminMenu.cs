static class AdminMenu
{
    public static void AdminStart()
    {
        // Accounts logic instance
        AccountsLogic accL = null;
        accL = new AccountsLogic();
        bool choosing = true;
        List<string> options = new List<string> { "View all reservations", "Filter reservations", "Update a reservation", "Delete a reservation", "Create a new admin account", "Update themes by month and year", "Go back to the main menu"};

        while (choosing)
        {
            bool choosingOption = true;
            int optionIndex = 0;

            while (choosingOption)
            {
                Console.Clear();
                System.Console.WriteLine(" ");
                System.Console.WriteLine("Welcome to the Admin page!");
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
                        choosing = false;
                        choosingOption = false;
                        if (options[optionIndex] == "View all reservations")
                        {
                            ShowAllReservations.Show();
                        }
                        else if (options[optionIndex] == "Filter reservations")
                        {
                            FilterReservations.Show();
                        }
                        else if (options[optionIndex] == "Update a reservation")
                        {
                            UpdateReservation.Show();
                        }
                        else if (options[optionIndex] == "Delete a reservation")
                        {
                            DeleteReservation.Show();
                        }
                        else if (options[optionIndex] == "Create a new admin account")
                        {
                            RegisterAdminAccount.CreateAdminAccount();
                        }
                        else if (options[optionIndex] == "Update themes by month and year")
                        {
                            ThemeView.UpdateTheme();
                        }
                        else if (options[optionIndex] == "Go back to the main menu")
                        {
                            Menu.Start();
                        }
                        break;
                }
            }
        }

        // System.Console.WriteLine(" ");
        // System.Console.WriteLine("Welcome to the Admin page!");
        // System.Console.WriteLine("1. View all reservations");
        // System.Console.WriteLine("2. Filter reservations");
        // System.Console.WriteLine("3. Update a reservation");
        // System.Console.WriteLine("4. Delete a reservation");
        // // NEW (admin is able to create another admin account)
        // System.Console.WriteLine("5. Create a new admin account");
        // System.Console.WriteLine("6. Update themes by month and year");
        // // System.Console.WriteLine("7. Add new themes");
        // System.Console.WriteLine("Q. Go back to the main menu");
        // System.Console.WriteLine(" ");

        // string choice = Console.ReadLine().ToLower();
        // switch (choice)
        // {
        //     case "1":
        //         ShowAllReservations.Show();
        //         break;
        //     case "2":
        //         FilterReservations.Show();
        //         break;
        //     case "3":
        //         UpdateReservation.Show();
        //         break;
        //     case "4":
        //         DeleteReservation.Show();
        //         break;
        //     case "5":
        //         accL.CreateAdminAccount();
        //         break;
        //     case "6":
        //         ThemeView.UpdateTheme();  // dit werkt ook
        //         break;
        //     // er zit nu een kleine bug in het opslaan van thema's naar de database, dit heeft met een aantal dingen te maken, maar handmatig thema's aanmaken kan wel
        //     // case "7":
        //     //     ThemeView.AddTheme();
        //     //     break;
        //     case "q":
        //         Menu.Start();
        //         break;
        //     default:
        //         Console.WriteLine("Invalid input. Try again.");
        //         AdminStart();
        //         break;
        // }
    }
}