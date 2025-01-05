using Project;

namespace Presentation
{
    public static class DeleteAccountAsUser
    {
        public static void DeleteAccount(UserModel user)
        {
            Console.Clear();
            Console.WriteLine("WARNING: Deleting your account will:");
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
        }
    }
}
