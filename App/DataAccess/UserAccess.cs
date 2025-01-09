using App.DataAccess.Utils;

namespace Restaurant;
internal class UserAccess: DataAccess<UserModel>
{
    internal UserAccess(): base(typeof(UserModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
