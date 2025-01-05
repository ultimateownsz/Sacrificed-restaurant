using System;
using Project;

static class UserLogin
{
    private static UserLogic userLogic = new();

    private static List<string> userInput = new(); // List to store the email and password
    
    // Method to request email from the user
    private static string? request_email()
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();

        string? email = null;

        // Pass the prompt directly to GetValidatedInput
        TryCatchHelper.EscapeKeyException(() =>
        {
            email = InputHelper.GetValidatedInput<string?>(
                "Email: ", // Pass the prompt here
                input => InputHelper.InputNotNull(input, "Email cannot be empty."),
                menuTitle: "LOGIN",
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );
        });

        // Reset the navigation controls to default when escape key is pressed
        ControlHelpPresent.Clear();
        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        if (email == null)
        {
            userInput.Clear(); // Clear inputs on escape key
            return null; // Escape key pressed
        }

        userInput.Add($"Email: {email}"); // Store the email in the list

        return email; // Valid email entered
    }


    private static string? request_password(string? email)
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();
        
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
                    ControlHelpPresent.ShowHelp();
                }
            );
        });

        ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        if (password == null)
        {
            userInput.Clear(); // Clear inputs on escape key
            return null;  // Escape key pressed
        }

        userInput.Add($"Password: {new string('*', password.Length)}"); // Mask password and store it
        
        ControlHelpPresent.ResetToDefault();
        userInput.Clear(); // Clear the stored inputs

        return password; // Valid password entered
    }


    public static UserModel? Start()
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();

        // Request email
        string? email = request_email();
        if (email == null)
        {
            ControlHelpPresent.DisplayFeedback("Exiting...", "bottom", "error");
            return null; // Escape key pressed during email input
        }
        // Request password
        string? password = request_password(email);
        if (password == null)
        {
            ControlHelpPresent.DisplayFeedback("Exiting...", "bottom", "error");
            return null; // Escape key pressed during password input
        }

        // Check credentials
        UserModel? acc = UserLogic.CheckLogin(email.ToLower(), password);
        string? loginMessage = acc!.Admin.HasValue && acc.Admin.Value > 0 ? $"Logged in as admin ({acc.FirstName})." : $"Logged in as {acc.FirstName}.";
        if (acc != null)
        {
            ControlHelpPresent.DisplayFeedback(loginMessage, "bottom", "success");
            return acc; // Successful login
        }
        else
        {
            // Invalid credentials
            Console.Clear();
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Exit", "<escape>");
            ControlHelpPresent.ShowHelp();
            ControlHelpPresent.ResetToDefault();

            Console.ForegroundColor = ConsoleColor.Red;
            // Display all stored inputs for user reference
            foreach (var input in userInput)
            {
                Console.WriteLine(input);
            }

            Console.ResetColor();
            ControlHelpPresent.DisplayFeedback("Invalid credentials, returning...");
            return null;
        }

    }
}
