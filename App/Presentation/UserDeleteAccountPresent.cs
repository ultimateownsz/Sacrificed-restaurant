namespace Restaurant;

public static class UserDeleteAccountPresent
{
    public static void DeleteAccount(UserModel user)
    {
        Console.Clear();
        string prefix = "WARNING: Deleting your account entails the following:\n\n";
        prefix += "- Delete all future reservations.\n";
        prefix += "- Make your past reservations anonymous.\n";
        prefix += "- Remove all your personal information.\n\n";
        prefix += "Please confirm your password to proceed: ";


        // Password confirmation
        string? enteredPassword = TerminableUtilsPresent.ReadLine(prefix);
        
        if (enteredPassword == null)
            return;
        
        // trimming ig..
        enteredPassword = enteredPassword.Trim();


        if (enteredPassword != user.Password)
        {
            Console.WriteLine("\nIncorrect password. Account deletion cancelled.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }

        // ConfirmAndDelete method from DeleteAccountLogic
        bool deletionSuccessful = DeleteAccountLogic.ConfirmAndDelete(user);

        if (deletionSuccessful)
        {
            // Provide feedback to the user
            Console.WriteLine("Your account has been successfully deleted.");
            Console.WriteLine("Press any key to return to the login page...");
            Console.ReadKey();

            // Go back to the login page after deletion
            UserMenuPresent.Start();
        }
        else
        {
            Console.WriteLine("Error occurred while deleting your account.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }
    }
}
