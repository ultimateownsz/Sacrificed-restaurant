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

            ControlHelpPresent.Clear();
            ControlHelpPresent.ResetToDefault();
            ControlHelpPresent.ShowHelp();

            // Display menu and get selection
            dynamic selection = SelectionPresent.Show([
                "View reservations (date)",
                "Create (admin account)",
                "Delete (accounts)",
                "Update (themes)",
                "(De)activate tables\n",
                "Logout",
            ], "ADMIN MENU\n\n");

            // Check if Escape was pressed
            if (selection.text == null)
            {
                ControlHelpPresent.DisplayFeedback("Logging out...", "bottom", "success");
                // Console.WriteLine(" Exiting Admin Menu...");  // optional message to show the user what is happening
                // Thread.Sleep(1500);  // use this to wait 1,5 seconds before returning to menu
                return;
            }

            // Process the selected option
            switch (selection.text)
            {
                case "View reservations (date)":
                    FuturePastReservations.Show(acc, true);
                    break;
                case "Create (admin account)":
                    RegisterUser.CreateAccount(true);
                    break;
                case "Delete (accounts)":
                    DeleteAccount.ShowDeleteAccountMenu(acc);
                    break;
                case "Update (themes)":
                    ThemeView.ThemedEditing();
                    break;
                case "(De)activate tables":
                    AdminTableControlPresent.Show();
                    break;
                case "Logout":
                    ControlHelpPresent.DisplayFeedback("Logging out...", "bottom", "success");
                    return;
            }
        } while (true);
    }
}

