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

        Console.Clear();
        Console.WriteLine(banner);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("First name: ");
        Console.ResetColor();
        string firstName = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("First name cannot be empty.");
            Console.Clear();
            Console.WriteLine(banner);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("First name: ");
            Console.ResetColor();
            firstName = Console.ReadLine() ?? "";
        }

        Console.Clear();
        Console.WriteLine(banner);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Last name: ");
        Console.ResetColor();
        string lastName = Console.ReadLine() ?? "";
        while (string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Last name cannot be empty.");
            Console.Clear();
            Console.WriteLine(banner);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Last name: ");
            Console.ResetColor();
            lastName = Console.ReadLine() ?? "";
        }

        Console.Clear();
        Console.WriteLine(banner);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Email (e.g., example@domain.com): ");
        Console.ResetColor();
        string email = Console.ReadLine() ?? "";
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
                Console.Clear();
                Console.WriteLine(banner);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Email (e.g., example@domain.com): ");
                Console.ResetColor();
                email = Console.ReadLine() ?? "";
            }
        }

        Console.Clear();
        Console.WriteLine(banner);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Password (8-16 characters, must include letters and numbers): ");
        Console.ResetColor();
        string password = Console.ReadLine() ?? "";
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
                Console.Clear();
                Console.WriteLine(banner);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Password (8-16 characters, must include letters and numbers): ");
                Console.ResetColor();
                password = Console.ReadLine() ?? "";
            }
        }

        Console.Clear();
        Console.WriteLine(banner);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Phone number (10 digits): ");
        Console.ResetColor();
        string phoneNumber = Console.ReadLine() ?? "";
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
