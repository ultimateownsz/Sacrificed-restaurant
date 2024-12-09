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
        Console.WriteLine("Please enter the following information:\n");

        TryCatchHelper.EscapeKeyException(() =>
        {
        // Use InputHelper.GetValidatedInput for streamlined input handling
        string firstName = InputHelper.GetValidatedInput<string>(
        "First name: ",
        input => InputHelper.InputNotNull(input, "First name")
        );
        string lastName = InputHelper.GetValidatedInput<string>(
        "Last name: ",
        input => InputHelper.InputNotNull(input, "Last name")
        );
        string email = InputHelper.GetValidatedInput<string>(
            "Email: ",
            input => UserLogic.IsEmailValid(input) ? (input, null) : (null, "Invalid email address, try again!")
        );
        string password = InputHelper.GetValidatedInput<string>(
            "Password (8-16 characters): ",
            input => UserLogic.IsPasswordValid(input) ? (input, null) : (null, "Password needs to have between 8-16 characters):")
        );
        string phoneNumber = InputHelper.GetValidatedInput<string>(
            "Phone number (10 numbers): ",
            input => UserLogic.IsPhoneNumberValid(input) ? (input, null) : (null, "Invalid phone number.")
        );

        ConfirmAndSaveAccount(firstName, lastName, email, password, phoneNumber, admin);
        });
        // }
        // catch (OperationCanceledException)
        // {
        //     Console.WriteLine("\nReturning to the previous menu...");
        //     return;
        // }
    }

    private static void ConfirmAndSaveAccount(string firstName, string lastName, string email, string password, string phoneNumber, bool admin)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Your information:\n");
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Phone Number: {phoneNumber}\n");
            string banner = "Are you sure this is correct?\n\n";
            switch (SelectionPresent.Show(["Yes", "No"], banner).text)
            {
                case "Yes":
                    SaveAccount(firstName, lastName, email, password, phoneNumber, admin);
                    return;

                case "No":
                    (firstName, lastName, email, password, phoneNumber) = EditInformation(firstName, lastName, email, password, phoneNumber);
                    break;
                
                default:
                    Console.WriteLine("Invalid choice. Please select 'Yes' or 'No'.");
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
        Thread.Sleep(1000);
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
                    input => InputHelper.InputNotNull(input, "First name")
                );
                break;

            case "Last name":
                lastName = InputHelper.GetValidatedInput<string>(
                    "Last name: ",
                    input => InputHelper.InputNotNull(input, "Last name")
                );
                break;

            case "Email":
                email = InputHelper.GetValidatedInput<string>(
                    "Email: ",
                    input => UserLogic.IsEmailValid(input) ? (input, null) : (null, "Invalid email address.")
                );
                break;

            case "Password":
                password = InputHelper.GetValidatedInput<string>(
                    "Password (8-16 characters): ",
                    input => UserLogic.IsPasswordValid(input) ? (input, null) : (null, "Invalid password.")
                );
                break;

            case "Phone number":
                phoneNumber = InputHelper.GetValidatedInput<string>(
                    "Phone number (10 numbers): ",
                    input => UserLogic.IsPhoneNumberValid(input) ? (input, null) : (null, "Invalid phone number.")
                );
                break;
        }

        return (firstName, lastName, email, password, phoneNumber);
    }
}
