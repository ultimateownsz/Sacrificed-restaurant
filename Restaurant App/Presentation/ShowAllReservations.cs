public static class ShowAllReservations
{
    public static void Show()
    {
        string filterChoice;

        while (true)
        {
            Console.WriteLine("Do you want to filter reservations by month? (Y/N)");
            filterChoice = Console.ReadLine().ToLower();

            if (filterChoice == "y" || filterChoice == "n")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'Y' for Yes or 'N' for No.");
            }
        }

        List<ReservationModel> reservations;

        if (filterChoice == "y")
        {
            while (true)
            {
                Console.Write("Enter month (MM): ");
                string monthInput = Console.ReadLine();

                Console.Write("Enter year (YYYY): ");
                string yearInput = Console.ReadLine();

                if (IsValidMonthYear(monthInput, yearInput, out int month, out int year))
                {
                    reservations = ReservationAdminLogic.GetReservationsByMonthYear(month, year);
                    break;
                }
                else
                {
                    Console.WriteLine($"Invalid month or year format. Please try again. Ensure month is MM and year is between 2024 and {DateTime.Now.Year}.");
                }
            }
        }
        else
        {
            Console.WriteLine("Showing all reservations");
            reservations = ReservationAdminLogic.GetAllReservations();
        }

        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
        }
        else
        {
            foreach (var reservation in reservations)
            {
                DisplayReservationDetails(reservation);
            }
        }

        AdminMenu.AdminStart();
    }

    private static bool IsValidMonthYear(string monthInput, string yearInput, out int month, out int year)
    {
        month = 0;
        year = 0;

        return monthInput.Length == 2 && yearInput.Length == 4
            && int.TryParse(monthInput, out month) && int.TryParse(yearInput, out year)
            && month >= 1 && month <= 12
            && year >= 2024 && year <= DateTime.Now.Year;
    }

    static void DisplayReservationDetails(ReservationModel reservation)
    {
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);
        Console.WriteLine("");
        Console.WriteLine($"ReservationID: {reservation.ID}, Date: {formattedDate:dd/MM/yyyy}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");

        var menuItems = ReservationAdminLogic.GetMenuItemsForReservation((int)reservation.ID);
        string theme = GetThemeByReservation((int)reservation.ID);

        if (!string.IsNullOrEmpty(theme))
        {
            Console.WriteLine($"Theme for the month: {theme}");
        }
        else
        {
            Console.WriteLine("No theme found for this reservation.");
        }

        if (menuItems.Count > 0)
        {
            Console.WriteLine("Selected Menu Items:");
            foreach (var item in menuItems)
            {
                Console.WriteLine($"- {item.Category}: {item.ProductName} (Quantity: {item.Quantity}, Price: {item.Price})");
            }
        }
        else
        {
            Console.WriteLine("No menu items found for this reservation.");
        }
    }

    private static string GetThemeByReservation(int reservationID)
    {
        var menuItems = ReservationAdminLogic.GetMenuItemsForReservation(reservationID);

        if (menuItems != null && menuItems.Count > 0)
        {
            int menuID = (int)menuItems.First().MenuID; // Explicit cast from long to int
            return ReservationAdminLogic.GetThemeByMenuID(menuID);
        }

        return string.Empty;
    }
}
