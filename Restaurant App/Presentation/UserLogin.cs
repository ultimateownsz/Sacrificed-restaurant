using System;
using Project;

static class UserLogin
{
    private static string? request_email()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("LOGIN\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Email: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;

            // Wrap the input handling logic with EscapeKeyException
            string? email = null;
            TryCatchHelper.EscapeKeyException(() =>
            {
                email = InputHelper.GetValidatedInput<string?>(
                    "",
                    input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (null, "Email cannot be empty.");
                        return (input, null); // Valid input
                    });
            });

            if (email == null) // Escape key pressed
            {
                return null;
            }

            return email; // Valid email entered
        }
    }


    private static string? request_password(string? email)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("LOGIN\n");
            Console.WriteLine($"Email: {email}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Password: ", Console.ForegroundColor);
            Console.ForegroundColor = ConsoleColor.White;

            // Use TryCatchHelper to handle Escape key exceptions during password input
            string? password = null;

            TryCatchHelper.EscapeKeyException(() =>
            {
                password = InputHelper.GetValidatedInput<string?>(
                    "",
                    input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                            return (null, "Password cannot be empty."); // Validation failure
                        return (input, null); // Valid input
                    });
            });

            if (password == null) // Escape key pressed
            {
                return null;
            }

            return password; // Valid password entered
        }
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

        // Invalid credentials
        Console.Clear();
        Console.WriteLine("LOGIN\n");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Email: {email}", Console.ForegroundColor);
        Console.WriteLine($"Password: {password}", Console.ForegroundColor);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nInvalid credentials, returning...");
        return null;
    }
}
