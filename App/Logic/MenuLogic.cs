using App.Presentation.Admin;
using App.Presentation.User;

namespace Restaurant;
internal class MenuLogic
{

    public static string? Login()
    {
        UserModel? acc = LoginPresent.Start();
        if (acc != null)
        {
            if (acc.Admin == 1)
            {
                AdminMenuPresent.AdminStart(acc);  // directs to Admin menu if the account is an admin
                return "continue";
            }
            else
            {
                UserMenuPresent.ShowUserMenu(acc);  // directs to User menu if the account is a regular user
                return "continue";
            }
        }
        else
            return "continue";

    }

}