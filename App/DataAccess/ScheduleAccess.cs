using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
internal class ScheduleAccess : DataAccess<ScheduleModel>
{
    internal ScheduleAccess() : base(typeof(ScheduleModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
