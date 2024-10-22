public class AccountsLogic
{
    public static AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        
    }

    public AccountModel GetById(int id)
    {
        return AccountsAccess.GetById(id);
    }

    public AccountModel? CheckLogin(string email, string password)
    {
        AccountModel acc = AccountsAccess.GetByEmail(email);

        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;

            if (acc.IsAdmin == 1)
            {
                Console.WriteLine("Ingelogd als admin.");
            }
            else
            {
                Console.WriteLine("Ingelogd als normale gebruiker.");
            }

            return acc;
        }

        return null;
    }
    
}
