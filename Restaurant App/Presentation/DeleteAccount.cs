using Project;
using Project.Presentation;
using Project.Logic;
using System.Linq;

namespace Project.Presentation
{
    internal class DeleteAccount
    {
        public static void ShowDeleteAccountMenu(UserModel currentUser)
        {
            try
            {
                var activeAccounts = DeleteAccountLogic.GetActiveAccounts()?
                .Where(acc => acc.ID != currentUser.ID) // exclude current user
                .OrderBy(acc => acc.FirstName)          // sort by first name
                .ToList();

                if (activeAccounts == null || !activeAccounts.Any())
                {
                    ControlHelpPresent.DisplayFeedback("No accounts available to delete.");
                    Console.WriteLine("\nPress any key to return to the previous menu...");
                    Console.ReadKey();
                    return;
                }

                int currentPage = 0;
                bool running = true;
                do
                {
                    Console.Clear();

                    ControlHelpPresent.Clear();
                    ControlHelpPresent.ResetToDefault();
                    ControlHelpPresent.ShowHelp();
                    // prepare options for the current page
                    var options = DeleteAccountLogic.GenerateMenuOptions(activeAccounts, currentPage);

                    // display menu and handle user input
                    dynamic selection = SelectionPresent.Show(options, "ACCOUNTS MENU\n\n");

                    if (selection.text == null || selection.text == "Back")
                    {
                        Console.WriteLine(" Exiting ACCOUNT MENU...");
                        // Thread.Sleep(1500);
                        return;
                    }

                    switch (selection.text)
                    {
                        case "Back":
                            running = false;
                            break;
                        
                        case "Next page >>":
                            currentPage++;
                            break;

                        case "<< Previous page":
                            currentPage--;
                            break;
                        
                        default:
                            // attempt to delete the selected account
                            bool success = DeleteAccountLogic.DeleteAccount(currentUser, selection.text, activeAccounts, currentPage);
                            if (success)
                            {
                                Console.WriteLine("\nAccount successfully deleted. Press any key to refresh...");
                                Console.ReadKey();
                                activeAccounts = DeleteAccountLogic.GetActiveAccounts()
                                    .Where(acc => acc.ID != currentUser.ID)
                                    .OrderBy(acc => acc.FirstName)
                                    .ToList();
                                currentPage = 0; // Reset to the first page after deletion
                            }
                            else
                            {
                                ControlHelpPresent.DisplayFeedback("\nFailed to delete the account. Press any key to continue...");
                                Console.ReadKey();
                            }
                            break;
                    }

                } while (running);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine(" Exiting ACCOUNT MENU...");
                // Thread.Sleep(1500);
            }
        }
    }
}
