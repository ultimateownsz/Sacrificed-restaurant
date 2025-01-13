using Restaurant;

namespace App.Presentation.User;

public static class UserDeleteAccountPresent
{
    public static void DeleteAccount(UserModel user)
    {
        // Method to display the warning message in the middle of the terminal
        void DisplayWarning()
        {
            Console.Clear();
            
            // Warning message
            string[] warningLines = new string[]
            {
                "WARNING: Deleting your account entails the following:",
                "",
                "- Delete all future reservations.",
                "- Make your past reservations anonymous.",
                "- Remove all your personal information.",
                ""
            };

            // Calculate vertical start position to center the message vertically
            int consoleHeight = Console.WindowHeight;
            int verticalStart = (consoleHeight / 2) - (warningLines.Length / 2);

            // Set warning color to red
            Console.ForegroundColor = ConsoleColor.Red;

            // Print each line starting from the leftmost position (X = 0)
            foreach (var (line, index) in warningLines.Select((line, index) => (line, index)))
            {
                Console.SetCursorPosition(0, verticalStart + index);
                Console.WriteLine(line);
            }

            // Reset console color to default
            Console.ResetColor();
        }


        // Show initial warning and help
        DisplayWarning();
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        // Attempt to confirm password with escape key handling
        bool passwordConfirmed = false;

        TryCatchHelper.EscapeKeyException(() =>
        {
            string enteredPassword = InputHelper.GetValidatedInput<string>(
                "Please type in your password to proceed: ",
                input => (input, null), // Simple validation that returns the input as is
                menuTitle: "DELETE ACCOUNT",
                showHelpAction: () =>
                {
                    DisplayWarning(); // Redraw warning message
                    ControlHelpPresent.ShowHelp();
                }
            );

            // Check password
            if (enteredPassword == user.Password)
            {
                passwordConfirmed = true; // Mark as confirmed
            }
            else
            {
                ControlHelpPresent.DisplayFeedback("Incorrect password. Account deletion cancelled.");
                ControlHelpPresent.DisplayFeedback("Press any key to return to the menu...", "bottom", "tip");
                Console.ReadKey();
            }
        },
        "Password confirmation cancelled. Returning to menu...");
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        // If the password wasn't confirmed, return to the previous menu
        if (!passwordConfirmed)
        {
            return;
        }

        // Proceed with account deletion
        bool deletionSuccessful = DeleteAccountLogic.ConfirmAndDelete(user);
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        if (deletionSuccessful)
        {
            ControlHelpPresent.DisplayFeedback("Your account has been successfully deleted.");
            ControlHelpPresent.DisplayFeedback("Press any key to return to the login page...", "bottom", "tip");
            Console.ReadKey();

            // Redirect to login menu
            LoginPresent.Show();
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("Error occurred while deleting your account.");
            ControlHelpPresent.DisplayFeedback("Press any key to return to the menu...", "bottom", "tip");
            Console.ReadKey();
        }
    }
}
