using System.Reflection.Metadata;

namespace Project.Presentation
{
    public static class CreateAdminOptions
    {
        public static void ManageAdmins(UserModel acc)
        {
            List<string> adminOptions = new()
            {
                "Create a new admin account",
                "Make an existing user an admin",
                "Back"
            };

            string choice = SelectionPresent.Show(adminOptions, "Create (admin account)\n\n").text;

            switch (choice)
            {
                case "Create a new admin account":
                    RegisterUser.CreateAccount(true);  // Call the existing CreateAccount method with admin=true
                    break;

                case "Make an existing user an admin":
                    PromoteUserToAdmin();
                    break;

                case "Back":
                    return;
            }
        }

        private static void PromoteUserToAdmin()
        {
            Console.Clear();
            Console.WriteLine("Enter the first and last name of the user you want to promote to admin.");
            Console.WriteLine("");

            Console.Write("First Name: ");
            string firstName = Console.ReadLine()?.Trim();
            Console.Write("Last Name: ");
            string lastName = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("Both first and last names are required. Promoting user canceled.");
                Console.WriteLine("Press any key to return to the menu...");
                Console.ReadKey();
                return;
            }
        }
    }
}
