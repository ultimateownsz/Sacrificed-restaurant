public class AccountModel
{

    public Int64 UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public Int64 PhoneNumber { get; set; }
    public int IsAdmin { get; set; }

    public AccountModel() {}

    public AccountModel(Int64 userID, string firstName, string lastName, string email, string password, Int64 phoneNumber, int isAdmin)
    {
        UserID = userID;
        EmailAddress = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        IsAdmin = isAdmin;
    }


}



