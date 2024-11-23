namespace Project.Presentation;

public static class ShowAllReservations
{
    public static void Show()
    {

        string filterChoice;

        while (true)
        {
            Console.Clear();
            Console.Write("Filter by month and year? (Y/N): ");
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
                Console.Write("\nMonth (MM): ");
                string monthInput = Console.ReadLine();

                Console.Write("Year (YYYY): ");
                string yearInput = Console.ReadLine();

                if (ReservationLogic.IsValidMonthYear(monthInput, yearInput, out int month, out int year))
                {
                    reservations = ReservationAdminLogic.GetReservationsByMonthYear(month, year);
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid month or year format, please try again. \n" +
                                      $"(ensure month is month and year is between 2024 and {DateTime.Now.Year})");
                }
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("RESERVATIONS");
            reservations = ReservationAdminLogic.GetAllReservations();
        }

        if (reservations.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("RESERVATIONS\n\nNo reservations found.");
        }
        else
        {
            foreach (var reservation in reservations)
            {
                DisplayReservationDetails(reservation);
            }
        }

        Console.WriteLine("\nPress enter to continue");
        Console.ReadKey();
        return;
    }


    // this method follows the format for the previously implemented underneath 
    public static void DisplayReservationHeader(ReservationModel reservation)
    {
        var r = reservation;
        Console.WriteLine($"ReservationID: {r.ID}, Date: {r.Date}, Table Choice: {r.TableChoice}" +
                          $", Number of People: {r.ReservationAmount}, UserID {r.UserID}");
    }

    public static void DisplayReservationDetails(ReservationModel reservation)
    {
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);
        Console.WriteLine("");
        Console.WriteLine($"ReservationID: {reservation.ID}, Date: {formattedDate:dd/MM/yyyy}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");

        var menuItems = ReservationAdminLogic.GetMenuItemsForReservation((int)reservation.ID);
        string theme = ReservationLogic.GetThemeByReservation((int)reservation.ID);

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


}
