using App.DataModels.Allergy;
using App.DataModels.Utils;
using Restaurant;

namespace App.Tests;

//[TestClass] (commented out for periodic-merge)
internal class ForeignEntryDeletionTest
{

    private static Dictionary<IModel, List<IModel>> _linkage
        = new() { 
            { new UserModel(), new() { new AllerlinkModel(), new ReservationModel() } } 
        };

}
