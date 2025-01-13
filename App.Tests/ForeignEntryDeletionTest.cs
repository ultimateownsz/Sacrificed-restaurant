using App.DataModels.Allergy;
using App.DataModels.Utils;
using Restaurant;

namespace App.Tests;

[TestClass]
internal class ForeignEntryDeletionTest
{
    // 
    private static Dictionary<IModel, List<IModel>> _linkage
        = new() { {UserModel, new () { AllerlinkModel, ReservationModel } };

}
