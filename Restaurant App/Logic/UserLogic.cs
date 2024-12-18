using Project.Presentation;
using Project;
using System.Text.RegularExpressions;

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

    public static (bool isValid, string? message) IsEmailValid(string email)
    {
        // Query the database to check for email existence
        var existingUser = Access.Users.GetBy<string>("Email", email);

        if (existingUser != null)
        {
            return (false, "This email already exists. Please use a different email.");
        }

        // Validate email format using regex
        string emailPattern = @"^[\w\.\-+]+@[a-zA-Z0-9\-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(email, emailPattern))
        {
            return (false, "The email format is invalid. Please enter a valid email address (e.g., example@domain.com).");
        }

        return (true, null); // Email is valid
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

    public static bool DeleteUserAccount(int userId)
    {
        var user = Access.Users.GetBy<int>("ID", userId);
        if (user == null)
        {
            return false; // Account not found
        }

        // Proceed to delete the user or admin account
        bool success = Access.Users.Delete(userId);
        return success;
    }
}
