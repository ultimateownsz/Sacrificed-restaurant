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
                    ControlHelpPresent.DisplayFeedback("Press any key to return to the previous menu...", "bottom", "tip");
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
                    var selection = SelectionPresent.Show(options, banner:"ACCOUNTS MENU\n\n").ElementAt(0).text;

                    if (selection == null || selection == "Back")
                    {
                        Console.WriteLine(" Exiting ACCOUNT MENU...");
                        // Thread.Sleep(1500);
                        return;
                    }

                    switch (selection)
                    {
                        case "Back":
                            running = false;
                            break;
                        
                        case "Next page":
                            currentPage++;
                            break;

                        case "Previous page":
                            currentPage--;
                            break;
                        
                        default:
                            // attempt to delete the selected account
                            bool success = DeleteAccountLogic.DeleteAccount(currentUser, selection, activeAccounts, currentPage);
                            if (success)
                            {
                                ControlHelpPresent.DisplayFeedback("Account successfully deleted. Press any key to refresh...", "bottom", "success");
                                Console.ReadKey();
                                activeAccounts = DeleteAccountLogic.GetActiveAccounts()
                                    .Where(acc => acc.ID != currentUser.ID)
                                    .OrderBy(acc => acc.FirstName)
                                    .ToList();
                                currentPage = 0; // Reset to the first page after deletion
                            }
                            else
                            {
                                ControlHelpPresent.DisplayFeedback("Failed to delete the account. Press any key to continue...");
                                Console.ReadKey();
                            }
                            break;
                    }

                } while (running);
            }
            catch (OperationCanceledException)
            {
                // Console.WriteLine(" Exiting ACCOUNT MENU...");
                ControlHelpPresent.DisplayFeedback("Exciting ACCOUNT MENU");
                // Thread.Sleep(1500);
            }
        }
    }
}
