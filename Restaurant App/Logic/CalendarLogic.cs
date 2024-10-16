public class CalendarLogic
{
    public int CalendarId { get; set; }
    public int Year { get; set; }
    public MenuLogic MenuId { get; set; }
    public int Month { get; set; }

    public CalendarLogic(int calendarId, int year, int month, MenuLogic menuId)
    {
        CalendarId = calendarId;
        Year = year;
        Month = month;
        MenuId = menuId;
    }
}