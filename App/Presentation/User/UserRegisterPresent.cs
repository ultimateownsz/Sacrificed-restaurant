using App.DataAccess.Utils;
using Restaurant;

namespace App.Presentation.User;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class UserRegisterPresent
{
    public static void CreateAccount(bool admin = false)
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Back", "<escape>");
        ControlHelpPresent.ShowHelp();
        Console.WriteLine("Please enter the following information:\n");

        // Input collection with validation loops
        while (true)
        {
            firstName = TerminableUtilsPresent.ReadLine("first name: ");
            if (firstName == null) return;

            if (LoginLogic.IsNameValid(firstName))
                break;

            Console.WriteLine("Invalid first name, try again!");
            Console.ReadKey();
        }

        while (true)
        {
            lastName = TerminableUtilsPresent.ReadLine("last name: ");
            if (lastName == null) return;

            if (LoginLogic.IsNameValid(lastName))
                break;
            Console.WriteLine("Invalid last name, try again!");
            Console.ReadKey();
        }

        // Loop until valid email is provided
        while (true)
        {
            email = TerminableUtilsPresent.ReadLine("email: ");
            if (email == null) return;

            if (LoginLogic.IsEmailValid(email))
                break;

            Console.WriteLine("Invalid email address, try again!");
            Console.ReadKey();
        }

        // Loop until valid password is provided
        while (true)
        {
            password = TerminableUtilsPresent.ReadLine("password (8-16 characters): ");
            if (password == null) return;

            if (LoginLogic.IsPasswordValid(password))
                break;

            Console.WriteLine("Invalid password, try again!");
            Console.ReadKey();
        }

        // Loop until valid phone number is provided
        while (true)
        {
            phoneNumber = TerminableUtilsPresent.ReadLine("phone number (8 numbers): ");
            if (phoneNumber == null) return;

            if (LoginLogic.IsPhoneNumberValid(phoneNumber))
                break;

            Console.WriteLine("Invalid phone number, try again!");
            Console.ReadKey();
        }

        // Confirm details loop
        while (!isInfoValid)
        {
            Console.Clear();
            Console.WriteLine("Your information: ");
            Console.WriteLine(" ");
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Phone Number: {phoneNumber}");
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");

            string choice = Console.ReadLine().ToUpper();

            if (selection == null || selection == "Cancel")
            {
                // Create account and write it to storage
                var account = new UserModel()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    Phone = phoneNumber,
                    Admin = Convert.ToInt16(admin)
                };

                Access.Users.Write(account);
                Console.WriteLine("\nYour account is successfully registered!");
                Thread.Sleep(1000); // so you can read the messages
                isInfoValid = true; // Exit the confirmation loop

            }
            else if (choice == "N")
            {
                while (true)
                {
                    string banner = "Choose which information you'd like to change:";
                    switch (SelectionPresent.Show(["first name", "last name", "email", "password", "phone number"], banner: banner).ElementAt(0).text)
                    {
                        case "first name":
                            Console.Clear();
                            Console.Write("first name: ");
                            firstName = Console.ReadLine();
                            break;

                case var s when s?.StartsWith("Last name"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        lastName = InputHelper.GetValidatedInput<string>(
                            "Last name: ",
                            input => InputHelper.InputNotNull(input, "Last name"),
                            menuTitle: "EDIT LASTNAME",
                            showHelpAction: () =>
                        {
                            ControlHelpPresent.AddOptions("Exit", "<escape>");
                            ControlHelpPresent.ShowHelp();

                        });
                    });
                    ControlHelpPresent.DisplayFeedback($"Changed last name to {lastName}.", "bottom", "success");
                    break;
                
                case var s when s?.StartsWith("Email"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        email = InputHelper.GetValidatedInput<string>(
                        "Email (e.g., example@domain.com): ",
                        input =>
                        {
                            var (isValid, message) = LoginLogic.IsEmailValid(input);
                            if (!isValid)
                            {
                                if (message != null)
                                {
                                    ControlHelpPresent.DisplayFeedback(message);
                                }
                                return (null, null);
                            }
                            return (input, null);
                        },
                        menuTitle: "EDIT EMAIL",
                        showHelpAction: () =>
                        {
                            ControlHelpPresent.AddOptions("Exit", "<escape>");
                            ControlHelpPresent.ShowHelp();

                        });
                    });
                    ControlHelpPresent.DisplayFeedback($"Changed email to {email}.", "bottom", "success");
                    break;
                
                case var s when s?.StartsWith("Password"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        password = InputHelper.GetValidatedInput<string>(
                        "Password (8-16 characters, must include letters and numbers): ",
                        input =>
                        {
                            var (isValid, message) = LoginLogic.IsPasswordValid(input);
                            if (!isValid)
                            {
                                if (message != null)
                                {
                                    ControlHelpPresent.DisplayFeedback(message);
                                }
                                return (null, null);
                            }
                            return (input, null);
                        },
                        menuTitle: "EDIT PASSWORD",
                        showHelpAction: () =>
                        {
                            ControlHelpPresent.AddOptions("Exit", "<escape>");
                            ControlHelpPresent.ShowHelp();

                        });
                    });
                    ControlHelpPresent.DisplayFeedback($"Changed password to {password}.", "bottom", "success");
                    break;
                
                case var s when s?.StartsWith("Phone number"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        phoneNumber = InputHelper.GetValidatedInput<string>(
                        "Phone number (10 digits): ",
                        input =>
                        {
                            var (isValid, error) = LoginLogic.IsPhoneNumberValid(input);
                            if (!isValid)
                            {
                                if (error != null)
                                {
                                    ControlHelpPresent.DisplayFeedback(error);
                                }
                                return (null, null);
                            }
                            return (input, null);
                        },
                        menuTitle: "EDIT PHONE NUMBER",
                        showHelpAction: () =>
                        {
                            ControlHelpPresent.AddOptions("Exit", "<escape>");
                            ControlHelpPresent.ShowHelp();

                        });
                    });
                    ControlHelpPresent.DisplayFeedback($"Changed phone number to {phoneNumber}.", "bottom", "success");
                    break;

                default:
                    ControlHelpPresent.DisplayFeedback("Invalid option selected.", "bottom", "error");
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
        if (admin)
        {
            ControlHelpPresent.DisplayFeedback("Admin account has been created!", "bottom", "success");
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("Your account has been successfully created!", "bottom", "success");
        }
    }
}