using System.Collections;
using System.Numerics;

namespace Project.Presentation;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class RegisterUser
{
    public static void CreateAccount(bool admin = false)
    {
        Console.Clear();
        Console.WriteLine("Controls:\nExit     : <escape>\n");
        Console.WriteLine("Please enter the following information:\n");

        TryCatchHelper.EscapeKeyException(() =>
        {
            // Use InputHelper.GetValidatedInput for streamlined input handling
            string firstName = InputHelper.GetValidatedInput<string>(
            "First name: ",
            input => InputHelper.InputNotNull(input, "First name"),
            menuTitle: "REGISTER"
            );
            string lastName = InputHelper.GetValidatedInput<string>(
            "Last name: ",
            input => InputHelper.InputNotNull(input, "Last name"),
            menuTitle: "REGISTER"
            );
            string email = InputHelper.GetValidatedInput<string>(
            "Email (e.g., example@domain.com): ",
            input =>
            {
                var (isValid, message) = UserLogic.IsEmailValid(input);
                return isValid ? (input, null) : (null, message);
            },
            menuTitle: "REGISTER"
            );
            string password = InputHelper.GetValidatedInput<string>(
                "Password (8-16 characters, must include letters and numbers): ",
                input => UserLogic.IsPasswordValid(input) ? (input, null) : (null, "Password must be 8-16 characters long and include both letters and numbers."),
                menuTitle: "REGISTER"
            );
            string phoneNumber = InputHelper.GetValidatedInput<string>(
                "Phone number (10 digits): ",
                input => UserLogic.IsPhoneNumberValid(input) ? (input, null) : (null, "Phone number must contain exactly 10 digits (e.g., 1234567890)."),
                menuTitle: "REGISTER"
            );

            ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
        });
    }

    private static void ConfirmAndSaveAccount(string firstName, string lastName, string email, string password, string phoneNumber, bool admin)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Please review your information:\n");
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Phone Number: {phoneNumber}\n");
            Console.WriteLine("Do you want to change anything?\n");
            Console.WriteLine("");
            Console.ReadKey();

            dynamic selection = SelectionPresent.Show(["\nYes", "No", "\nBack"]);

            
            if (selection.text == null) return;  // escape is pressed
            
            switch (selection.text)
            {
                case "Yes":
                    SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                    return;

                case "No":
                    (firstName, lastName, email, password, phoneNumber) = EditInformation(firstName, lastName, email, password, phoneNumber);
                    break;
                
                case "Back":
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
        Console.WriteLine("\nYour account is successfully registered!");
    }

    private static (string firstName, string lastName, string email, string password, string phoneNumber) EditInformation(
        string firstName, string lastName, string email, string password, string phoneNumber)
    {
        string banner = "Choose which information you'd like to change:\n\n";
        string fieldToChange = SelectionPresent.Show([ "First name", "Last name", "Email", "Password", "Phone number" ], banner).text;

        switch (fieldToChange)
        {
            case "First name":
                firstName = InputHelper.GetValidatedInput<string>(
                    "First name: ",
                    input => InputHelper.InputNotNull(input, "First name"),
                    menuTitle: "EDIT USER INFORMATION"
                );
                break;

            case "Last name":
                lastName = InputHelper.GetValidatedInput<string>(
                    "Last name: ",
                    input => InputHelper.InputNotNull(input, "Last name"),
                    menuTitle: "EDIT USER INFORMATION"
                );
                break;

            case "Email":
                email = InputHelper.GetValidatedInput<string>(
                    "Email (e.g., example@domain.com): ",
                    input =>
                    {
                        var (isValid, message) = UserLogic.IsEmailValid(input);
                        return isValid ? (input, null) : (null, message);
                    },
                    menuTitle: "EDIT USER INFORMATION");
                break;

            case "Password":
                password = InputHelper.GetValidatedInput<string>(
                    "Password (8-16 characters, must include letters and numbers): ",
                    input => UserLogic.IsPasswordValid(input) ? (input, null) : (null, "Password must be 8-16 characters long and include both letters and numbers."),
                    menuTitle: "EDIT USER INFORMATION"
                );
                break;

            case "Phone number":
                phoneNumber = InputHelper.GetValidatedInput<string>(
                    "Phone number (10 digits): ",
                    input => UserLogic.IsPhoneNumberValid(input) ? (input, null) : (null, "Phone number must contain exactly 10 digits (e.g., 1234567890)."),
                    menuTitle: "EDIT USER INFORMATION"
                );
                break;
        }

        return (firstName, lastName, email, password, phoneNumber);
    }
}
