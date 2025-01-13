using App.DataAccess.Utils;
using Restaurant;

namespace App.DataAccess;
public class ScheduleAccess : DataAccess<ScheduleModel>
{
    public ScheduleAccess() : base(typeof(ScheduleModel).GetProperties().Select(p => p.Name).ToArray()) { }

}
