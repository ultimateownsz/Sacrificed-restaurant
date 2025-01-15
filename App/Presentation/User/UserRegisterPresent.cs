using Restaurant;
using App.DataAccess.Utils;

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

        string? firstName = TerminableUtilsPresent.ReadLine("First name: ", colour: ConsoleColor.Yellow);
        if (firstName == null) return;
        while (string.IsNullOrWhiteSpace(firstName))
        {
            string message = "First name cannot be empty.";
            firstName = TerminableUtilsPresent.ReadLine($"First name: \n\n{message}");
            if (firstName == null) return;
        }

        string? lastName = TerminableUtilsPresent.ReadLine("Last name: ");
        if (lastName == null) return;
        while (string.IsNullOrWhiteSpace(lastName))
        {
            string message = "Last name cannot be empty.";
            lastName = TerminableUtilsPresent.ReadLine($"Last name: \n\n{message}");
            if (lastName == null) return;
        }

        string? email = TerminableUtilsPresent.ReadLine("E-mail (e.g., example@domain.com): ");
        if (email == null) return;
        while (true)
        {
            var (isValid, message) = LoginLogic.IsEmailValid(email);
            if (isValid)
            {
                break;
            }
            else
            {
                email = TerminableUtilsPresent.ReadLine($"E-mail (e.g., example@domain.com): \n\n{message}");
                if (email == null) return;
            }
        }

        string? password = TerminableUtilsPresent.ReadLine("Password (8-16 characters, must include letters and numbers): ");
        if (password == null) return;
        while (true)
        {
            var (isValid, message) = LoginLogic.IsPasswordValid(password);
            if (isValid)
            {
                break;
            }
            else
            {
                password = TerminableUtilsPresent.ReadLine($"Password (8-16 characters, must include letters and numbers): \n\n{message}");
                if (password == null) return;
            }
        }

        string? phoneNumber = TerminableUtilsPresent.ReadLine("Phone number (10 digits, starting with 06): ");
        if (phoneNumber == null) return;
        while (true)
        {
            var (isValid, error) = LoginLogic.IsPhoneNumberValid(phoneNumber);
            if (isValid)
            {
                break;
            }
            else
            {
                phoneNumber = TerminableUtilsPresent.ReadLine($"Phone number (10 digits, starting with 06): \n\n{error}");
                if (phoneNumber == null) return;
            }
        }


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
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("First name: ");
                    Console.ResetColor();
                    firstName = Console.ReadLine() ?? "";
                    while (string.IsNullOrWhiteSpace(firstName))
                    {
                        Console.WriteLine("First name cannot be empty.");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("First name: ");
                        Console.ResetColor();
                        firstName = Console.ReadLine() ?? "";
                    }
                    break;

                case var s when s?.StartsWith("Last name"):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Last name: ");
                    Console.ResetColor();
                    lastName = Console.ReadLine() ?? "";
                    while (string.IsNullOrWhiteSpace(lastName))
                    {
                        Console.WriteLine("Last name cannot be empty.");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Last name: ");
                        Console.ResetColor();
                        lastName = Console.ReadLine() ?? "";
                    }
                    break;

                case var s when s?.StartsWith("Email"):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Email (e.g., example@domain.com): ");
                    Console.ResetColor();
                    email = Console.ReadLine() ?? "";
                    while (true)
                    {
                        var (isValid, message) = LoginLogic.IsEmailValid(email);
                        if (isValid)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Email (e.g., example@domain.com): ");
                            Console.ResetColor();
                            email = Console.ReadLine() ?? "";
                        }
                    }
                    break;

                case var s when s?.StartsWith("Password"):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Password (8-16 characters, must include letters and numbers): ");
                    Console.ResetColor();
                    password = Console.ReadLine() ?? "";
                    while (true)
                    {
                        var (isValid, message) = LoginLogic.IsPasswordValid(password);
                        if (isValid)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine(message);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Password (8-16 characters, must include letters and numbers): ");
                            Console.ResetColor();
                            password = Console.ReadLine() ?? "";
                        }
                    }
                    break;
                case var s when s?.StartsWith("Phone number"):
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Phone number (10 digits): ");
                    Console.ResetColor();
                    phoneNumber = Console.ReadLine() ?? "";
                    while (true)
                    {
                        var (isValid, error) = LoginLogic.IsPhoneNumberValid(phoneNumber);
                        if (isValid)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine(error);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Phone number (10 digits): ");
                            Console.ResetColor();
                            phoneNumber = Console.ReadLine() ?? "";
                        }
                    }
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
