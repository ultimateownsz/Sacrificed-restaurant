namespace Restaurant;
public static class LoginPresent
{
    private static SelectionPresent.Palette palette = new();

    private static string? _request_email(string? email)
    {
        Console.Clear();
        string prefix = "LOGIN MENU\n\n";
        prefix += $"E-mail: ";
        
        Console.ForegroundColor = palette.Base;
        return TerminableUtilsPresent.ReadLine(prefix, email ?? "", colour: Console.ForegroundColor);
    }

    private static string? request_password(string? email)
    {
        Console.Clear();
        string prefix = "LOGIN MENU\n\n";
        prefix += $"E-mail: {email}\nPassword: ";
        
        Console.ForegroundColor = palette.Base;
        return TerminableUtilsPresent.ReadLine(prefix, colour: Console.ForegroundColor);
    }

    public static UserModel? Show()
    {

        string? email = null;
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

        // ControlHelpPresent.ResetToDefault();
        ControlHelpPresent.ShowHelp();

        if (password == null)
        {
            ControlHelpPresent.ResetToDefault();
            userInput.Clear(); // Clear inputs on escape key
            return null;  // Escape key pressed
        }

        userInput.Add($"Password: {new string('*', password.Length)}"); // Mask password and store it
        
        ControlHelpPresent.ResetToDefault();
        userInput.Clear(); // Clear the stored inputs

        return password; // Valid password entered
    }


    // public static UserModel? Start()
    // {
    //     ControlHelpPresent.Clear();
    //     ControlHelpPresent.AddOptions("Exit", "<escape>");
    //     ControlHelpPresent.ShowHelp();

    //     // Request email
    //     string? email = request_email();
    //     if (string.IsNullOrEmpty(email)) return null;
    //     // Request password
    //     string? password = request_password(email);
    //     if (string.IsNullOrEmpty(password)) return null;

    //     // Check credentials
    //     UserModel? acc = UserLogic.CheckLogin(email.ToLower(), password);
    //     string? loginMessage = acc!.Admin.HasValue == true && acc.Admin.Value > 0 ? $"Logged in as admin ({acc.FirstName})." : $"Logged in as {acc.FirstName}.";
    //     if (acc != null)
    //     {
    //         ControlHelpPresent.DisplayFeedback(loginMessage, "bottom", "success");
    //         return acc; // Successful login
    //     }
    //     else
    //     {
    //         // Invalid credentials
    //         Console.Clear();
    //         ControlHelpPresent.Clear();
    //         ControlHelpPresent.AddOptions("Exit", "<escape>");
    //         ControlHelpPresent.ShowHelp();
    //         ControlHelpPresent.ResetToDefault();

    //         Console.ForegroundColor = ConsoleColor.Red;
    //         // Display all stored inputs for user reference
    //         foreach (var input in userInput)
    //         {
    //             Console.WriteLine(input);
    //         }

    //         Console.ResetColor();
    //         ControlHelpPresent.DisplayFeedback("Invalid credentials, returning...");
    //         return null;
    //     }

    // }

    public static UserModel? Start()
    {
        ControlHelpPresent.Clear();
        ControlHelpPresent.AddOptions("Exit", "<escape>");
        ControlHelpPresent.ShowHelp();

        // Request email
        string? email = request_email();
        if (string.IsNullOrEmpty(email)) return null;

        // Request password
        string? password = request_password(email);
        if (string.IsNullOrEmpty(password)) return null;

        // Check credentials
        UserModel? acc = LoginLogic.CheckLogin(email.ToLower(), password);
        if (acc == null)
        {
            ControlHelpPresent.DisplayFeedback("Invalid credentials, returning...");
            return null;
        }

        string loginMessage = acc.Admin.HasValue && acc.Admin.Value > 0 
            ? $"Logged in as admin ({acc.FirstName})." 
            : $"Logged in as {acc.FirstName}.";

        ControlHelpPresent.DisplayFeedback(loginMessage, "bottom", "success");
        return acc; // Successful login
    }

}
