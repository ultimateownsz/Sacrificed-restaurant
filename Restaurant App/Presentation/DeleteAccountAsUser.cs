using Project;
using Project.Logic;

namespace Presentation
{
    public static class DeleteAccountAsUser
    {
        public static void DeleteAccount(UserModel user)
        {
            Console.Clear();
            Console.WriteLine("WARNING: Deleting your account will:");
            Console.WriteLine("");
            Console.WriteLine("- Delete all future reservations.");
            Console.WriteLine("- Make your past reservations anonymous.");
            Console.WriteLine("- Remove all your personal information.\n");

            // Password confirmation
            Console.Write("Please confirm your password to proceed: ");
            string enteredPassword = Console.ReadLine()?.Trim();

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
                // Delete future reservations after the account is marked as inactive
                DeleteAccountLogic.DeleteFutureReservations(user.ID);

                // Provide feedback to the user
                Console.WriteLine("Your account has been successfully deleted and anonymized.");
                Console.WriteLine("Press any key to exit the program...");
                Console.ReadKey();

                // Exit the program after account deletion
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Error occurred while deleting your account.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }
}
