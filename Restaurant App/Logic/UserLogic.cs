using Project.Presentation;
using Project;

public class UserLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    public UserLogic()
    {
        
    }

    public static UserModel? GetById(int id)
    {
        return Access.Users.GetBy<int>("ID", id);
    }

    public static UserModel? CheckLogin(string? email, string? password)
    {
        UserModel acc = Access.Users.GetBy<string>("Email", email);

        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;

            // tough to segment, badly designed.
            if (acc.Admin == 1)
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
        foreach (var mail in Access.Users.Read().Select(o => o.Email))
        {
            if (mail == email)
                return false;
        }
         
        return email.Contains("@") && email.Contains(".com");
    }

    public static bool IsPasswordValid(string password)
    {
        return password.Length <= 16 && password.Length >= 8;
    }

    public static bool IsPhoneNumberValid(string phoneNumber)
    {
        return int.TryParse(phoneNumber, out _) && phoneNumber.Length == 10;
    }

    public static UserModel UserAccount(string firstName, string lastName, string email, string password, string phoneNumber)
    {
        return new UserModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Phone = phoneNumber,
            Admin = 0
        };
    }

    public static UserModel AdminAccount(string firstName, string lastName, string email, string password, string phoneNumber)
    {
        return new UserModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password,
            Phone = phoneNumber,
            Admin = 1
        };
    }
}
