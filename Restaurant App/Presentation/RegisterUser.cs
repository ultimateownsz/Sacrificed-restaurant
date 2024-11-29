namespace Project.Presentation;

// The methods underneath could've easily been one method.
// Original developer research modularity and reusability

internal class RegisterUser
{
    public static void CreateUserAccount()
    {
        bool isInfoValid = false;
        string firstName = "", lastName = "", email = "", password = "", phoneNumber = "";

        Console.Clear();
        Console.WriteLine("Please enter the following information:\n");

        // Input collection with validation loops
        Console.Write("first name: ");
        firstName = Console.ReadLine();

        Console.Write("last name: ");
        lastName = Console.ReadLine();

        // Loop until valid email is provided
        while (true)
        {
            Console.Write("email: ");
            email = Console.ReadLine();
            if (AccountsLogic.IsEmailValid(email))
                break;
            Console.WriteLine("Invalid email address, try again!");
        }

        // Loop until valid password is provided
        while (true)
        {
            Console.Write("password (8-16 characters): ");
            password = Console.ReadLine();
            if (AccountsLogic.IsPasswordValid(password))
                break;
            Console.WriteLine("Invalid password, try again!");
        }

        // Loop until valid phone number is provided
        while (true)
        {
            Console.Write("phone number (8 numbers): ");
            phoneNumber = Console.ReadLine();
            if (AccountsLogic.IsPhoneNumberValid(phoneNumber))
                break;
            Console.WriteLine("Invalid phone number, try again!");
        }

        // Confirm details loop
        while (!isInfoValid)
        {
            Console.Clear();
            Console.WriteLine("Your information: ");
            Console.WriteLine(" ");
            Console.WriteLine($"first name: {firstName}");
            Console.WriteLine($"last name: {lastName}");
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"password: {password}");
            Console.WriteLine($"phone Number: {phoneNumber}");
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");

            string choice = Console.ReadLine().ToUpper();

            if (choice == "Y")
            {
                // Create account and write it to storage
                var account = AccountsLogic.UserAccount(firstName, lastName, email, password, phoneNumber);
                
                Access.Users.Write(account);
                Console.WriteLine("\nYour account is successfully registered!");
                Thread.Sleep(1000); // so you can read the messages
                isInfoValid = true; // Exit the confirmation loop

            }
            else if (choice == "N")
            {
                while (true)
                {
                    string banner = "Choose which information you'd like to change:\n\n";
                    switch (SelectionPresent.Show(["first name", "last name", "email", "password", "phone number"], banner).text)
                    {
                        case "first name":
                            Console.Clear();
                            Console.Write("first name: ");
                            firstName = Console.ReadLine();
                            break;

                        case "last name":
                            Console.Clear();
                            Console.Write("last name: ");
                            lastName = Console.ReadLine();
                            break;

                        case "email":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("email address: ");
                                string newEmail = Console.ReadLine();
                                if (AccountsLogic.IsEmailValid(newEmail))
                                {
                                    email = newEmail;
                                    break;
                                }
                                Console.WriteLine("Invalid email address, try again!");
                            }
                            break;

                        case "password":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("password (8-16 characters): ");
                                string newPassword = Console.ReadLine();
                                if (AccountsLogic.IsPasswordValid(newPassword))
                                {
                                    password = newPassword;
                                    break;
                                }
                                Console.WriteLine("Invalid password, try again!");
                            }
                            break;

                        case "phone number":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("phone number (8 numbers): ");
                                string newPhoneNumber = Console.ReadLine();
                                if (AccountsLogic.IsPhoneNumberValid(newPhoneNumber))
                                {
                                    phoneNumber = newPhoneNumber;
                                    break;
                                }
                                Console.WriteLine("Invalid phone number, try again!");
                            }
                            break;

                        default:
                            continue;
                    }

                    // valid input has been provided
                    break;
                }

            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 'Y' for Yes or 'N' for No.");
            }
        }
    }

    public static void CreateAdminAccount()
    {
        bool isInfoValid = false;
        string firstName = "", lastName = "", email = "", password = "", phoneNumber = "";

        Console.Clear();
        Console.WriteLine("Please enter the following information:\n");

        // Input collection with validation loops
        Console.Write("first name: ");
        firstName = Console.ReadLine();

        Console.Write("last name: ");
        lastName = Console.ReadLine();

        // Loop until valid email is provided
        while (true)
        {
            Console.Write("email: ");
            email = Console.ReadLine();
            if (AccountsLogic.IsEmailValid(email))
                break;
            Console.WriteLine("Invalid email address, try again!");
        }

        // Loop until valid password is provided
        while (true)
        {
            Console.WriteLine("password (8-16 characters): ");
            password = Console.ReadLine();
            if (AccountsLogic.IsPasswordValid(password))
                break;
            Console.WriteLine("Invalid password, try again!");
        }

        // Loop until valid phone number is provided
        while (true)
        {
            Console.WriteLine("phone number (8 numbers): ");
            phoneNumber = Console.ReadLine();
            if (AccountsLogic.IsPhoneNumberValid(phoneNumber))
                break;
            Console.WriteLine("Invalid phone number, try again!");
        }

        // Confirm details loop
        while (!isInfoValid)
        {
            Console.Clear();
            Console.WriteLine("Your information: ");
            Console.WriteLine(" ");
            Console.WriteLine($"first name: {firstName}");
            Console.WriteLine($"last name: {lastName}");
            Console.WriteLine($"email: {email}");
            Console.WriteLine($"password: {password}");
            Console.WriteLine($"phone number: {phoneNumber}");
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");

            string choice = Console.ReadLine().ToUpper();

            if (choice == "Y")
            {
                // Create account and write it to storage
                var account = AccountsLogic.AdminAccount(firstName, lastName, email, password, phoneNumber);
                Access.Users.Write(account);
                Console.WriteLine("Your account is successfully registered!");
                Thread.Sleep(1000);
                isInfoValid = true; // Exit the confirmation loop
            }
            else if (choice == "N")
            {
                while (true)
                {
                    string banner = "Choose which information you'd like to change:\n\n";
                    switch (SelectionPresent.Show(["first name", "last name", "email", "password", "phone number"], banner))
                    {
                        case "first name":
                            Console.Clear();
                            Console.Write("first name: ");
                            firstName = Console.ReadLine();
                            break;

                        case "last name":
                            Console.Clear();
                            Console.Write("last name: ");
                            lastName = Console.ReadLine();
                            break;

                        case "email":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("email address: ");
                                string newEmail = Console.ReadLine();
                                if (AccountsLogic.IsEmailValid(newEmail))
                                {
                                    email = newEmail;
                                    break;
                                }
                                Console.WriteLine("Invalid email address, try again!");
                            }
                            break;

                        case "password":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("password (8-16 characters): ");
                                string newPassword = Console.ReadLine();
                                if (AccountsLogic.IsPasswordValid(newPassword))
                                {
                                    password = newPassword;
                                    break;
                                }
                                Console.WriteLine("Invalid password, try again!");
                            }
                            break;

                        case "phone number":
                            while (true)
                            {
                                Console.Clear();
                                Console.Write("phone number (8 numbers): ");
                                string newPhoneNumber = Console.ReadLine();
                                if (AccountsLogic.IsPhoneNumberValid(newPhoneNumber))
                                {
                                    phoneNumber = newPhoneNumber;
                                    break;
                                }
                                Console.WriteLine("Invalid phone number, try again!");
                            }
                            break;

                        default:
                            continue;
                    }

                    // valid input has been provided
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 'Y' for Yes or 'N' for No.");
            }
        }
    }
}
