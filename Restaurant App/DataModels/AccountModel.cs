public class AccountModel
{
    public Int64 UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public Int64 PhoneNumber { get; set; }
    public Int64 IsAdmin { get; set; }


    public AccountModel(Int64 userID, string firstName, string lastName, string email, string password, Int64 phoneNumber, Int64 isAdmin)
    {
        UserID = userID;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = email;
        Password = password;
        PhoneNumber = phoneNumber;
        IsAdmin = isAdmin;
    }
}
