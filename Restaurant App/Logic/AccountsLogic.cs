using Project.Presentation;

public class AccountsLogic
{
    public static AccountModel? CurrentAccount { get; private set; }
    public AccountsLogic()
    {
        
    }

    public static AccountModel GetById(int id)
    {
        return AccountsAccess.GetById(id);
    }

    public static AccountModel? CheckLogin(string email, string password)
    {
        AccountModel acc = AccountsAccess.GetByEmail(email);

        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;

            // tough to segment, badly designed.
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

    public static bool IsEmailValid(string email)
    {
        return email.Contains("@") && email.Contains(".com");
    }

    public static bool IsPasswordValid(string password)
    {
        return password.Length <= 16 && password.Length >= 8;
    }

    public static bool IsPhoneNumberValid(string phoneNumber)
    {
        return int.TryParse(phoneNumber, out _) && phoneNumber.Length == 8;
    }

    public static AccountModel UserAccount(string firstName, string lastName, string email, string password, string phoneNumber)
    {
        return new AccountModel
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = email,
            Password = password,
            PhoneNumber = Convert.ToInt32(phoneNumber),
            IsAdmin = 0
        };
    }

    public static AccountModel AdminAccount(string firstName, string lastName, string email, string password, string phoneNumber)
    {
        return new AccountModel
        {
            FirstName = firstName,
            LastName = lastName,
            EmailAddress = email,
            Password = password,
            PhoneNumber = Convert.ToInt32(phoneNumber),
            IsAdmin = 1
        };
    }
}
