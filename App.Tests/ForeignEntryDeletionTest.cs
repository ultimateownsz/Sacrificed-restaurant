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
        var reservation = new ReservationModel()
        {
            ID = -1,
            UserID = -1,
        };

        var allerlink = new AllerlinkModel()
        {
            ID = -1,
            EntityID = -1,
        };

        var user = new UserModel()
        {
            ID = -1,
        };

    }

}
