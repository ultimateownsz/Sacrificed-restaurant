using Project;
using Project.Presentation;
using Project.Logic;
using System.Linq;

namespace Project.Presentation
{
    internal class DeleteAccount
    {
        // Method to show the delete account menu
        public static void ShowDeleteAccountMenu(UserModel currentUser)
        {
            while (true)
            {
                var activeAccounts = DeleteAccountLogic.GetActiveAccounts(); // Get active accounts
                if (activeAccounts == null || !activeAccounts.Any())
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                // Exclude the current logged-in account
                var accountsToDisplay = activeAccounts.Where(acc => acc.ID != currentUser.ID).ToList();

                // Sort accounts by first name alphabetically
                var sortedAccounts = accountsToDisplay.OrderBy(acc => acc.FirstName).ToList();

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
                    if (DeleteAccountLogic.DeleteAccount(currentUser, currentPage, sortedAccounts, selectedText))
                    {
                        Console.WriteLine("Press any key to refresh...");
                        Console.ReadKey();
                        break; // Refresh list after deletion
                    }
                }
            }
        }
    }
}
