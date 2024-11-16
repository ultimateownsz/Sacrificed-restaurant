using DataAccess;

namespace Logic
{
    public class CalendarLogic
    {
        public List<string> GetAvailableTables(DateTime selectedDate)
        {
            return CalendarAccess.GetAvailableTables(selectedDate);
        }
    }
}
