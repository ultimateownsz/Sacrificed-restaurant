using System;
using Project;

static class UserLogin
{
    private static UserLogic userLogic = new();

    private static List<string> userInput = new(); // List to store the email and password
    
    private static string? request_email()
    {
        ControlsHelperPresent.Clear();
        ControlsHelperPresent.AddOptions("Exit", "<escape>");

        string? email = null;

        // Pass the prompt directly to GetValidatedInput
        TryCatchHelper.EscapeKeyException(() =>
        {
            email = InputHelper.GetValidatedInput<string?>(
                "Email: ", // Pass the prompt here
                input => InputHelper.InputNotNull(input, "Email cannot be empty."),
                menuTitle: "LOGIN",
                showHelpAction: () => ControlsHelperPresent.ShowHelp()
            );
        });

        if (email == null) return null; // Escape key pressed

        userInput.Add($"Email: {email}"); // Store the email in the list

        return email; // Valid email entered
    }


    private static string? request_password(string? email)
    {
        // ControlsHelperPresent.Clear();
        ControlsHelperPresent.AddOptions("Exit", "<escape>");
        
        // Display the email above the password prompt
        Console.SetCursorPosition(0, 0);
        foreach (var input in userInput)
        {
            Console.WriteLine(input);
        }

        string? password = null;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.ResetColor();

        TryCatchHelper.EscapeKeyException(() =>
        {
            password = InputHelper.GetValidatedInput<string?>(
                "Password: ",
                input => InputHelper.InputNotNull(input, "Password cannot be empty."),
                menuTitle: "LOGIN",
                showHelpAction: () =>
                {
                    // Display the email above the password prompt
                    // Console.SetCursorPosition(0, 0);
                    foreach (var input in userInput)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(input);
                    }
                    ControlsHelperPresent.ShowHelp();
                }
            );
        });

        if (password == null) return null;  // Escape key pressed

        userInput.Add($"Password: {new string('*', password.Length)}"); // Mask password and store it
        
        ControlsHelperPresent.ResetToDefault();
        userInput.Clear(); // Clear the stored inputs

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
            ControlsHelperPresent.Clear();
            ControlsHelperPresent.AddOptions("Exit", "<escape>");
            ControlsHelperPresent.ShowHelp();
            ControlsHelperPresent.ResetToDefault();

            Console.ForegroundColor = ConsoleColor.Red;
            // Display all stored inputs for user reference
            foreach (var input in userInput)
            {
                Console.WriteLine(input);
            }

            Console.ResetColor();
            ControlsHelperPresent.DisplayFeedback("\nInvalid credentials, returning...");
            return null;
        }

    }
}
