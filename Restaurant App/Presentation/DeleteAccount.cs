using Project;
using Project.Presentation;
using Project.Logic;
using System.Linq;

namespace Project.Presentation
{
    internal class DeleteAccount
    {
        // Method to show the delete account menu
        public static void ShowDeleteAccountMenu()
        {
            while (true)
            {
                var allAccounts = Access.Users.Read(); // Re-fetch all accounts
                if (allAccounts == null || !allAccounts.Any())
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                // Sort accounts by first name alphabetically
                var sortedAccounts = allAccounts.OrderBy(acc => acc.FirstName).ToList();

                int currentPage = 0;
                int totalPages = (int)Math.Ceiling((double)sortedAccounts.Count / 10); // Accounts per page

                while (true)
                {
                    // Generate and display the menu options
                    var options = DeleteAccountLogic.GenerateMenuOptions(sortedAccounts, currentPage, totalPages);
                    var selection = SelectionPresent.Show(options, "ACCOUNTS\n\n");

                    string selectedText = selection.text;

                    // Handle user navigation and actions
                    if (selectedText == "Back")
                        return;

                    if (selectedText == "Next Page >>")
                    {
                        currentPage = Math.Min(currentPage + 1, totalPages - 1); // Avoid exceeding total pages
                        continue;
                    }

                    if (selectedText == "<< Previous Page")
                    {
                        currentPage = Math.Max(currentPage - 1, 0); // Avoid going below page 0
                        continue;
                    }

                    // Call the logic layer to delete an account
                    if (DeleteAccountLogic.DeleteAccount(currentPage, sortedAccounts))
                    {
                        Console.WriteLine("Account deleted successfully.");
                        Console.WriteLine("Press any key to refresh...");
                        Console.ReadKey(); // Pause for feedback

                        break; // Refresh list after deletion
                    }
                }
            }
        }
    }
}
