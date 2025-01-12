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

                var selection = SelectionPresent.Show(["Create a new admin account", "Make existing user admin\n", "Cancel"], banner:"ACCOUNT REGISTRATION").ElementAt(0).text;
                if (selection == null || selection == null || selection == "Cancel")
                {
                    // ControlHelpPresent.DisplayFeedback("Admin account creation canceled.", "bottom", "error");
                    return;
                }
                else if (selection == "Make existing user admin\n")
                {
                    PromoteUserToAdmin();
                    // ControlHelpPresent.ResetToDefault();
                    return;
                }
            }

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
            ControlHelpPresent.ResetToDefault();

            isAdminCreated = ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
        });

        // make sure controls are displayed again when escaping or returning
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();
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
        var userAccess = new DataAccess<UserModel>(new[] { "ID", "FirstName", "LastName", "Email", "Password", "Phone", "Admin" });

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

    // private static void PromoteUserToAdmin()
    // {
    //     Console.Clear();
    //     ControlHelpPresent.Clear();
    //     ControlHelpPresent.AddOptions("Escape", "<escape>");
    //     ControlHelpPresent.ShowHelp();

    //     int footerHeight = ControlHelpPresent.GetFooterHeight();
    //     int inputAreaHeight = Console.WindowHeight - footerHeight - 2;

    //     Console.SetCursorPosition(0, 0);
        
    //     Console.WriteLine("Enter the first and last name of the user you want to promote to admin.");
    //     Console.WriteLine("");

    //     Console.Write("First name: ");
    //     string firstName = Console.ReadLine()?.Trim();
    //     Console.Write("Last name: ");
    //     string lastName = Console.ReadLine()?.Trim();

    //     // Validate inputs
    //     if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
    //     {
    //         ControlHelpPresent.DisplayFeedback("Both first and last names are required. Promoting user canceled.");
    //         ControlHelpPresent.DisplayFeedback("Press any key to return to the menu...", "bottom", "tip");
    //         Console.ReadKey();
    //         return;
    //     }
        
    //     // Create an instance fpr UserModel
    //     var userAccess = new DataAccess<UserModel>(new[] { "ID", "FirstName", "LastName", "Email", "Password", "Phone", "Admin" });

    //     // Fetch user bases on the input
    //     var user = userAccess.GetAllBy("FirstName", firstName)
    //         .FirstOrDefault(u =>
    //             string.Equals(u.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
    //             u.Admin == 0); // Only fetch users
        
    //     if (user == null)
    //     {
    //         ControlHelpPresent.DisplayFeedback("User not found.");
    //         ControlHelpPresent.DisplayFeedback("Press any key to return to the menu...", "bottom", "tip");
    //         Console.ReadKey();
    //         return;
    //     }

    //     // Confirmation for promoting
    //     Console.Clear();
    //     ControlHelpPresent.ResetToDefault();

    //     var confirmationOptions = new List<string> { "Yes", "No" };
    //     string confirmation = SelectionPresent.Show(confirmationOptions, banner: "Are you sure?").ElementAt(0).text;

    //     if (confirmation == "Yes")
    //     {
    //         // Update the user's Admin status
    //         user.Admin = 1;
    //         if (userAccess.Update(user))
    //         {
    //             ControlHelpPresent.DisplayFeedback("User successfully promoted to admin.", "bottom", "success");
    //         }
    //         else
    //         {
    //             ControlHelpPresent.DisplayFeedback("Failed to promote the user. Try again.");
    //         }
    //     }
    //     else
    //     {
    //         ControlHelpPresent.DisplayFeedback("Action canceled. User was not promoted.");
    //     }

    //     ControlHelpPresent.DisplayFeedback("Press any key to return to the menu...", "bottom", "tip");
    //     Console.ReadKey();
    // }


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
            // ControlHelpPresent.AddOptions("Exit", "<escape>");
            // ControlHelpPresent.ShowHelp();
            ControlHelpPresent.ResetToDefault();

            dynamic selection = SelectionPresent.Show(details, banner:"Review and select your account details you want to modify:").ElementAt(0).text!;

            if (selection == null || selection == "Cancel")
            {
                ControlHelpPresent.DisplayFeedback("Account creation canceled. All entered information has been discarded.", "bottom", "tip");
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