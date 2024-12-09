using System;
using System.Linq;

namespace Project
{
    public class CalendarPresent
    {
        public static DateTime Show(DateTime initialDate, bool isAdmin)
        {
            DateTime currentDate = initialDate;
            int selectedDay = currentDate.Day;
            bool running = true;

            while (running)
            {
                DisplayCalendar(currentDate, selectedDay, isAdmin);
                var key = Console.ReadKey(intercept: true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, direction: -1);
                        break;
                    case ConsoleKey.RightArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, direction: 1);
                        break;
                    case ConsoleKey.UpArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, direction: -7);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, direction: 7);
                        break;
                    case ConsoleKey.P: // Previous month
                        currentDate = currentDate.AddMonths(-1);
                        selectedDay = NavigateToAvailableDay(currentDate, 1, isAdmin, direction: 1); // Start at the first available day
                        break;
                    case ConsoleKey.N: // Next month
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = NavigateToAvailableDay(currentDate, 1, isAdmin, direction: 1); // Start at the first available day
                        break;
                    case ConsoleKey.Enter: // Select date
                        return new DateTime(currentDate.Year, currentDate.Month, selectedDay);
                    case ConsoleKey.Q: // Quit
                        throw new OperationCanceledException("User canceled calendar navigation.");
                    default:
                        Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                        break;
                }
            }
            throw new InvalidOperationException("Calendar navigation exited unexpectedly.");
        }


        private static int NavigateToAvailableDay(DateTime currentDate, int startDay, bool isAdmin, int direction)
        {
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int day = startDay;

            // Loop to find the next available day
            while (true)
            {
                day += direction;

                // Wrap around to the next/previous month if out of bounds
                if (day < 1 || day > daysInMonth)
                {
                    return startDay; // Redirect to the current day if no valid day is found
                }

                // Check if the current date is selectable
                DateTime dateToCheck = new DateTime(currentDate.Year, currentDate.Month, day);
                if (IsDaySelectable(dateToCheck, isAdmin))
                {
                    return day; // Found a valid day
                    
                }
            }
        }


        private static bool IsDaySelectable(DateTime dateToCheck, bool isAdmin)
        {
            DateTime today = DateTime.Today;

            // Non-admin users cannot select past days
            if (!isAdmin && dateToCheck < today)
            {
                return false;
            }

            // Check if there are available tables on this day
            return HasAvailableTables(dateToCheck);
        }

        private static void DisplayCalendar(DateTime currentDate, int selectedDay, bool isAdmin)
        {
            Console.Clear();
            Console.WriteLine(currentDate.ToString("MMMM yyyy").ToUpper());

            // Display calendar days layout
            Console.WriteLine("Mo Tu We Th Fr Sa Su");
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;

            // Adjust for 0-based index for Monday start
            startDay = startDay == 0 ? 6 : startDay - 1;

            // Get today for comparison
            DateTime today = DateTime.Today;

            // Print spaces for the first week
            for (int i = 0; i < startDay; i++)
                Console.Write("   ");

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dateToCheck = new DateTime(currentDate.Year, currentDate.Month, day);

                // Check if the day is in the past (only for non-admin users)
                bool isPast = !isAdmin && dateToCheck < today;

                // Check if the day has no available tables
                bool hasNoAvailableTables = !HasAvailableTables(dateToCheck);

                // Apply gray for past days or fully reserved days
                if (isPast || hasNoAvailableTables)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (day == selectedDay)
                {
                    // Highlight the selected day
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ResetColor();
                }

                // Print the day
                Console.Write($"{day,2} ");

                // Reset color after printing
                Console.ResetColor();

                if ((day + startDay) % 7 == 0) Console.WriteLine();
            }

            Console.WriteLine("\nUse Arrow Keys to Navigate, Enter to Select Date, P for Previous Month, N for Next Month, Q to Quit.");
        }


        private static bool HasAvailableTables(DateTime date)
        {
            var allTables = Enumerable.Range(1, 15); // Assuming 15 tables
            var reservedTables = Access.Reservations
                                    .GetAllBy<DateTime>("Date", date)
                                    .Where(r => r?.PlaceID != null)
                                    .Select(r => r!.PlaceID!.Value)
                                    .ToHashSet();

            return allTables.Except(reservedTables).Any(); // Returns true if any tables are still available
        }

        private static bool IsDayUnavailable(DateTime currentDate, int day, bool isAdmin)
        {
            DateTime targetDate = new DateTime(currentDate.Year, currentDate.Month, day);
            bool isInPast = !isAdmin && targetDate < DateTime.Now.Date;
            bool hasNoAvailableTables = !HasAvailableTables(targetDate);

            return isInPast || hasNoAvailableTables;
        }
    }
}
