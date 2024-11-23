using Project.Presentation;

public static class FilterReservations
{
    public static void Show()
    {
        // Flag to control when the user has entered valid input
        bool isValid = false;

        // Loop until the user provides valid input
        while (!isValid)
        {
            switch (SelectionMenu.Show(["ID", "date\n", "back"], "FILTER BY\n\n"))
            {
                case "ID":

                    // display all reservation headers
                    Console.Clear();
                    foreach (var reservation in ReservationAdminLogic.GetAllReservations())
                    {
                        UpdateReservation.DisplayReservationDetails(reservation);
                    }

                    Console.Write("\nEnter 'Q' to quit or a reservation ID: ");
                    string? ID = Console.ReadLine();

                    if (ID.ToLower() == "q")
                    {
                        continue;
                    }

                    if (int.TryParse(ID, out int reservationID))  // Try to parse the Reservation ID input
                    {
                        // Get reservation by ID from the business logic layer
                        var reservation = ReservationAdminLogic.GetReservationByID(reservationID);
                        if (reservation != null)  // If reservation is found
                        {
                            // Display reservation details
                            Console.Clear();
                            Console.WriteLine("RESERVATIONS\n");
                            DisplayReservationDetails(reservation);
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("RESERVATIONS\n\nNo reservations found.\n");
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Reservation ID format. Please try again.\n");
                    }
                    Console.Write("Press enter to continue..."); Console.ReadKey(); continue;

                case "date\n":
                    
                    Console.Clear();
                    Console.Write("Enter 'Q' to quit or a date (DD/MM/YYYY): ");
                    string? date = Console.ReadLine();

                    if (date.ToLower() == "q")
                    {
                        continue;
                    }


                    if (DateTime.TryParseExact(date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateParsed))  // Try to parse the date input
                    {
                        // Convert parsed date to an integer format (01122024 for 01/12/2024)
                        int dateInt = int.Parse(dateParsed.ToString("ddMMyyyy"));
                        // Get reservations by the parsed date
                        var reservationsByDate = ReservationAdminLogic.GetReservationsByDate(dateInt);
                        if (reservationsByDate.Count > 0)  // If reservations exist for the given date
                        {
                            Console.Clear();
                            Console.WriteLine("RESERVATIONS\n");
                            // Display each reservation found for the selected date
                            foreach (var reservation in reservationsByDate)
                            {
                                DisplayReservationDetails(reservation);
                                Console.WriteLine();
                            }
                            isValid = true;  // Mark input as valid
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("RESERVATIONS\n\nNo reservations found.\n");
                        }
                        Console.Write("Press enter to continue..."); Console.ReadKey(); break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid Date format. Please use DD/MM/YYYY.");
                        Console.Write("\nPress enter to continue..."); Console.ReadKey(); continue;
                    }

                case "back":
                    return;
            }
        }
    }

    static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format the Date as dd/MM/yyyy from the stored integer representation
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);

        // Display the reservation details: ReservationID, Date, TableChoice, Number of People, UserID
        Console.WriteLine($"ReservationID: {reservation.ID}, Date: {formattedDate:dd/MM/yyyy}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");

        // Fetch the menu items associated with this reservation using its reservation ID
        var menuItems = ReservationAdminLogic.GetMenuItemsForReservation((int)reservation.ID);

        // Check if there are menu items associated with this reservation
        if (menuItems.Count > 0)
        {
            Console.WriteLine("Selected Menu Items:");

            // Loop through each menu item and display its category, product name, quantity, and price
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
