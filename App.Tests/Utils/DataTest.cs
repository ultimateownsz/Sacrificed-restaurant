using App.DataAccess.Utils;
using App.DataModels.Utils;

namespace App.Tests.Utils;

internal class DataTest<T> : DataAccess<T> where T : IModel
{
    internal DataTest(T model) :
        base(model.GetType().GetProperties().Select(p => p.Name).ToArray())
    { }
}
