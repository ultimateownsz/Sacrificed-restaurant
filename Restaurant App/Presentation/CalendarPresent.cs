using System;

namespace Presentation
{
    public class CalendarPresent
    {
        public static DateTime Show(DateTime initialDate, bool isAdmin)
        {
            DateTime currentDate = initialDate;
            DateTime today = DateTime.Today; // Current date for comparison
            int selectedDay = Math.Max(currentDate.Day, today.Day); // Ensure the selected day is not in the past
            bool running = true;

            while (running)
            {
                DisplayCalendar(currentDate, selectedDay, today, isAdmin);

                var key = Console.ReadKey(intercept: true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (selectedDay > 1 && IsDaySelectable(currentDate, selectedDay - 1, today, isAdmin))
                            selectedDay--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (selectedDay < DateTime.DaysInMonth(currentDate.Year, currentDate.Month) &&
                            IsDaySelectable(currentDate, selectedDay + 1, today, isAdmin))
                            selectedDay++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedDay - 7 > 0 && IsDaySelectable(currentDate, selectedDay - 7, today, isAdmin))
                            selectedDay -= 7;
                        else if (selectedDay - 7 <= 0 && IsDaySelectable(currentDate, 1, today, isAdmin))
                            selectedDay = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedDay + 7 <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month) &&
                            IsDaySelectable(currentDate, selectedDay + 7, today, isAdmin))
                            selectedDay += 7;
                        else if (selectedDay + 7 > DateTime.DaysInMonth(currentDate.Year, currentDate.Month) &&
                                 IsDaySelectable(currentDate, DateTime.DaysInMonth(currentDate.Year, currentDate.Month), today, isAdmin))
                            selectedDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                        break;
                    case ConsoleKey.P: // Previous month
                        if (isAdmin || currentDate.AddMonths(-1) >= today)
                        {
                            currentDate = currentDate.AddMonths(-1);
                            selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                        }
                        break;
                    case ConsoleKey.N: // Next month
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                        break;
                    case ConsoleKey.Enter: // Select date
                        if (isAdmin || IsDaySelectable(currentDate, selectedDay, today, isAdmin))
                            return new DateTime(currentDate.Year, currentDate.Month, selectedDay);
                        break;
                    case ConsoleKey.Q: // Quit
                        throw new OperationCanceledException("User canceled calendar navigation.");
                    default:
                        Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                        break;
                }
            }

            throw new InvalidOperationException("Calendar navigation exited unexpectedly.");
        }

        private static void DisplayCalendar(DateTime currentDate, int selectedDay, DateTime today, bool isAdmin)
        {
            Console.Clear();
            Console.WriteLine(currentDate.ToString("MMMM yyyy").ToUpper());
            Console.WriteLine("Mo Tu We Th Fr Sa Su");

            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;

            // Adjust for 0-based index for Monday start
            startDay = startDay == 0 ? 6 : startDay - 1;

            for (int i = 0; i < startDay; i++)
                Console.Write("   "); // Padding for the first week

            for (int day = 1; day <= daysInMonth; day++)
            {
                if (!isAdmin && currentDate.Year == today.Year && currentDate.Month == today.Month && day < today.Day)
                {
                    // Mark past dates as gray for non-admins
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{day,2} ");
                    Console.ResetColor();
                }
                else if (day == selectedDay)
                {
                    // Highlight the currently selected day
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{day,2} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"{day,2} ");
                }

                if ((day + startDay) % 7 == 0) Console.WriteLine();
            }

            Console.WriteLine("\nUse Arrow Keys to Navigate, Enter to Select Date, P for Previous Month, N for Next Month, Q to Quit.");
        }

        private static bool IsDaySelectable(DateTime currentDate, int day, DateTime today, bool isAdmin)
        {
            if (isAdmin) return true; // Admins can select any day

            DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, day);
            return selectedDate >= today; // Only allow selection of today or future dates for non-admins
        }
    }
}
