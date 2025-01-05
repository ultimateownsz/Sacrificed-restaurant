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

        }
    }
}
