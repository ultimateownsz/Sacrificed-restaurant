public class AccountsLogic
{
    public static AccountModel? CurrentAccount { get; private set; }
    public int NewUser;

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

    public void CreateAccount()
    {
        Console.WriteLine("Please, enter your first name: ");
        string firstName = Console.ReadLine();
        Console.WriteLine("Please, enter your last name: ");
        string lastName = Console.ReadLine();
        Console.WriteLine("Please, enter your email address: ");
        string email = Console.ReadLine();
        Console.WriteLine("Please, enter your password: ");
        string password = Console.ReadLine();
        Console.WriteLine("Please, enter your phone number: ");
        int phoneNumber = Convert.ToInt32(Console.ReadLine());

        NewUser++;

        AccountModel newUser = new AccountModel(NewUser, firstName, lastName, email, password, phoneNumber, 0);
        AccountsAccess.Write(newUser);
    }
    
}
