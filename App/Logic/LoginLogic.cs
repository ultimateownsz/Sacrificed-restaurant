using App.DataAccess.Utils;
using System.Text.RegularExpressions;

namespace Restaurant;
public class LoginLogic
{
    public static UserModel? CurrentAccount { get; private set; }
    public LoginLogic()
    {
        
    }
    public static UserModel? GetById(int id)
    {
        return Access.Users.GetBy<int>("ID", id);
    }

    public static (UserModel? user, string? message) CheckLogin(string? email, string? password)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return (null, "Email and password cannot be empty. Please try again.");

        // Fetch user from database
        UserModel? acc = Access.Users.GetBy<string>("Email", email.ToLower());

        if (acc == null) return (null, "User not found, double check your email or register a new account. Please try again");

        // Check password
        if (acc.Password != password)  return (null, "Incorrect password. Please try again.");

        // Login successful
        CurrentAccount = acc;
        string role = acc.Admin == 1 ? "Admin" : "User";
        return (acc, $"Logged in as {role}");
    }

    public static (bool isValid, string? message) IsEmailValid(string email)
    {
        // Query the database to check for email existence
        var existingUser = Access.Users.GetBy<string>("Email", email);

        if (existingUser != null) return (false, "This email already exists. Please use a different email.");

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
        if (string.IsNullOrWhiteSpace(password)) return (false, "Password cannot be empty.");

        if (password.Length < 8 || password.Length > 16) return (false, "Password must be between 8 and 16 characters long.");

        if (!password.Any(char.IsLetter)) return (false, "Password must contain at least one letter.");

        if (!password.Any(char.IsDigit)) return (false, "Password must contain at least one number.");

        return (true, null); // Password is valid
    }

    public static (bool isValid, string? error) IsPhoneNumberValid(string phoneNumber)
    {
        if (phoneNumber.Length != 10) return (false, "Phone number must be exactly 10 digits long.");
        
        if (!phoneNumber.StartsWith("06")) return (false, "Phone number must start with '06'.");
        
        if (!phoneNumber.All(char.IsDigit)) return (false, "Phone number must contain only digits.");
       
        // if (phoneNumber == "0612345678") return (false, "The phone number '0612345678' is reserved for special purposes and cannot be used. Please enter a different phone number.");
        
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