
using System;
using Logic;


namespace Presentation
{
    public static class MakingReservations
    {
        private static CalendarLogic calendarLogic = new CalendarLogic();

        public static void DisplayCalendar(DateTime currentDate)
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
                Console.Write($"{day,2} ");
                if ((day + startDay) % 7 == 0) Console.WriteLine();
            }

            Console.WriteLine("\nPREVIOUS         NEXT");
            Console.WriteLine("Press P for Previous month, N for Next month, S to select a date, or Q to quit.");
        }

        public static void CalendarNavigation()
        {
            DateTime currentDate = DateTime.Now;
            bool running = true;

            while (running)
            {
                DisplayCalendar(currentDate);
                string input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "p":
                        currentDate = currentDate.AddMonths(-1);
                        break;
                    case "n":
                        currentDate = currentDate.AddMonths(1);
                        break;
                    case "s":
                        Console.Write("Enter day to select: ");
                        if (int.TryParse(Console.ReadLine(), out int day) && day >= 1 && day <= DateTime.DaysInMonth(currentDate.Year, currentDate.Month))
                        {
                            DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, day);
                            ShowAvailableTables(selectedDate);
                        }
                        else
                        {
                            Console.WriteLine("Invalid day.");
                        }
                        break;
                    case "q":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }
            }
        }

        public static void ShowAvailableTables(DateTime selectedDate)
        {
            var availableTables = calendarLogic.GetAvailableTables(selectedDate);
            Console.WriteLine($"Available tables for {selectedDate.ToString("MMMM dd, yyyy")}:");
            foreach (var table in availableTables)
            {
                Console.WriteLine($" - {table}");
            }
        }
    }
}
