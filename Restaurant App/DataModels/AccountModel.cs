public class AccountModel
{

    public Int64 Id { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int PhoneNumber { get; set; }
    public bool IsAdmin { get; set; }

    public AccountModel(Int64 id, string email, string password, string firstName, string lastName, int phoneNumber, bool isAdmin)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        IsAdmin = isAdmin;
    }


}



