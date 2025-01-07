using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Project.Logic;

namespace Project.Presentation
{
    public static class CreateAdminOptions
    {
        public static void Options(UserModel acc)
        {
            List<string> adminOptions = new()
            {
                "Create a new admin account",
                "Make an existing user an admin",
                "Back"
            };

            string choice = SelectionPresent.Show(adminOptions, banner: "Create (admin account)").ElementAt(0).text;

            switch (choice)
            {
                case "Create a new admin account":
                    RegisterUser.CreateAccount(true);
                    break;

                case "Make an existing user an admin":
                    PromoteUserToAdmin();
                    break;

                case "Back":
                    return;
            }
        }

        private static void PromoteUserToAdmin()
        {
            while (true)
            {
                var activeAccounts = DeleteAccountLogic.GetActiveAccounts();
                if (activeAccounts == null || !activeAccounts.Any())
                {
                    Console.WriteLine("No user accounts found.");
                    Console.ReadKey();
                    return;
                }

                // Only show users, no admins
                var nonAdminAccounts = activeAccounts.Where(acc => acc.Admin == 0).ToList();
                var sortedAccounts = nonAdminAccounts.OrderBy(acc => acc.FirstName).ToList();

                int currentPage = 0;
                int totalPages = (int)Math.Ceiling((double)sortedAccounts.Count / 10); // Accounts per page

                while (true)
                {
                    // Menu options
                    var options = DeleteAccountLogic.GenerateMenuOptions(sortedAccounts, currentPage, totalPages);
                    options.Add("Search User by Email (Press 's')");

                    var selection = SelectionPresent.Show(options, banner: "PROMOTE USER TO ADMIN").ElementAt(0);

                    string selectedText = selection.text;

                    // Handle navigation and actions
                    if (selectedText == "Back")
                    return;

                    if (selectedText == "Next Page >>")
                    {
                        currentPage = Math.Min(currentPage + 1, totalPages - 1);
                        continue;
                    }

                    if (selectedText == "<< Previous Page")
                    {
                        currentPage = Math.Max(currentPage - 1, 0);
                        continue;
                    }

                    if (selectedText == "Search User by Email (Press 's')")
                    {
                        Console.Clear();
                        Console.Write("Enter the user's email: ");
                        string email = Console.ReadLine()?.Trim();

                        if (!string.IsNullOrEmpty(email))
                        {
                            var searchResults = nonAdminAccounts.Where(acc =>
                                string.Equals(acc.Email, email, StringComparison.OrdinalIgnoreCase)).ToList();

                            if (!searchResults.Any())
                            {
                                Console.WriteLine("No users found with that email.");
                                Console.ReadKey();
                                continue;
                            }

                            sortedAccounts = searchResults.OrderBy(acc => acc.FirstName).ToList();
                            currentPage = 0;
                            totalPages = (int)Math.Ceiling((double)sortedAccounts.Count / 10);
                        }
                        else
                        {
                            Console.WriteLine("Email is required.");
                            Console.ReadKey();
                        }

                        continue;
                    }

                    // hanndle user selection for promotion
                    var accountsToDisplay = DeleteAccountLogic.GetPage(sortedAccounts, currentPage, 10);
                    var selectedAccount = accountsToDisplay.FirstOrDefault(acc => DeleteAccountLogic.FormatAccount(acc) == selectedText);

                    if (selectedAccount != null)
                    {
                        Console.Clear();
                        var confirmationOptions = new List<string> { "Yes", "No" };
                        string confirmation = SelectionPresent.Show(confirmationOptions, banner: $"Are you sure?").ElementAt(0).text;

                        if (confirmation == "Yes")
                        {
                            selectedAccount.Admin = 1;
                            if (Access.Users.Update(selectedAccount))
                            {
                                Console.WriteLine("User successfully promoted to admin.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to promote the user. Try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Action canceled. User was not promoted.");
                        }

                        Console.WriteLine("Press any key to return to the menu...");
                        Console.ReadKey();
                        return;
                    }
                }
            }
            // Console.Clear();
            // Console.WriteLine("Enter the first and last name of the user you want to promote to admin.");
            // Console.WriteLine("");

            // Console.Write("First Name: ");
            // string firstName = Console.ReadLine()?.Trim();
            // Console.Write("Last Name: ");
            // string lastName = Console.ReadLine()?.Trim();

            // // Validate inputs
            // if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            // {
            //     Console.WriteLine("\nBoth first and last names are required. Promoting user canceled.");
            //     Console.WriteLine("Press any key to return to the menu...");
            //     Console.ReadKey();
            //     return;
            // }
            
            // // Create an instance fpr UserModel
            // var userAccess = new DataAccess<UserModel>(new[] { "ID", "FirstName", "LastName", "Email", "Password", "Phone", "Admin" });

            // // Fetch user bases on the input
            // var user = userAccess.GetAllBy("FirstName", firstName)
            //     .FirstOrDefault(u =>
            //         string.Equals(u.LastName, lastName, StringComparison.OrdinalIgnoreCase) &&
            //         u.Admin == 0); // Only fetch users
            
            // if (user == null)
            // {
            //     Console.WriteLine("\nUser not found.");
            //     Console.WriteLine("Press any key to return to the menu...");
            //     Console.ReadKey();
            //     return;
            // }

            // // Confirmation for promoting
            // Console.Clear();
            // var confirmationOptions = new List<string> { "Yes", "No" };
            // string confirmation = SelectionPresent.Show(confirmationOptions, banner: "Are you sure?").ElementAt(0).text;

            // if (confirmation == "Yes")
            // {
            //     // Update the user's Admin status
            //     user.Admin = 1;
            //     if (userAccess.Update(user))
            //     {
            //         Console.WriteLine("User successfully promoted to admin.");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Failed to promote the user. Try again.");
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("Action canceled. User was not promoted.");
            // }

            // Console.WriteLine("Press any key to return to the menu...");
            // Console.ReadKey();
        }
    }
}
