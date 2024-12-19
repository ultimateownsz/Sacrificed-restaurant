using Project.Logic;

namespace Project.Presentation
{
    internal class DeleteAccount
    {
        public static void ShowDeleteAccountMenu(UserModel currentUser)
        {
            while (true)
            {
                var activeAccounts = DeleteAccountLogic.GetActiveAccounts();
                if (activeAccounts == null || !activeAccounts.Any())
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                // Exclude the current logged-in account
                var accountsToDisplay = activeAccounts
                    .Where(acc => acc.ID != currentUser.ID)
                    .OrderBy(acc => acc.FirstName)
                    .ToList();

                // Use PaginationHelper to display accounts with arrow key navigation
                var selectedText = PaginationHelper.PaginateWithArrowKeys(
                    accountsToDisplay,
                    10, // Items per page
                    acc => $"{acc.FirstName} {acc.LastName} ({(acc.Admin == 1 ? "Admin" : "User")})"
                );

                if (selectedText == null) // User chose "Back"
                    return;

                // Find the selected account
                var selectedAccount = accountsToDisplay.FirstOrDefault(acc =>
                    $"{acc.FirstName} {acc.LastName} ({(acc.Admin == 1 ? "Admin" : "User")})" == selectedText);

                if (selectedAccount != null)
                {
                    // Confirm and delete the selected account
                    if (DeleteAccountLogic.ConfirmAndDelete(selectedAccount))
                    {
                        Console.WriteLine("Press any key to refresh...");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
