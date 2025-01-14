using Restaurant;
using App.DataAccess.Utils;
using App.Presentation.User;
using App.Presentation.Admin;

namespace App.Presentation.User;

internal class UserRegisterPresent
{
    public static void CreateAccount(bool admin = false)
    {
        Console.WriteLine("Please enter the following information:\n");

        bool isAdminCreated = false;

        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Escape", "<escape>");
        ControlHelpPresent.ShowHelp();

        string banner = admin ? "REGISTER : ADMIN\n\n" : "REGISTER\n\n";

        string firstName = GetValidatedInput(
            "First name: ",
            input => !string.IsNullOrWhiteSpace(input) ? (true, null) : (false, "First name cannot be empty."),
            menuTitle: banner
        );

        string lastName = GetValidatedInput(
            "Last name: ",
            input => !string.IsNullOrWhiteSpace(input) ? (true, null) : (false, "Last name cannot be empty."),
            menuTitle: banner
        );

        string email = GetValidatedInput(
            "Email (e.g., example@domain.com): ",
            input =>
            {
                var (isValid, message) = LoginLogic.IsEmailValid(input);
                return isValid ? (true, null) : (false, message);
            },
            menuTitle: banner
        );

        string password = GetValidatedInput(
            "Password (8-16 characters, must include letters and numbers): ",
            input =>
            {
                var (isValid, message) = LoginLogic.IsPasswordValid(input);
                return isValid ? (true, null) : (false, message);
            },
            menuTitle: banner
        );

        string phoneNumber = GetValidatedInput(
            "Phone number (10 digits): ",
            input =>
            {
                var (isValid, error) = LoginLogic.IsPhoneNumberValid(input);
                return isValid ? (true, null) : (false, error);
            },
            menuTitle: banner
        );

        isAdminCreated = ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);

        ControlHelpPresent.ResetToDefault();
    }

    private static bool ConfirmAndSaveAccount(string firstName, string lastName, string email, string password, string phoneNumber, bool admin)
    {
        while (true)
        {
            var details = new List<string>
            {
                $"First name   : {firstName}",
                $"Last name    : {lastName}",
                $"Email        : {email}",
                $"Password     : {password}",
                $"Phone number : {phoneNumber}\n",
                "Save and return",
                "Cancel"
            };

            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Exit", "<escape>");
            ControlHelpPresent.ShowHelp();

            dynamic selection = SelectionPresent.Show(details, banner:"Review and select your account details you want to modify:").ElementAt(0).text!;

            if (selection == null || selection == "Cancel")
            {
                ControlHelpPresent.ResetToDefault();
                return false;
            }

            if (selection == "Save and return")
            {
                SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                return admin;
            }

            switch (selection)
            {
                case var s when s?.StartsWith("First name"):
                    firstName = GetValidatedInput(
                        "First name: ",
                        input => !string.IsNullOrWhiteSpace(input) ? (true, null) : (false, "First name cannot be empty."),
                        menuTitle: "EDIT FIRSTNAME"
                    );
                    break;

                case var s when s?.StartsWith("Last name"):
                    lastName = GetValidatedInput(
                        "Last name: ",
                        input => !string.IsNullOrWhiteSpace(input) ? (true, null) : (false, "Last name cannot be empty."),
                        menuTitle: "EDIT LASTNAME"
                    );
                    break;

                case var s when s?.StartsWith("Email"):
                    email = GetValidatedInput(
                        "Email (e.g., example@domain.com): ",
                        input =>
                        {
                            var (isValid, message) = LoginLogic.IsEmailValid(input);
                            return isValid ? (true, null) : (false, message);
                        },
                        menuTitle: "EDIT EMAIL"
                    );
                    break;

                case var s when s?.StartsWith("Password"):
                    password = GetValidatedInput(
                        "Password (8-16 characters, must include letters and numbers): ",
                        input =>
                        {
                            var (isValid, message) = LoginLogic.IsPasswordValid(input);
                            return isValid ? (true, null) : (false, message);
                        },
                        menuTitle: "EDIT PASSWORD"
                    );
                    break;

                case var s when s?.StartsWith("Phone number"):
                    phoneNumber = GetValidatedInput(
                        "Phone number (10 digits): ",
                        input =>
                        {
                            var (isValid, error) = LoginLogic.IsPhoneNumberValid(input);
                            return isValid ? (true, null) : (false, error);
                        },
                        menuTitle: "EDIT PHONE NUMBER"
                    );
                    break;

                default:
                    break;
            }
        }
    }

    private static void SaveAccount(string firstName, string lastName, string email, string password, string phoneNumber, bool admin)
    {
        var account = new UserModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Phone = phoneNumber,
            Admin = Convert.ToInt16(admin)
        };

        Access.Users.Write(account);
    }

    private static string GetValidatedInput(string prompt, Func<string, (bool, string?)> validate, string menuTitle)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(menuTitle);
            Console.WriteLine(prompt);

            string? input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }

            var (isValid, errorMessage) = validate(input);
            if (isValid)
            {
                return input;
            }

            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}
