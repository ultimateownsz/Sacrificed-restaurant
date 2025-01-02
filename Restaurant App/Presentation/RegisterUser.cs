using System.Collections;
using System.Numerics;

namespace Project.Presentation;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class RegisterUser
{
    public static void CreateAccount(bool admin = false)
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Back", "<escape>");
        ControlHelpPresent.ShowHelp();
        Console.WriteLine("Please enter the following information:\n");

        bool isRegistrationSuccessful = false;

        TryCatchHelper.EscapeKeyException(() =>
        {
            // Use InputHelper.GetValidatedInput for streamlined input handling
            string firstName = InputHelper.GetValidatedInput<string>(
            "First name: ",
            input => InputHelper.InputNotNull(input, "First name"),
            menuTitle: "REGISTER",
            showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string lastName = InputHelper.GetValidatedInput<string>(
            "Last name: ",
            input => InputHelper.InputNotNull(input, "Last name"),
            menuTitle: "REGISTER",
            showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string email = InputHelper.GetValidatedInput<string>(
                "Email (e.g., example@domain.com): ",
                input =>
                {
                    var (isValid, message) = UserLogic.IsEmailValid(input);
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
                menuTitle: "REGISTER",
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            string password = InputHelper.GetValidatedInput<string>(
            "Password (8-16 characters, must include letters and numbers): ",
            input =>
            {
                var (isValid, message) = UserLogic.IsPasswordValid(input);
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
            menuTitle: "REGISTER",
            showHelpAction: () => ControlHelpPresent.ShowHelp()
        );
            string phoneNumber = InputHelper.GetValidatedInput<string>(
                "Phone number (10 digits): ",
                input =>
                {
                    var (isValid, error) = UserLogic.IsPhoneNumberValid(input);
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
                menuTitle: "REGISTER",
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
            // reset help to default before confirming and saving the account
            ControlHelpPresent.ResetToDefault();

            ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
            isRegistrationSuccessful = true;
        });

        if (isRegistrationSuccessful)
        {
            ControlHelpPresent.DisplayFeedback("\nYour account has been successfully created!", "bottom", "success");
        }
        else
        {
            ControlHelpPresent.DisplayFeedback("\nAccount creation was canceled. All entered information has been discarded.", "bottom", "tip");
        }

        // make sure controls are displayed again when escaping or returning
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();
    }

    private static void ShowAccountDetails(string firstName, string lastName, string email, string password, string phoneNumber)
    {
        Console.Clear();
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();

        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Review and modify your account details:\n");
        Console.WriteLine($"First Name   : {firstName}");
        Console.WriteLine($"Last Name    : {lastName}");
        Console.WriteLine($"Email        : {email}");
        Console.WriteLine($"Password     : {new string('*', password.Length)}"); // Mask password
        Console.WriteLine($"Phone Number : {phoneNumber}");
    }


    private static void ConfirmAndSaveAccount(string firstName, string lastName, string email, string password, string phoneNumber, bool admin)
    {
        while (true)
        {
            // display the account details for review
            ShowAccountDetails(firstName, lastName, email, password, phoneNumber);

            // Pause for the user to review the account details
            ControlHelpPresent.DisplayFeedback("\nPress any key to continue...", "bottom", "tip");
            Console.ReadKey(intercept: true); // Wait for user input to proceed

            // add navigation options for the confirmation menu
            ControlHelpPresent.ShowHelp();

            List<string> options = new()
            {
                "Save and return",
                "Edit first name",
                "Edit last name",
                "Edit email",
                "Edit password",
                "Edit phone number"
            };

            dynamic selection = SelectionPresent.Show(options, "Choose an option:\n\n");
            
            if (selection.text == null) return;  // escape is pressed

            if (selection.text == null || selection.text == "Save and return")
            {
                ControlHelpPresent.DisplayFeedback("\nSaving your account...", "bottom", "success");
                SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                return;
            }
            
            switch (selection.text)
            {
                case "Edit first name":
                    firstName = InputHelper.GetValidatedInput<string>(
                    "First Name: ",
                    input => InputHelper.InputNotNull(input, "First name")
                    );
                    break;

                case "Edit last name":
                    lastName = InputHelper.GetValidatedInput<string>(
                    "Last Name: ",
                    input => InputHelper.InputNotNull(input, "Last name")
                    );
                    break;
                
                case "Edit email":
                    email = InputHelper.GetValidatedInput<string>(
                    "Email (e.g., example@domain.com): ",
                    input =>
                    {
                        var (isValid, message) = UserLogic.IsEmailValid(input);
                        if (!isValid)
                        {
                            if (message != null)
                            {
                                ControlHelpPresent.DisplayFeedback(message);
                            }
                            return (null, null);
                        }
                        return (input, null);
                    });
                    break;
                
                case "Edit password":
                    password = InputHelper.GetValidatedInput<string>(
                    "Password (8-16 characters, must include letters and numbers): ",
                    input =>
                    {
                        var (isValid, message) = UserLogic.IsPasswordValid(input);
                        if (!isValid)
                        {
                            if (message != null)
                            {
                                ControlHelpPresent.DisplayFeedback(message); // Show feedback to the user
                            }
                            return (null, null); // Return null to prompt the user again
                        }
                        return (input, null); // Return valid input
                    });
                    break;
                
                case "Edit phone number":
                    phoneNumber = InputHelper.GetValidatedInput<string>(
                        "Phone number (10 digits): ",
                        input =>
                        {
                            var (isValid, error) = UserLogic.IsPhoneNumberValid(input);
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
                        menuTitle: "REGISTER",
                        showHelpAction: () => ControlHelpPresent.ShowHelp()
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
        ControlHelpPresent.DisplayFeedback("\nYour account has been successfully updated!", "bottom", "success");
    }

    // private static (string firstName, string lastName, string email, string password, string phoneNumber) EditInformation(
    //     string firstName, string lastName, string email, string password, string phoneNumber)
    // {
    //     ControlHelpPresent.ShowHelp();
    //     string banner = "Choose which information you'd like to change:\n\n";
    //     string fieldToChange = SelectionPresent.Show([ "First name", "Last name", "Email", "Password", "Phone number" ], banner).text;

    //     switch (fieldToChange)
    //     {
    //         case "First name":
    //             firstName = InputHelper.GetValidatedInput<string>(
    //                 "First name: ",
    //                 input => InputHelper.InputNotNull(input, "First name"),
    //                 menuTitle: "EDIT USER INFORMATION",
    //                 showHelpAction: () => ControlHelpPresent.ShowHelp()
    //             );
    //             break;

    //         case "Last name":
    //             lastName = InputHelper.GetValidatedInput<string>(
    //                 "Last name: ",
    //                 input => InputHelper.InputNotNull(input, "Last name"),
    //                 menuTitle: "EDIT USER INFORMATION",
    //                 showHelpAction: () => ControlHelpPresent.ShowHelp()
    //             );
    //             break;

    //         case "Email":
    //             email = InputHelper.GetValidatedInput<string>(
    //                 "Email (e.g., example@domain.com): ",
    //                 input =>
    //                 {
    //                     var (isValid, message) = UserLogic.IsEmailValid(input);
    //                     return isValid ? (input, null) : (null, message);
    //                 },
    //                 menuTitle: "EDIT USER INFORMATION",
    //                 showHelpAction: () => ControlHelpPresent.ShowHelp());
    //             break;

    //         case "Password":
    //             password = InputHelper.GetValidatedInput<string>(
    //                 "Password (8-16 characters, must include letters and numbers): ",
    //                 input => UserLogic.IsPasswordValid(input) ? (input, null) : (null, "Password must be 8-16 characters long and include both letters and numbers."),
    //                 menuTitle: "EDIT USER INFORMATION",
    //                 showHelpAction: () => ControlHelpPresent.ShowHelp()
    //             );
    //             break;

    //         case "Phone number":
    //             phoneNumber = InputHelper.GetValidatedInput<string>(
    //                 "Phone number (10 digits): ",
    //                 input => UserLogic.IsPhoneNumberValid(input) ? (input, null) : (null, "Phone number must contain exactly 10 digits (e.g., 1234567890)."),
    //                 menuTitle: "EDIT USER INFORMATION",
    //                 showHelpAction: () => ControlHelpPresent.ShowHelp()
    //             );
    //             break;
    //     }
    //     ControlHelpPresent.ResetToDefault();

    //     return (firstName, lastName, email, password, phoneNumber);
    // }
}
