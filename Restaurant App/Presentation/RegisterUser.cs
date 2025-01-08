using System.Collections;
using System.Numerics;

namespace Project.Presentation;

internal class RegisterUser
{
    public static void CreateAccount(bool admin = false)
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Back", "<escape>");
        ControlHelpPresent.ShowHelp();
        Console.WriteLine("Please enter the following information:\n");

        // bool isRegistrationSuccessful = false;
        bool isAdminCreated = false;

        TryCatchHelper.EscapeKeyException(() =>
        {
            if (admin)
            {
                // ControlHelpPresent.Clear();
                // ControlHelpPresent.AddOptions("Exit", "<escape>");
                ControlHelpPresent.ResetToDefault();
                ControlHelpPresent.ShowHelp();

                var selection = SelectionPresent.Show(["Create a new admin account", "Promote an existing user to admin\n", "Cancel"], banner:"ACCOUNT REGISTRATION\n\n").ElementAt(0).text;
                if (selection == null || selection == null || selection == "Cancel")
                {
                    ControlHelpPresent.DisplayFeedback("Admin account creation canceled.", "bottom", "error");
                    return;
                }

                if (selection == "Promote an existing user to admin")
                {
                    PromoteExistingUserToAdmin();
                    ControlHelpPresent.ResetToDefault();
                    return;
                }
            }

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
            ControlHelpPresent.ResetToDefault();

            isAdminCreated = ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
        });

        // make sure controls are displayed again when escaping or returning
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();
    }

    private static void PromoteExistingUserToAdmin()
    {
        // fetch all users who are not admins
        var nonAdminUsers = Access.Users.Read()
            .Where(u => u.Admin == 0)
            .ToList();

        if (!nonAdminUsers.Any())
        {
            ControlHelpPresent.DisplayFeedback("No users found to promote to admin.", "bottom", "error");
            return;
        }

        var userOptions = nonAdminUsers.Select(u => $"{u.FirstName} {u.LastName} - {u.Email}\n").ToList();
        userOptions.Add("Cancel");

        var userSelection = SelectionPresent.Show(userOptions, banner:"ACCOUNT PROMOTION\n\n").ElementAt(0).text;
        
        if (string.IsNullOrEmpty(userSelection) || userSelection == "Cancel")
        {
            ControlHelpPresent.DisplayFeedback("Account promotion canceled", "bottom", "error");
            return;
        }

        var selectedUser = nonAdminUsers.FirstOrDefault(u => userSelection.StartsWith($"{u.FirstName} {u.LastName} - {u.Email}") == true);
        
        if (selectedUser != null)
        {
            selectedUser.Admin = 1;
            Access.Users.Update(selectedUser);
            ControlHelpPresent.DisplayFeedback($"{selectedUser.FirstName} {selectedUser.LastName} has been promoted to admin.", "bottom", "success");
            return;
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("Promotion to admin was canceled.", "bottom", "error");
            return;
        }
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

            dynamic selection = SelectionPresent.Show(details, banner:"Review and select your account details you want to modify:\n\n").ElementAt(0).text!;

            if (selection.text == null || selection.text == "Cancel")
            {
                ControlHelpPresent.DisplayFeedback("Account creation canceled. All entered information has been discarded.", "bottom", "tip");
                return false;
            }
            
            if (selection.text == "Save and return")
            {
                // ControlHelpPresent.DisplayFeedback("\nSaving your account...", "bottom", "success");
                SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                return admin;
            }
            
            switch (selection?.text)
            {
                case var s when s?.StartsWith("First name"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        firstName = InputHelper.GetValidatedInput<string>(
                            "First name: ",
                            input => InputHelper.InputNotNull(input, "First name"),
                            menuTitle: "EDIT FIRSTNAME",
                            showHelpAction: () => ControlHelpPresent.ShowHelp()
                        );
                    });
                    ControlHelpPresent.DisplayFeedback("First name edit canceled.", "bottom", "error");
                    break;

                case var s when s?.StartsWith("Last name"):
                    TryCatchHelper.EscapeKeyException(() =>
                    {
                        lastName = InputHelper.GetValidatedInput<string>(
                            "Last name: ",
                            input => InputHelper.InputNotNull(input, "Last name"),
                            menuTitle: "EDIT LASTNAME",
                            showHelpAction: () => ControlHelpPresent.ShowHelp()
                        );
                    });
                    ControlHelpPresent.DisplayFeedback("Last name edit canceled.", "bottom", "error");
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
                        showHelpAction: () => ControlHelpPresent.ShowHelp()
                        );
                    });
                    ControlHelpPresent.DisplayFeedback("Email edit canceled.", "bottom", "error");
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
                        showHelpAction: () => ControlHelpPresent.ShowHelp()
                        );
                    });
                    ControlHelpPresent.DisplayFeedback("Password edit canceled.", "bottom", "error");
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
                        showHelpAction: () => ControlHelpPresent.ShowHelp()
                    );
                    });
                    ControlHelpPresent.DisplayFeedback("Phone number edit canceled.", "bottom", "error");
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