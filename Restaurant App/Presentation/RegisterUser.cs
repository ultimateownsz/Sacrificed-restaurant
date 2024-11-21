namespace Project.Presentation;
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
            Console.Write("email address: ");
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
            Console.WriteLine($"First Name: {firstName}");
            Console.WriteLine($"Last Name: {lastName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Phone Number: {phoneNumber}");
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");

            string choice = Console.ReadLine().ToUpper();

            if (choice == "Y")
            {
                // Create account and write it to storage
                var account = AccountsLogic.UserAccount(firstName, lastName, email, password, phoneNumber);
                AccountsAccess.Write(account);
                Console.WriteLine("\nYour account is successfully registered!");
                Thread.Sleep(1000); // so you can read the messages
                isInfoValid = true; // Exit the confirmation loop

            }
            else if (choice == "N")
            {
                while (true)
                {
                    string banner = "Choose which information you'd like to change:\n\n";
                    switch (SelectionMenu.Show(["first name", "last name", "email", "password", "phone number"], banner))
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

        // Input collection with validation loops
        Console.WriteLine("Please, enter your first name: ");
        firstName = Console.ReadLine();

        Console.WriteLine("Please, enter your last name: ");
        lastName = Console.ReadLine();

        // Loop until valid email is provided
        while (true)
        {
            Console.WriteLine("Please, enter your email address: ");
            email = Console.ReadLine();
            if (AccountsLogic.IsEmailValid(email))
                break;
            Console.WriteLine("Invalid email address, try again!");
        }

        // Loop until valid password is provided
        while (true)
        {
            Console.WriteLine("Please, enter your password (Min: 8 characters | Max: 16 characters): ");
            password = Console.ReadLine();
            if (AccountsLogic.IsPasswordValid(password))
                break;
            Console.WriteLine("Invalid password, try again!");
        }

        // Loop until valid phone number is provided
        while (true)
        {
            Console.WriteLine("Please, enter your phone number (Must be 8 numbers): ");
            phoneNumber = Console.ReadLine();
            if (AccountsLogic.IsPhoneNumberValid(phoneNumber))
                break;
            Console.WriteLine("Invalid phone number, try again!");
        }

        // Confirm details loop
        while (!isInfoValid)
        {
            Console.WriteLine("Your information: ");
            Console.WriteLine(" ");
            Console.WriteLine($"First Name: {firstName}");
            Console.WriteLine($"Last Name: {lastName}");
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Phone Number: {phoneNumber}");
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");

            string choice = Console.ReadLine().ToUpper();

            if (choice == "Y")
            {
                // Create account and write it to storage
                var account = AccountsLogic.AdminAccount(firstName, lastName, email, password, phoneNumber);
                AccountsAccess.Write(account);
                Console.WriteLine("Your account is successfully registered!");
                isInfoValid = true; // Exit the confirmation loop
            }
            else if (choice == "N")
            {
                // Prompt to update specific fields
                Console.WriteLine("Choose which information you'd like to change:");
                Console.WriteLine("(Type in: [1] = First Name, [2] = Last Name, [3] = Email, [4] = Password, [5] = Phone Number)");

                string info = Console.ReadLine();
                switch (info)
                {
                    case "1":
                        Console.WriteLine("Please, enter your new first name: ");
                        firstName = Console.ReadLine();
                        break;
                    case "2":
                        Console.WriteLine("Please, enter your new last name: ");
                        lastName = Console.ReadLine();
                        break;
                    case "3":
                        while (true)
                        {
                            Console.WriteLine("Please, enter your new email address: ");
                            string newEmail = Console.ReadLine();
                            if (AccountsLogic.IsEmailValid(newEmail))
                            {
                                email = newEmail;
                                break;
                            }
                            Console.WriteLine("Invalid email address, try again!");
                        }
                        break;
                    case "4":
                        while (true)
                        {
                            Console.WriteLine("Please, enter your new password (Min: 8 characters | Max: 16 characters): ");
                            string newPassword = Console.ReadLine();
                            if (AccountsLogic.IsPasswordValid(newPassword))
                            {
                                password = newPassword;
                                break;
                            }
                            Console.WriteLine("Invalid password, try again!");
                        }
                        break;
                    case "5":
                        while (true)
                        {
                            Console.WriteLine("Please, enter your new phone number (Must be 8 numbers): ");
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
                        Console.WriteLine("Invalid selection, please choose a valid option.");
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
