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
        }
    }
}
