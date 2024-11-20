using System;
using Logic;

namespace Presentation
{
    public static class MakingReservations
    {
        private static CalendarLogic calendarLogic = new CalendarLogic();

        public static void DisplayCalendar(DateTime currentDate, int selectedDay)
        {
            Console.Clear();
            Console.WriteLine(currentDate.ToString("MMMM yyyy").ToUpper());

            // Display calendar days layout
            Console.WriteLine("Mo Tu We Th Fr Sa Su");
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;

            // Adjust for 0-based index for Monday start
            startDay = startDay == 0 ? 6 : startDay - 1;

            // Print spaces for the first week
            for (int i = 0; i < startDay; i++)
                Console.Write("   ");

            for (int day = 1; day <= daysInMonth; day++)
            {
                if (day == selectedDay)
                {
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

            Console.WriteLine("\n\nUse Arrow Keys to Navigate, Enter to Select Date, P for Previous Month, N for Next Month, Q to Quit.");
        }

        public static void CalendarNavigation()
        {
            DateTime currentDate = DateTime.Now;
            int selectedDay = currentDate.Day;
            bool running = true;

            while (running)
            {
                DisplayCalendar(currentDate, selectedDay);
                var key = Console.ReadKey(intercept: true);

                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (selectedDay > 1) selectedDay--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (selectedDay < DateTime.DaysInMonth(currentDate.Year, currentDate.Month)) selectedDay++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (selectedDay - 7 > 0)
                            selectedDay -= 7;
                        else
                            selectedDay = 1; // Jump to start if moving up goes out of bounds
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedDay + 7 <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
                            selectedDay += 7;
                        else
                            selectedDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month); // Jump to end if moving down goes out of bounds
                        break;
                    case ConsoleKey.P: // Previous month
                        currentDate = currentDate.AddMonths(-1);
                        selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)); // Adjust selected day if new month has fewer days
                        break;
                    case ConsoleKey.N: // Next month
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = Math.Min(selectedDay, DateTime.DaysInMonth(currentDate.Year, currentDate.Month)); // Adjust selected day if new month has fewer days
                        break;
                    case ConsoleKey.Enter: // Select date
                        DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, selectedDay);
                        ShowAvailableTables(selectedDate);
                        break;
                    case ConsoleKey.Q: // Quit
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                        break;
                }
            }
        }

        public static void ShowAvailableTables(DateTime selectedDate)
        {
            var availableTables = calendarLogic.GetAvailableTables(selectedDate);
            Console.WriteLine($"\nAvailable tables for {selectedDate.ToString("MMMM dd, yyyy")}:");
            foreach (var table in availableTables)
            {
                Console.WriteLine($" - {table}");
            }
            Console.WriteLine("\nPress any key to return to the calendar...");
            Console.ReadKey();
        }
    }
}












































































































































































































































































































































































































































































































































































































































