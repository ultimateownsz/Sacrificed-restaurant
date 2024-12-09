using System.Numerics;

namespace Project.Presentation;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class RegisterUser
{
    public static void CreateAccount(bool admin = false)
    {
        bool isInfoValid = false;
        string firstName = "", lastName = "", email = "", password = "", phoneNumber = "";

        Console.Clear();
        Console.WriteLine("Please enter the following information:\n");

        // Input collection with validation loops
        Console.Write("First name: ");
        Console.Write("First name: ");
        firstName = Console.ReadLine();

        Console.Write("Last name: ");
        Console.Write("Last name: ");
        lastName = Console.ReadLine();

        // Loop until valid email is provided
        while (true)
        {
            Console.Write("Email: ");
            Console.Write("Email: ");
            email = Console.ReadLine();
            if (UserLogic.IsEmailValid(email))
                break;
            Console.WriteLine("Invalid email address, try again!");
        }

        // Loop until valid password is provided
        while (true)
        {
            Console.Write("Password (8-16 characters): ");
            Console.Write("Password (8-16 characters): ");
            password = Console.ReadLine();
            if (UserLogic.IsPasswordValid(password))
                break;
            Console.WriteLine("Invalid password, try again!");
        }

        // Loop until valid phone number is provided
        while (true)
        {
            Console.Write("Phone number (8 numbers): ");
            Console.Write("Phone number (8 numbers): ");
            phoneNumber = Console.ReadLine();
            if (UserLogic.IsPhoneNumberValid(phoneNumber))
                break;
            Console.WriteLine("Invalid phone number, try again!");
        }

        // Confirm details loop
        while (!isInfoValid)
        {
            Console.Clear();
            string confirm_info = $"Your information:\n\nFirst name: {firstName}\nLast name: {lastName}\nEmail: {email}\nPassword: {password}\nPhone Number: {phoneNumber}\n\nAre you sure this information is correct?\n";

            switch (SelectionPresent.Show(["Yes", "No"], confirm_info).text)
            {
                case "Yes":
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
                    isInfoValid = true; // Exit the confirmation loop
                    Console.WriteLine("\nYour account is successfully registered!");
                    Thread.Sleep(1000); // so you can read the messages
                    break;

                case "No":
                    while (true)
                    {
                        string banner = "Choose which information you'd like to change:\n\n";
                        switch (SelectionPresent.Show(["First name", "Last name", "Email", "Password", "Phone number", "\nBack"], banner).text)
                        {
                            case "First name":
                                Console.Clear();
                                Console.Write("First name: ");
                                firstName = Console.ReadLine();
                                break;

                            case "Last name":
                                Console.Clear();
                                Console.Write("Last name: ");
                                lastName = Console.ReadLine();
                                break;

                            case "Email":
                                while (true)
                                {
                                    Console.Clear();
                                    Console.Write("Email address: ");
                                    string newEmail = Console.ReadLine();
                                    if (UserLogic.IsEmailValid(newEmail))
                                    {
                                        email = newEmail;
                                        break;
                                    }
                                    Console.WriteLine("Invalid email address, try again!");
                                }
                                break;

                            case "Password":
                                while (true)
                                {
                                    Console.Clear();
                                    Console.Write("Password(8-16 characters): ");
                                    string newPassword = Console.ReadLine();
                                    if (UserLogic.IsPasswordValid(newPassword))
                                    {
                                        password = newPassword;
                                        break;
                                    }
                                    Console.WriteLine("Invalid password, try again!");
                                }
                                break;

                            case "Phone number":
                                while (true)
                                {
                                    Console.Clear();
                                    Console.Write("Phone number (8 numbers): ");
                                    string newPhoneNumber = Console.ReadLine();
                                    if (UserLogic.IsPhoneNumberValid(newPhoneNumber))
                                    {
                                        phoneNumber = newPhoneNumber;
                                        break;
                                    }
                                    Console.WriteLine("Invalid phone number, try again!");
                                }
                                break;

                            case "Back":
                                break;

                            default:
                                continue;
                                
                        }
                        // valid input has been provided
                        break;
                    }

                break;
            }
        }
    }
}
