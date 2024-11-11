public class AccountsLogic
{
    public AccountModel? CurrentAccount { get; private set; }
    public AccountsLogic()
    {
        
    }

    public AccountModel GetById(int id)
    {
        return AccountsAccess.GetById(id);
    }

    public AccountModel? CheckLogin(string email, string password)
    {
        AccountModel acc = AccountsAccess.GetByEmail(email);

        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;

            if (acc.IsAdmin == 1)
            {
                Console.WriteLine("Logged in as Admin.");
            }
            else
            {
                Console.WriteLine("Logged in as a User.");
            }

            return acc;
        }

        return null;
    }

    public void CreateAccount()
    {
        bool checker = false;
        string at = "@";
        string dotCom = ".com";

        while (!checker)
        {
            Console.WriteLine("Please, enter your first name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Please, enter your last name: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Please, enter your email address: ");
            string email = Console.ReadLine();
            while (!checker)
            {
                if (!email.Contains(at) || !email.Contains(dotCom))
                {
                    Console.WriteLine("Invalid email address, try again.");
                    Console.WriteLine("Please, enter your email address: ");
                    email = Console.ReadLine();
                }
                else
                {
                    checker = true;
                }
            }

            Console.WriteLine("Please, enter your password (Min: 8 characters | Max: 16: characters): ");
            string password = Console.ReadLine();
            while (!checker)
            {
                if (password.Length > 16 || password.Length < 8)
                {
                    Console.WriteLine("Invalid password, try again.");
                    Console.WriteLine("Please, enter your password (Min: 8 characters | Max: 16: characters): ");
                    password = Console.ReadLine();
                }
                else
                {
                    checker =  true;
                }
            }

            Console.WriteLine("Please, enter your phone number (Must be 8 numbers): ");
            string phoneNumber = Console.ReadLine();
            int phoneOutput;
            bool isPhoneValid = int.TryParse(phoneNumber, out phoneOutput) && phoneNumber.Length == 8;
            while (!checker)
            {
                if (!isPhoneValid)
                {
                    Console.WriteLine("Invalid phone number, try again.");
                    Console.WriteLine("Please, enter your phone number (Must be 8 numbers): ");
                    phoneNumber = Console.ReadLine();
                }
                else
                {
                    checker =  true;
                }
            }

            Console.WriteLine("Your information: ");
            Console.WriteLine(" ");
            Console.WriteLine(firstName);
            Console.WriteLine(lastName);
            Console.WriteLine(email);
            Console.WriteLine(password);
            Console.WriteLine(phoneNumber);
            Console.WriteLine(" ");
            Console.WriteLine("Are you sure this is correct? Y/N");
            string choice = Console.ReadLine().ToUpper();

            switch (choice)
            {
                case "Y":
                    Console.WriteLine("You're account is succesfully registered!");
                    checker = true;
                    break;
                case "N":
                    Console.WriteLine("Choose which information you'd like to change: ");
                    Console.WriteLine("(Type in: [1] = First Name [2] = Last Name [3] = Email [4] = Password [5] = Phone Number)");
                    string info = Console.ReadLine();
                    if (info == "1")
                    {
                        Console.WriteLine("Please, enter your new first name: ");
                        string newFirstName = Console.ReadLine();
                        firstName = newFirstName;
                        Console.WriteLine("You're account is succesfully registered!");
                        checker = true;
                    }
                    else if (info == "2")
                    {
                        Console.WriteLine("Please, enter new last name: ");
                        string newLastName = Console.ReadLine();
                        lastName = newLastName;
                        Console.WriteLine("You're account is succesfully registered!");
                        checker = true;
                    }
                    else if (info == "3")
                    {
                        Console.WriteLine("Please, enter your new email: ");
                        string newEmail = Console.ReadLine();
                        while (!checker)
                        {
                            if (!newEmail.Contains(at) || !newEmail.Contains(dotCom))
                            {
                                Console.WriteLine("Invalid email address, try again.");
                                Console.WriteLine("Please, enter your email address: ");
                                newEmail = Console.ReadLine();
                            }
                            else
                            {
                                checker = true;
                            }
                        }
                        email = newEmail;
                        Console.WriteLine("You're account is succesfully registered!");
                        checker = true;
                    }
                    else if (info == "4")
                    {
                        Console.WriteLine("Please, enter your new password: ");
                        string newPassword = Console.ReadLine();
                        while (!checker)
                        {
                            if (newPassword.Length < 8 || newPassword.Length > 16)
                            {
                                Console.WriteLine("Invalid email address, try again.");
                                Console.WriteLine("Please, enter your email address: ");
                                newPassword = Console.ReadLine();
                            }
                            else
                            {
                                checker = true;
                            }
                        }
                        password = newPassword;
                        Console.WriteLine("You're account is succesfully registered!");
                        checker = true;
                    }
                    else if (info == "5")
                    {
                        Console.WriteLine("Please, enter your new phone number: ");
                        string newPhoneNumber = Console.ReadLine();
                        isPhoneValid = int.TryParse(newPhoneNumber, out phoneOutput) && newPhoneNumber.Length == 8;
                        while (!checker)
                        {
                            if (!isPhoneValid)
                            {
                                Console.WriteLine("Invalid email address, try again.");
                                Console.WriteLine("Please, enter your email address: ");
                                newPhoneNumber = Console.ReadLine();
                            }
                            else
                            {
                                checker = true;
                            }
                        }
                        phoneNumber = newPhoneNumber;
                        Console.WriteLine("You're account is succesfully registered!");
                        checker = true;
                    }
                    break;
            }
        AccountModel account = new AccountModel { FirstName = firstName, LastName = lastName, EmailAddress = email, Password = password, PhoneNumber = phoneNumber, IsAdmin = 0 };
        AccountsAccess.Write(account);
        }
    }
}
