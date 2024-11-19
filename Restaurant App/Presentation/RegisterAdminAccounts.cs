public static class RegisterAdminAccount
{
    public static void CreateAdminAccount()
    {
        bool isInfoValid = false;
        string firstName = "", lastName = "", email = "", password = "", phoneNumber = "";
        AccountsLogic accL = null;
        accL = new AccountsLogic();

        // Input collection with validation loops
        Console.Write("Please, enter your first name: ");
        firstName = Console.ReadLine();

        Console.Write("Please, enter your last name: ");
        lastName = Console.ReadLine();

        // Loop until valid email is provided
        while (true)
        {
            Console.Write("Please, enter your email address: ");
            email = Console.ReadLine();
            if (accL.IsEmailValid(email))
                break;
            Console.WriteLine("Invalid email address, try again!");
        }

        // Loop until valid password is provided
        while (true)
        {
            Console.Write("Please, enter your password (Min: 8 characters | Max: 16 characters): ");
            password = Console.ReadLine();
            if (accL.IsPasswordValid(password))
                break;
            Console.WriteLine("Invalid password, try again!");
        }

        // Loop until valid phone number is provided
        while (true)
        {
            Console.Write("Please, enter your phone number (Must be 8 numbers): ");
            phoneNumber = Console.ReadLine();
            if (accL.IsPhoneNumberValid(phoneNumber))
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
            Console.Write("Are you sure this is correct? Y/N: ");
            
            string choice = Console.ReadLine().ToUpper();
            
            if (choice == "Y")
            {
                // Create account and write it to storage
                var account = accL.AdminAccount(firstName, lastName, email, password, phoneNumber);
                AccountsAccess.Write(account);
                Console.WriteLine("Your account is successfully registered!");
                isInfoValid = true; // Exit the confirmation loop
                AdminMenu.AdminStart();
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
                        Console.Write("Please, enter your new first name: ");
                        firstName = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Please, enter your new last name: ");
                        lastName = Console.ReadLine();
                        break;
                    case "3":
                        while (true)
                        {
                            Console.Write("Please, enter your new email address: ");
                            string newEmail = Console.ReadLine();
                            if (accL.IsEmailValid(newEmail))
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
                            Console.Write("Please, enter your new password (Min: 8 characters | Max: 16 characters): ");
                            string newPassword = Console.ReadLine();
                            if (accL.IsPasswordValid(newPassword))
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
                            Console.Write("Please, enter your new phone number (Must be 8 numbers): ");
                            string newPhoneNumber = Console.ReadLine();
                            if (accL.IsPhoneNumberValid(newPhoneNumber))
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