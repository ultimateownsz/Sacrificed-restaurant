using App.DataAccess.Utils;
using App.DataModels.Allergy;
using App.DataModels.Utils;
using App.Tests.Utils;
using Restaurant;

namespace App.Tests;

//[TestClass]
public class ForeignEntryDeletionTest
{
    private bool _create_tree(IModel root, List<IModel> branches)
    {
        foreach (var branch in branches)
        {
            var bacces = new DataTest<IModel>(branch);
            var type = branch.GetType();
            branch.ID = -1;

            bacces.Write(branch);
            if (!bacces.Read().Contains(branch))
                return false;
        }

        root.ID = -1;
        var access = new DataTest<IModel>(root);

        access.Write(root);
        if (!access.Read().Contains(root))
            return false;

        return true;
    }

    [TestMethod]
    public void User()
    {
        var branches = new List<IModel>()
        {
            new ReservationModel(),
            new AllerlinkModel(),
        };

        Assert.IsTrue(_create_tree(new UserModel(), branches));
    }

}
