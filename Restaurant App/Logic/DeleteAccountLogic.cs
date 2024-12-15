using Project;
using System.Linq;

namespace Project.Logic
{
    public class DeleteAccountLogic
    {
        private const int AccountsPerPage = 10;

        // Method to fetch a specific page of accounts
        public static List<UserModel> GetPage(List<UserModel> accounts, int page)
        {
            return accounts.Skip(page * AccountsPerPage).Take(AccountsPerPage).ToList();
        }

        // Method to format account display text
        public static string FormatAccount(UserModel account)
        {
            return $"{account.FirstName} {account.LastName} ({(account.Admin == 1 ? "Admin" : "User")})";
        }

        // Method to generate menu options based on the current page and total pages
        public static List<string> GenerateMenuOptions(List<UserModel> accounts, int currentPage)
        {
            var options = GetPage(accounts, currentPage).Select(FormatAccount).ToList();
            if (currentPage > 0) options.Add("<< Previous Page");
            if ((currentPage + 1) * AccountsPerPage < accounts.Count) options.Add("Next Page >>");
            options.Add("Back");
            return options;
        }

        // Method to handle account deletion confirmation and deletion
        public static bool ConfirmAndDelete(UserModel account)
        {
            var options = new List<string> { "Yes", "No" };
            var selection = SelectionPresent.Show(
                options,
                $"Are you sure you want to delete {account.FirstName} {account.LastName}?\n\n"
            );

            if (selection.text == "Yes")
            {
                // Mark the account as inactive
                account.FirstName = "Inactive";
                
                // Get the current number of inactive accounts for unique identifier
                int inactiveCount = Access.Users.GetAllBy<string>("FirstName", "Inactive").Count();
                account.LastName = $"#{inactiveCount + 1}";

                // Replace data
                account.Email = "Inactive";
                account.Password = "Inactive";
                account.Phone = "Inactive";

                // Update the account in the database
                if (Access.Users.Update(account))
                {
                    // After deleting the account, delete future reservations
                    DeleteFutureReservations(account.ID);

                    return true;
                }
            }

            return false;
        }

        // Method to delete future reservations
        public static void DeleteFutureReservations(int? userId)
        {
            // Check if userId is not null
            if (userId.HasValue)
            {
                int id = userId.Value;  // Get the value of the nullable int

                // Fetch all reservations for the user
                var allReservations = Access.Reservations.Read();

                // Get today's date (ignores the time part)
                var currentDate = DateTime.Now.Date;  // This will set the time to 00:00:00

                // Filter for future reservations, including today
                var futureReservations = allReservations
                    .Where(res => res.UserID == id && res.Date.HasValue && res.Date.Value.Date >= currentDate)  // Include today as part of the future
                    .ToList();

                // Delete all future reservations
                foreach (var reservation in futureReservations)
                {
                    Access.Reservations.Delete(reservation.ID);
                }

                Console.WriteLine($"{futureReservations.Count} future reservations deleted.");
            }
            else
            {
                Console.WriteLine("User ID is null. Cannot delete future reservations.");
            }
        }
        public static List<UserModel> GetActiveAccounts()
        {
            var allAccounts = Access.Users.Read(); // Fetch all accounts
            return allAccounts.Where(account => account.FirstName != "Inactive").ToList();
        }

        // Method to delete an account and refresh the list
        public static bool DeleteAccount(UserModel currentUser, string selectedText, List<UserModel> sortedAccounts, int currentPage)
        {
            var accountsToDisplay = GetPage(sortedAccounts, currentPage);

            // Find the account that matches the selected text
            var selectedAccount = accountsToDisplay.FirstOrDefault(acc => FormatAccount(acc) == selectedText);

            if (selectedAccount != null && selectedAccount.ID != currentUser.ID) // Ensure you don't delete your own account
            {
                // Call the confirmation method
                if (ConfirmAndDelete(selectedAccount))
                {
                    return true;
                }
            }

            Console.WriteLine("Failed to delete account.");
            return false;
        }
    }
}
