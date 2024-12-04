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
            Console.WriteLine($"\nAre you sure you want to delete {account.FirstName} {account.LastName}? (y/n)");
            if (Console.ReadKey(true).Key == ConsoleKey.Y)
            {
                return UserLogic.DeleteUserAccount(account.ID.Value);
            }
            return false;
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
                Console.WriteLine("Account deleted successfully.");
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
