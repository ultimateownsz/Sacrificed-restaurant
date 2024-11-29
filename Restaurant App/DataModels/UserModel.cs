namespace Project;
public class UserModel: IModel
{
    public int? ID { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public int? Admin { get; set; }

    public UserModel() { }

    public UserModel(string? firstName, string? lastName, 
        string? email, string? phone, string? password, int? admin, int? id = null)
    {
        ID = id;
        Phone = phone;
        Admin = admin;
        Email = email;
        LastName = lastName;
        Password = password;
        FirstName = firstName;
    }
}
