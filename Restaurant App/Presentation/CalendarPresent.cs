using System;
using System.Linq;
using Presentation;

namespace Project
{
    public class CalendarPresent
    {
        public static DateTime Show(DateTime initialDate, bool isAdmin, int guests, UserModel acc)
        {
            DateTime currentDate = initialDate;

            int selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);
            // Console.Clear(); // Clear any lingering output before rendering the calendar
            // DisplayCalendar(currentDate, selectedDay, isAdmin, guests); // Render calendar


            while (true)
            {
                DisplayCalendar(currentDate, selectedDay, isAdmin, guests);

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
                    case ConsoleKey.P:
                        if (currentDate.AddMonths(-1) < DateTime.Today)
                        {
                            Console.SetCursorPosition(0, Console.CursorTop + 2);
                            Console.WriteLine("You cannot reserve in the past.");
                        }
                        else
                        {
                            currentDate = currentDate.AddMonths(-1);
                            selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);
                        }
                        break;
                    case ConsoleKey.N:
                        currentDate = currentDate.AddMonths(1);
                        selectedDay = FindFirstAvailableDay(currentDate, isAdmin, guests);
                        break;
                    case ConsoleKey.Enter:
                        DateTime selectedDate = new DateTime(currentDate.Year, currentDate.Month, selectedDay);

                        if (IsDayFullyBooked(selectedDate, guests))
                        {
                            Console.SetCursorPosition(0, Console.CursorTop + 2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This day is fully reserved.");
                            Console.ResetColor();
                        }
                        if (NoThemeForMonth(selectedDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This month has no theme.");
                            Console.ResetColor();
                        }
                        if (NoProductsInTheme(selectedDate))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("This month has no products.");
                            Console.ResetColor();
                        }
                        else
                        {
                            return selectedDate;
                        }
                        break;
                    case ConsoleKey.B:
                        return DateTime.MinValue; // Go back
                    default:
                        Console.WriteLine("Invalid input. Use Arrow Keys to navigate, Enter to select.");
                        break;
                }
            }
        }

        private static void DisplayCalendar(DateTime currentDate, int selectedDay, bool isAdmin, int guests)
        {
            Console.Clear();
            Console.WriteLine(currentDate.ToString("MMMM yyyy").ToUpper());
            Console.WriteLine("Mo Tu We Th Fr Sa Su");
            Console.ResetColor();

            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            int startDay = (int)new DateTime(currentDate.Year, currentDate.Month, 1).DayOfWeek;
            startDay = startDay == 0 ? 6 : startDay - 1;

            DateTime today = DateTime.Today;
            bool showFullyReservedMessage = false;
            bool noThemeReservedMessage = false;
            bool noProductsReservedMessage = false;

            for (int i = 0; i < startDay; i++)
                Console.Write("   ");

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime dateToCheck = new DateTime(currentDate.Year, currentDate.Month, day);

                bool isPast = !isAdmin && dateToCheck < today;
                bool isFullyBooked = IsDayFullyBooked(dateToCheck, guests);
                bool NoTheme= NoThemeForMonth(dateToCheck);
                bool NoProducts= NoProductsInTheme(dateToCheck);

                if (NoTheme)
                {
                    noThemeReservedMessage = true;
                }
                else if (NoProducts)
                {
                    noProductsReservedMessage = true;
                }

                if (isPast || noThemeReservedMessage || noProductsReservedMessage)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else if (day == selectedDay)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    // Flag to show the "fully reserved" message if the selected day is fully booked
                    if (isFullyBooked)
                    {
                        showFullyReservedMessage = true;
                    }
                }
                else if (isFullyBooked)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write($"{day,2} ");
                Console.ResetColor();

                if ((day + startDay) % 7 == 0) Console.WriteLine();
            }

            Console.ResetColor();
            if (noThemeReservedMessage)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nThis month has no theme.");
            }
            else if (noProductsReservedMessage)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nThis month has no products.");
            }

            Console.ResetColor();
            Console.WriteLine("\n\nnext month : <n>\nprev month : <p>\nnavigate   : <arrows>\nselect     : <enter>\nback       : <b>");

            // Display the "fully reserved" message at the bottom
            if (showFullyReservedMessage)
            {
                Console.WriteLine("\nThis day is fully reserved.");
            }
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

            while (true)
            {
                day += direction;

                if (day < 1 || day > daysInMonth)
                {
                    return startDay;
                }

                DateTime dateToCheck = new DateTime(currentDate.Year, currentDate.Month, day);
                if (IsDaySelectable(dateToCheck, isAdmin, guests))
                {
                    return day;
                }
            }
        }

        private static bool IsDaySelectable(DateTime dateToCheck, bool isAdmin, int guests)
        {
            DateTime today = DateTime.Today;

            if (!isAdmin && dateToCheck < today)
            {
                return false;
            }

            return HasAvailableTablesForGuests(dateToCheck, guests) || IsDayFullyBooked(dateToCheck, guests);
        }

        private static bool IsDayFullyBooked(DateTime date, int guests)
        {
            return !HasAvailableTablesForGuests(date, guests);
        }

        private static bool NoThemeForMonth(DateTime date)
        {
            if(ReservationMenuLogic.GetCurrentTheme(date) == null) return true;
            return false;
        }

        private static bool NoProductsInTheme(DateTime date)
        {
            ThemeModel? theme = ReservationMenuLogic.GetCurrentTheme(date);

            if(theme is null) return true;
            if(!ProductManager.AnyProductsInTheme(theme.ID)) return true;

            return false;
        }

        private static bool HasAvailableTablesForGuests(DateTime date, int guests)
        {
            var availableTables = guests switch
            {
                1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                5 or 6 => new int[] { 2, 3 },
                _ => Array.Empty<int>()
            };

            var activeTables = Access.Places.Read()
                .Where(p => p.Active == 1)
                .Select(p => p.ID.Value)
                .ToHashSet();

            var reservedTables = Access.Reservations
                .GetAllBy<DateTime>("Date", date)
                .Where(r => r?.PlaceID != null)
                .Select(r => r!.PlaceID!.Value)
                .ToHashSet();

            return availableTables
                .Intersect(activeTables)
                .Except(reservedTables)
                .Any();
        }
    }
}
