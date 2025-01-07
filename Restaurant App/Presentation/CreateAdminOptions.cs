// using System.Reflection.Metadata;

// namespace Project.Presentation
// {
//     public static class CreateAdminOptions
//     {
//         public static void Options(UserModel acc)
//         {
//             List<string> adminOptions = new()
//             {
//                 "Create a new admin account",
//                 "Make an existing user an admin",
//                 "Back"
//             };

//             string choice = SelectionPresent.Show(adminOptions, banner: "Create (admin account)").ElementAt(0).text;

//             switch (choice)
//             {
//                 case "Create a new admin account":
//                     RegisterUser.CreateAccount(true);
//                     break;

//                 case "Make an existing user an admin":
//                     PromoteUserToAdmin();
//                     break;

//                 case "Back":
//                     return;
//             }
//         }

//         private static void PromoteUserToAdmin()
//         {
//             Console.Clear();
//             Console.WriteLine("Enter the first and last name of the user you want to promote to admin.");
//             Console.WriteLine("");

//             Console.Write("First Name: ");
//             string firstName = Console.ReadLine()?.Trim();
//             Console.Write("Last Name: ");
//             string lastName = Console.ReadLine()?.Trim();

//             // Validate inputs
//             if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
//             {
//                 Console.WriteLine("\nBoth first and last names are required. Promoting user canceled.");
//                 Console.WriteLine("Press any key to return to the menu...");
//                 Console.ReadKey();
//                 return;
//             }
            
//             // Create an instance fpr UserModel
//             var userAccess = new DataAccess<UserModel>(new[] { "ID", "FirstName", "LastName", "Email", "Password", "Phone", "Admin" });

//             // Fetch user bases on the input
//             var user = userAccess.GetAllBy("FirstName", firstName)
//                 .FirstOrDefault(u =>
//                     string.Equals(u.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
//                     u.Admin == 0); // Only fetch users
            
//             if (user == null)
//             {
//                 Console.WriteLine("\nUser not found.");
//                 Console.WriteLine("Press any key to return to the menu...");
//                 Console.ReadKey();
//                 return;
//             }

//             // Confirmation for promoting
//             Console.Clear();
//             var confirmationOptions = new List<string> { "Yes", "No" };
//             string confirmation = SelectionPresent.Show(confirmationOptions, banner: "Are you sure?").ElementAt(0).text;

//             if (confirmation == "Yes")
//             {
//                 // Update the user's Admin status
//                 user.Admin = 1;
//                 if (userAccess.Update(user))
//                 {
//                     Console.WriteLine("User successfully promoted to admin.");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Failed to promote the user. Try again.");
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("Action canceled. User was not promoted.");
//             }

//             Console.WriteLine("Press any key to return to the menu...");
//             Console.ReadKey();
//         }
//     }
// }
