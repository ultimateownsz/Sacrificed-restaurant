using System;
using System.Linq;

namespace Project
{
    public class CalendarPresent
    {
        public static DateTime Show(DateTime initialDate, bool isAdmin, int guests)
        {
            DateTime currentDate = initialDate;

            // Default to the current day if selectable, otherwise find the first available day
            int selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);

            // Immediately display the calendar
            DisplayCalendar(currentDate, selectedDay, isAdmin, guests);

            bool running = true;
            while (running)
            {
                var key = Console.ReadKey(intercept: true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, guests, -1);
                        break;
                    case ConsoleKey.RightArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, guests, 1);
                        break;
                    case ConsoleKey.UpArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, guests, -7);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedDay = NavigateToAvailableDay(currentDate, selectedDay, isAdmin, guests, 7);
                        break;
                    case ConsoleKey.P: // Previous month
                        currentDate = currentDate.AddMonths(-1);
                        selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);
                        break;
                    case ConsoleKey.N: // Next month
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);
                        break;
                    case ConsoleKey.Enter: // Select date
                        return new DateTime(currentDate.Year, currentDate.Month, selectedDay);
                    case ConsoleKey.Q: // Quit
                        throw new OperationCanceledException("User canceled calendar navigation.");
                    default:
                        Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                        break;
                }

                // Update calendar display after any key press
                DisplayCalendar(currentDate, selectedDay, isAdmin, guests);
            }

            throw new InvalidOperationException("Calendar navigation exited unexpectedly.");
        }

        private static void DisplayCalendar(DateTime currentDate, int selectedDay, bool isAdmin, int guests)
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

                // Check if the day has no available tables for the guest count
                bool hasNoAvailableTables = !HasAvailableTablesForGuests(dateToCheck, guests);

                // Apply dark gray for past days
                if (isPast)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                // Apply light gray for busy/unavailable days
                else if (hasNoAvailableTables)
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
                    // Reset to the default console color
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

        private static int FindFirstAvailableDay(DateTime currentDate, bool isAdmin, int guests)
        {
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dateToCheck = new DateTime(currentDate.Year, currentDate.Month, day);
                if (IsDaySelectable(dateToCheck, isAdmin, guests))
                {
                    return day;
                }
            }

            throw new InvalidOperationException("No available days in the current month.");
        }


        private static int NavigateToAvailableDay(DateTime currentDate, int startDay, bool isAdmin, int guests, int direction)
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
                if (IsDaySelectable(dateToCheck, isAdmin, guests))
                {
                    return day; // Found a valid day
                }
            }
        }

        private static bool IsDaySelectable(DateTime dateToCheck, bool isAdmin, int guests)
        {
            DateTime today = DateTime.Today;

            // Non-admin users cannot select past days
            if (!isAdmin && dateToCheck < today)
            {
                return false;
            }

            // Check if there are available tables for the selected guest count on this day
            return HasAvailableTablesForGuests(dateToCheck, guests);
        }


        private static bool HasAvailableTablesForGuests(DateTime date, int guests)
        {
            // Get the tables suitable for the given guest count
            var availableTables = guests switch
            {
                1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                5 or 6 => new int[] { 2, 3 },
                _ => Array.Empty<int>()
            };

            // Fetch all active tables
            var activeTables = Access.Places.Read()
                .Where(p => p.Active == 1)
                .Select(p => p.ID.Value)
                .ToHashSet();

            // Get all reserved tables for the specified date
            var reservedTables = Access.Reservations
                .GetAllBy<DateTime>("Date", date)
                .Where(r => r?.PlaceID != null)
                .Select(r => r!.PlaceID!.Value)
                .ToHashSet();

            // Check if any tables are both active and available for the given guest count
            return availableTables
                .Intersect(activeTables)
                .Except(reservedTables)
                .Any();
        }

    }
}
