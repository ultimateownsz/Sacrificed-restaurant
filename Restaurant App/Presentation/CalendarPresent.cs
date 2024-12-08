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
                        if (selectedDay > 1 && !IsDayUnavailable(currentDate, selectedDay - 1, isAdmin))
                            selectedDay--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (selectedDay < DateTime.DaysInMonth(currentDate.Year, currentDate.Month) &&
                            !IsDayUnavailable(currentDate, selectedDay + 1, isAdmin))
                            selectedDay++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedDay - 7 > 0 && !IsDayUnavailable(currentDate, selectedDay - 7, isAdmin))
                            selectedDay -= 7;
                        else
                            selectedDay = 1; // Jump to start if moving up goes out of bounds
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedDay + 7 <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month) &&
                            !IsDayUnavailable(currentDate, selectedDay + 7, isAdmin))
                            selectedDay += 7;
                        else
                            selectedDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month); // Jump to end if moving down goes out of bounds
                        break;
                    case ConsoleKey.P: // Previous month
                        currentDate = currentDate.AddMonths(-1);
                        selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                        break;
                    case ConsoleKey.N: // Next month
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                        break;
                    case ConsoleKey.Enter: // Select date
                        if (!IsDayUnavailable(currentDate, selectedDay, isAdmin))
                            return new DateTime(currentDate.Year, currentDate.Month, selectedDay); // Return the selected date
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

            return allTables.Except(reservedTables).Any();
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
