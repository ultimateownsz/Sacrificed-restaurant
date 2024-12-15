using System;
using Project;

static class UserLogin
{
    private static UserLogic userLogic = new();
    
    private static string? request_email()
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");

        string? email = null;

        // Pass the prompt directly to GetValidatedInput
        TryCatchHelper.EscapeKeyException(() =>
        {
            email = InputHelper.GetValidatedInput<string?>(
                "Email: ", // Pass the prompt here
                input => InputHelper.InputNotNull(input, "Email cannot be empty.")
            );
        });

        if (email == null) return null; // Escape key pressed

        return email; // Valid email entered
    }


    private static string? request_password(string? email)
    {
        Console.Clear();
        Console.WriteLine("LOGIN\n");

        Console.WriteLine($"Email: {email}");  // Keep the email visible above the password prompt

        Console.ForegroundColor = ConsoleColor.Yellow;
        
        // Use TryCatchHelper to handle Escape key exceptions during password input
        string? password = null;

        TryCatchHelper.EscapeKeyException(() =>
        {
            password = InputHelper.GetValidatedInput<string?>(
                "Password: ",
                input => InputHelper.InputNotNull(input, "Password cannot be empty.")  // Validation failure
            );
        });

        if (password == null) return null;  // Escape key pressed

        return password; // Valid password entered
    }


    public static UserModel? Start()
    {
        // Request email
        string? email = request_email();
        if (email == null) return null; // Escape key pressed during email input

        // Request password
        string? password = request_password(email);
        if (password == null) return null; // Escape key pressed during password input

        // Check credentials
        UserModel? acc = UserLogic.CheckLogin(email.ToLower(), password);
        if (acc != null)
        {
            return acc; // Successful login
        }
        else
        {
            // Invalid credentials
            Console.Clear();
            Console.WriteLine("LOGIN\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Email: {email}");
            Console.WriteLine($"Password: {password}");

            Console.ResetColor();
            Console.WriteLine("\nInvalid credentials, returning...");
            return null;
        }

    }
}
