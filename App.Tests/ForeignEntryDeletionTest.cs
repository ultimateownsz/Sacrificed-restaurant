using App.DataAccess.Utils;
using App.DataModels.Allergy;
using Restaurant;

namespace App.Tests;

[TestClass]
public class ForeignEntryDeletionTest
{

    [TestMethod]
    public void UserTree()
    {
        // create entries and verify existence
        Access.Users.Write(new UserModel() { ID = -1 });
        Assert.IsTrue(Access.Users.Read().Where(x => x.ID == -1).Count() == 1, "User couldn't be created");
        
        Access.Allerlinks.Write(new AllerlinkModel() { ID = -1, EntityID = -1});
        Assert.IsTrue(Access.Allerlinks.Read().Where(x => x.ID == -1).Count() == 1, "Allerlink couldn't be created");

        Access.Reservations.Write(new ReservationModel() { ID = -1, UserID = -1 });
        Assert.IsTrue(Access.Reservations.Read().Where(x => x.ID == -1).Count() == 1, "Reservation couldn't be created");

        // user should be deleted
        Assert.IsTrue(Access.Users.Delete(-1), "User couldn't be deleted");
        
        // all allerlinks of user should be deleted
        Assert.IsTrue(Access.Allerlinks.Read().Where(x => x.EntityID == -1 
            && x.Personal == 1).Count() == 0, "Allerlink still present after user deletion");

        // all reservations should be deleted
        Assert.IsTrue(Access.Reservations.Read().Where(x => x.UserID == -1)
            .Count() == 0, "Allerlink still present after user deletion");
    }

}
