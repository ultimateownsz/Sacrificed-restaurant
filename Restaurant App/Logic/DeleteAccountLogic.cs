using Project;
using System.Linq;

namespace Project.Logic
{
    public class DeleteAccountLogic
    {
        private const int AccountsPerPage = 10;

        // Method to fetch a specific page of accounts
        public static List<UserModel> GetPage(List<UserModel> accounts, int page, int itemsPerPage)
        {
            return accounts.Skip(page * itemsPerPage).Take(itemsPerPage).ToList();
        }

        // Method to format account display text
        public static string FormatAccount(UserModel account)
        {
            return $"{account.FirstName} {account.LastName} ({(account.Admin == 1 ? "Admin" : "User")})";
        }

        // Method to handle account deletion confirmation and deletion
        public static bool ConfirmAndDelete(UserModel account)
        {
            // Use SelectionPresent for confirmation
            var options = new List<string> { "Yes", "No" };
            var selection = SelectionPresent.Show(
                options,
                $"Are you sure?\n\n"
            );

            if (selection.text == "Yes")
            {
                // Mark the account as inactive
                account.FirstName = "Inactive";
                
                // Get the current number of inactive accounts for unique identifier
                int inactiveCount = Access.Users.GetAllBy<string>("FirstName", "Inactive").Count();
                account.LastName = $"#{inactiveCount + 1}";

                // Remove sensitive data
                account.Email = "Inactive";
                account.Password = "Removed";
                account.Phone = "Inactive";

                // Update the account in the database
                return Access.Users.Update(account);
            }

            return false;
        }

        private static bool MarkAsInactive(UserModel account)
        {
            // Find other accounts with "Inactive" as the first name
            var inactiveAccounts = Access.Users.GetAllBy<string>("FirstName", "Inactive");
            int nextNumber = inactiveAccounts.Count() + 1;

            // Update the account's first and last names
            account.FirstName = "Inactive";
            account.LastName = $"#{nextNumber}";

            // Perform the update
            return Access.Users.Update(account);
        }

        public static List<UserModel> GetActiveAccounts()
        {
            var allAccounts = Access.Users.Read(); // Fetch all accounts
            return allAccounts.Where(account => account.FirstName != "Inactive").ToList();
        }

        // Method to generate menu options based on the current page and total pages
        public static List<string> GenerateMenuOptions(List<UserModel> accounts, int currentPage, int totalPages)
        {
            var options = accounts.Select(FormatAccount).ToList();
            if (currentPage > 0) options.Add("<< Previous Page");
            if (currentPage < totalPages - 1) options.Add("Next Page >>");
            options.Add("Back");
            return options;
        }

        // Method to delete an account and refresh the list
        public static bool DeleteAccount(int currentPage, List<UserModel> sortedAccounts)
        {
            var accountsToDisplay = GetPage(sortedAccounts, currentPage, AccountsPerPage);
            var options = GenerateMenuOptions(accountsToDisplay, currentPage, (int)Math.Ceiling((double)sortedAccounts.Count / AccountsPerPage));

            // If a valid account is selected, delete it
            var selectedAccount = accountsToDisplay.FirstOrDefault(acc => FormatAccount(acc) == options.First());
            if (selectedAccount?.ID != null && ConfirmAndDelete(selectedAccount))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Failed to delete account.");
                return false;
            }
        }
    }
}
