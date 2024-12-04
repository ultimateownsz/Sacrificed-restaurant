using Project;
using Project.Presentation;
using System.Linq; // Ensure LINQ is available

internal class DeleteAccount
{
    public static void ShowDeleteAccountMenu()
    {
        const int AccountsPerPage = 10;

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
            int totalPages = (int)Math.Ceiling((double)sortedAccounts.Count / AccountsPerPage);

            Console.Clear();
            Console.WriteLine($"DELETE ACCOUNTS - Page {currentPage + 1}/{totalPages}\n");

            // Fetch accounts for the current page
            var accountsToDisplay = sortedAccounts
                .Skip(currentPage * AccountsPerPage)
                .Take(AccountsPerPage)
                .ToList();

            // Create a list of display options for SelectionPresent
            var options = accountsToDisplay.Select(acc =>
                $"{acc.FirstName} {acc.LastName} ({(acc.Admin == 1 ? "Admin" : "User")})"
            ).ToList();

            // Add navigation options
            if (currentPage > 0) options.Add("<< Previous Page");
            if (currentPage < totalPages - 1) options.Add("Next Page >>");
            options.Add("Back"); // Changed "Cancel" to "Back"

            // Use SelectionPresent to navigate
            var selection = SelectionPresent.Show(options, "Select an account to delete:\n");
            string selectedText = selection.text;

            if (selectedText == "Back") // Changed from "Cancel" to "Back"
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

            // If a valid account is selected, find the account and delete it
            var selectedAccount = accountsToDisplay.FirstOrDefault(acc =>
                $"{acc.FirstName} {acc.LastName} ({(acc.Admin == 1 ? "Admin" : "User")})" == selectedText);

            if (selectedAccount?.ID != null) // Ensure selectedAccount and ID are not null
            {
                Console.WriteLine($"\nAre you sure you want to delete {selectedAccount.FirstName} {selectedAccount.LastName}? (y/n)");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    // Use the value of ID with .Value
                    bool success = UserLogic.DeleteUserAccount(selectedAccount.ID.Value);
                    Console.WriteLine(success ? "Account deleted." : "Failed to delete account.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(); // Pause to show feedback

                    if (success)
                        continue; // Refresh the list by re-fetching accounts
                }
            }
            else
            {
                Console.WriteLine("Account ID is invalid or null. Cannot delete this account.");
                Console.ReadKey(); // Pause for invalid account
            }
        }
    }
}
