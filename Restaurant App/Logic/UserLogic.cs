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



    public static (bool isValid, string? error) IsPasswordValid(string password)
    {
            if (string.IsNullOrWhiteSpace(password))
        {
            return (false, "Password cannot be empty.");
        }

        if (password.Length < 8 || password.Length > 16)
        {
            return (false, "Password must be between 8 and 16 characters long.");
        }

        if (!password.Any(char.IsLetter))
        {
            return (false, "Password must contain at least one letter.");
        }

        if (!password.Any(char.IsDigit))
        {
            return (false, "Password must contain at least one number.");
        }
        return (true, null); // Password is valid
    }

    public static (bool isValid, string? error) IsPhoneNumberValid(string phoneNumber)
    {
        if (phoneNumber.Length != 10)
        {
            return (false, "Phone number must be exactly 10 digits long.");
        }
        if (!phoneNumber.StartsWith("06"))
        {
            return (false, "Phone number must start with '06'.");
        }
        if (!phoneNumber.All(char.IsDigit))
        {
            return (false, "Phone number must contain only digits.");
        }
        // if (phoneNumber == "0612345678") // Example of an excluded number
        // {
        //     return (false, "The phone number '0612345678' is reserved for special purposes and cannot be used. Please enter a different phone number.");

        // }
        return (true, null); // Valid phone number
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
