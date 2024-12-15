using Project;
using Project.Presentation;
using Presentation;

static class AdminMenu
{
    public static void AdminStart(UserModel acc)
    {
        do
        {
            Console.Clear();

            // Display menu and get selection
            dynamic selection = SelectionPresent.Show([
                "View reservations (date)",
                "Create (admin account)",
                "Delete (accounts)",
                "Update (themes)",
                "(De)activate tables\n",
                "Back"
            ], "ADMIN MENU\n\n");

            // Check if Escape was pressed
            if (selection.text == null)
            {
                Console.WriteLine(" Exiting Admin Menu...");  // optional message to show the user what is happening
                Thread.Sleep(1500);  // use this to wait 1,5 seconds before returning to menu
                return;
            }

            // Process the selected option
            switch (selection.text)
            {
                case "View reservations (date)":
                    ShowReservations.Show(acc);
                    break;
                case "Create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "Delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu(acc);
                    break;
                case "Update (themes)\n":
                    ThemeView.ThemedEditing();
                    break;
                case "(De)activate tables\n":
                    AdminTableControlPresent.Show();
                    break;
                case "Back":
                    return;
            }
        } while (true);
    }
}

