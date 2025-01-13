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

        var selection = SelectionPresent.Show(adminOptions, banner:"ACCOUNT REGISTRATION").ElementAt(0).text;

        switch (selection)
        {
            case "Create admin account":
                UserRegisterPresent.CreateAccount(true);
                ControlHelpPresent.ResetToDefault();
                break;
            
            case "Make existing user admin\n":
                PromoteUserToAdmin();
                ControlHelpPresent.ResetToDefault();
                break; 

            case null:
                return;
        }
    }

    private static void PromoteUserToAdmin()
    {
        // Display help options and initial prompt
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        string? firstName = null;
        string? lastName = null;
        TryCatchHelper.EscapeKeyException(() =>
        {
            
        Console.WriteLine("Enter the first and last name of the user you want to promote to admin.\n");

        firstName = InputHelper.GetValidatedInput<string>(
            "First name: ",
            input => InputHelper.InputNotNull(input.ToLower(), "First name"),
            menuTitle: "PROMOTE USER TO ADMIN",
            showHelpAction: () => ControlHelpPresent.ShowHelp());

        lastName = InputHelper.GetValidatedInput<string>(
            "Last Name: ",
            input => InputHelper.InputNotNull(input.ToLower(), "Last name"),
            menuTitle: "PROMOTE USER TO ADMIN",
            showHelpAction: () => ControlHelpPresent.ShowHelp());
        });

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            // ControlHelpPresent.DisplayFeedback("Both first and last names are required..", "bottom", "error");
            return;
        }

        // Access the user database
        var userAccess = new UserAccess();

        // Fetch user based on input
        var user = userAccess.GetAllBy("FirstName", firstName)
            .FirstOrDefault(u =>
                string.Equals(u!.FirstName, firstName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(u!.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
                u.Admin == 0);

        // Confirmation for promotion
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        // Console.WriteLine("Are you sure you want to promote this user to admin?");
        
        if (user == null)
        {
            ControlHelpPresent.DisplayFeedback("User not found. Promotion canceled.", "bottom", "error");
            return;
        }

        // Confirmation for promotion
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        dynamic? confirmation = SelectionPresent.Show(["Yes", "No"],
            banner: "PROMOTE USER TO ADMIN").ElementAt(0).text;

        ControlHelpPresent.ShowHelp();

        if (confirmation == "Yes")
        {
            user.Admin = 1;
            if (userAccess.Update(user))
            {
                ControlHelpPresent.DisplayFeedback($"{user.FirstName} {user.LastName} successfully promoted to admin.", "bottom", "success");
            }
            else
            {
                ControlHelpPresent.DisplayFeedback("Failed to promote the user. Try again.", "bottom", "error");
            }
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("Action canceled. User was not promoted.");
        }
    }
}
