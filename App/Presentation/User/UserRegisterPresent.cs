using Restaurant;
using App.DataAccess.Utils;
using App.Presentation.User;
using App.Presentation.Admin;

namespace App.Presentation.User;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class UserRegisterPresent
{
    public static void CreateAccount(bool admin = false)
    {
        // ControlHelpPresent.Clear();
        // ControlHelpPresent.AddOptions("Back", "<escape>");
        // ControlHelpPresent.ShowHelp();
        Console.WriteLine("Please enter the following information:\n");

        // bool isRegistrationSuccessful = false;
        bool isAdminCreated = false;

        TryCatchHelper.EscapeKeyException(() =>
        {
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Escape", "<escape>");
            ControlHelpPresent.ShowHelp();
            string banner = admin ? "REGISTER : ADMIN\n\n" : "REGISTER\n\n"; // Dynamic banner
            // Use InputHelper.GetValidatedInput for streamlined input handling
            string firstName = InputHelper.GetValidatedInput<string>(
            "First name: ",
            input => InputHelper.InputNotNull(input, "First name"),
            menuTitle: banner,
            showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string lastName = InputHelper.GetValidatedInput<string>(
            "Last name: ",
            input => InputHelper.InputNotNull(input, "Last name"),
            menuTitle: banner,
            showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string email = InputHelper.GetValidatedInput<string>(
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
                menuTitle: banner,
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string password = InputHelper.GetValidatedInput<string>(
            "Password (8-16 characters, must include letters and numbers): ",
            input =>
            {
                var (isValid, message) = LoginLogic.IsPasswordValid(input);
                if (!isValid)
                {
                    if (message != null)
                    {
                        ControlHelpPresent.DisplayFeedback(message); // Show feedback to the user
                    }
                    return (null, null); // Return null to prompt the user again
                }
                return (input, null); // Return valid input
            },
            menuTitle: banner,
            showHelpAction: () => ControlHelpPresent.ShowHelp()
        );
            string phoneNumber = InputHelper.GetValidatedInput<string>(
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
                menuTitle: banner,
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            // reset help to default before confirming and saving the account
            // ControlHelpPresent.ResetToDefault();

            isAdminCreated = ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
        });

        // make sure controls are displayed again when escaping or returning
        ControlHelpPresent.ResetToDefault();
        // ControlHelpPresent.ShowHelp();
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
                // $"Password     : {new string('*', password.Length)}",
                $"Password     : {password}",
                $"Phone number : {phoneNumber}\n",
                "Save and return",
                "Cancel"
            };

            // add navigation options for the confirmation menu
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Exit", "<escape>");
            ControlHelpPresent.ShowHelp();
            // ControlHelpPresent.ResetToDefault();

            dynamic selection = SelectionPresent.Show(details, banner:"Review and select your account details you want to modify:").ElementAt(0).text!;

            if (selection == null || selection == "Cancel")
            {
                ControlHelpPresent.DisplayFeedback("Account creation canceled. All entered information has been discarded.", "bottom", "tip");
                ControlHelpPresent.ResetToDefault();
                return false;
            }
            
            if (selection == "Save and return")
            {
                // ControlHelpPresent.DisplayFeedback("\nSaving your account...", "bottom", "success");
                SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                return admin;
            }
            
            switch (selection)
            {
                case var s when s?.StartsWith("First name"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        firstName = InputHelper.GetValidatedInput<string>(
                            "First name: ",
                            input => InputHelper.InputNotNull(input, "First name"),
                            menuTitle: "EDIT FIRSTNAME",
                            showHelpAction: () =>
                        {
                            ControlHelpPresent.AddOptions("Exit", "<escape>");
                            ControlHelpPresent.ShowHelp();

                        });
                    });
                    ControlHelpPresent.DisplayFeedback($"Changed first name to {firstName}.", "bottom", "success");
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