using App.DataAccess.Utils;
using App.Presentation.User;

namespace Restaurant;

public static class AdminCreateOptionsPresent
{
    public static void Options(UserModel acc)
    {
        List<string> adminOptions = new()
        {
            "Create admin account",
            "Make existing user admin\n",
            "Back"
        };

        string choice = SelectionPresent.Show(
            adminOptions, banner: "Create (admin account)").ElementAt(0).text;

        switch (choice)
        {
            case "Create admin account":
                UserRegisterPresent.CreateAccount(true);
                break;

            case "Make existing user admin\n":
                PromoteUserToAdmin();
                break;

            case "":
                return;

        }
    }

    private static void PromoteUserToAdmin()
    {
        string prefix = "Please enter the following information of the user:\n\n";

        string firstName = TerminableUtilsPresent.ReadLine(prefix + "First Name: ");
        if (firstName == null) return;

        string lastName = TerminableUtilsPresent.ReadLine(prefix + "Last Name: ");
        if (lastName == null) return;

        // Validate inputs
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            Console.WriteLine("\nBoth first and last names are required. Promoting user canceled.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }

        // Create an instance fpr UserModel
        var userAccess = Access.Users;

        // Fetch user bases on the input
        var user = userAccess.GetAllBy("FirstName", firstName)
            .FirstOrDefault(u =>
                string.Equals(u.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                u.Admin == 0); // Only fetch users

        if (user == null)
        {
            Console.WriteLine("\nUser not found.");
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
            return;
        }

        // Confirmation for promoting
        Console.Clear();
        var confirmationOptions = new List<string> { "Yes", "No" };
        string confirmation = SelectionPresent.Show(confirmationOptions, banner: "Are you sure?").ElementAt(0).text;

        if (confirmation == "Yes")
        {
            // Update the user's Admin status
            user.Admin = 1;
            userAccess.Update(user);
            Console.WriteLine("User privileges elevated to admin");
            Thread.Sleep(1000);
        }
    }
}
