namespace Restaurant;
static class LoginPresent
{
    private static LoginLogic loginLogic = new();
    private static List<string> userInput = new(); // List to store the email and password
    
    // one private method to call instead of repeated code
    private static void ManualControls()
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();
    }

    private static void SelectionControls()
    {
        Console.Clear();
        ControlHelpPresent.ResetToDefault();
    }

    private static void ResetControlsOnEscape(bool isEscapePressed)
    {
        if (isEscapePressed)
        {
            ControlHelpPresent.ResetToDefault();
            userInput.Clear(); // Clear inputs on escape key
        }
    }

    // Method to request email from the user
    private static string? RequestEmail()
    {
        ManualControls();

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
        ManualControls();

        if (email == null)
        {
            SelectionControls();
            userInput.Clear(); // Clear inputs on escape key
            return null; // Escape key pressed
        }

        userInput.Add($"Email: {email}"); // Store the email in the list

        return email; // Valid email entered
    }


    private static string? RequestPassword(string? email)
    {
        ManualControls();
        
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

        ControlHelpPresent.ShowHelp();

        if (password == null)
        {
            ResetControlsOnEscape(true);
            return null;  // Escape key pressed
        }

        userInput.Add($"Password: {new string('*', password.Length)}"); // Mask password and store it
        
        ControlHelpPresent.ResetToDefault();
        userInput.Clear(); // Clear the stored inputs

        return password; // Valid password entered
    }

    public static UserModel? Show()
    {
        while (true)
        {
            ManualControls();

            // Request email
            string? email = RequestEmail();
            if (string.IsNullOrEmpty(email)) return null; // Exit if email is empty

            // Request password
            string? password = RequestPassword(email);
            if (string.IsNullOrEmpty(password)) return null; // Exit if password is empty

            // Check credentials
            var (acc, message) = LoginLogic.CheckLogin(email.ToLower(), password);
            
            if (acc == null)
            {
                ControlHelpPresent.DisplayFeedback($"{message}", "bottom", "tip");
                Console.ReadKey(true);
                continue;
            }
            string loginMessage = acc!.Admin == 1
                ? $"Logged in as admin ({acc.FirstName})."
                : $"Logged in as {acc.FirstName}.";

            ControlHelpPresent.DisplayFeedback(loginMessage, "bottom", "success");
            
            return acc;
        }
    }
}