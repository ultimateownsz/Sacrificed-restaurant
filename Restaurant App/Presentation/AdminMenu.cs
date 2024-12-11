using Project;
using Project.Presentation;
using Presentation;

static class AdminMenu
{
    public static void AdminStart(UserModel acc)
    {
        List<string> options = new List<string>
        {
            "View reservations by date",
            "Create (admin account)",
            "Delete (accounts)",
            "Update (themes)",
            "(De)activate tables\n",
            "Back"
        };

        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine("Press Escape to exit the Admin Menu\n");

            // Display menu and get selection
            string selection = SelectionPresent.Show(options, "ADMIN MENU\n\n").text;

            // Process the selected option
            switch (selection)
            {
                case "View reservations by date":
                    ShowReservations.Show();
                    break;
                case "Create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "Delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu();
                    break;
                case "Update (themes)\n":
                    ThemeView.ThemedEditing();
                    break;
                case "(De)activate tables\n":
                    AdminTableControlPresent.Show();
                    break;
                case "back":
                    return;
            }

            Console.WriteLine("\nPress Escape to exit the Admin Menu, or any key to continue...");

            // read the key and store in 'key'
            key = Console.ReadKey(true);

        } while (key.Key != ConsoleKey.Escape); // Loop until Escape is pressed
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Exiting Admin Menu...");
        return;
    }
}
