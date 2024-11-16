public static class FilterReservations
{
    public static void Show()
    {
        // Flag to control when the user has entered valid input
        bool isValid = false;

        // Loop until the user provides valid input
        while (!isValid)
        {
            Console.WriteLine("");
            Console.WriteLine("Enter filter criteria:");
            Console.WriteLine("1. Filter by Reservation ID");
            Console.WriteLine("2. Filter by Date");
            // Console.WriteLine("3. Filter by User ID");
            Console.WriteLine("Q. Go back to Admin Menu");
            Console.WriteLine("");

            string? filterChoice = Console.ReadLine()?.ToLower();

            if (filterChoice == "q")
            {
                AdminMenu.AdminStart();
                return;
            }

            // Process the user's choice based on the selected filter criteria
            switch (filterChoice)
            {
                case "1":
                    // Filter by Reservation ID
                    Console.WriteLine("");
                    Console.Write("Enter Reservation ID: ");
                    if (int.TryParse(Console.ReadLine(), out int reservationID))  // Try to parse the Reservation ID input
                    {
                        // Get reservation by ID from the business logic layer
                        var reservation = ReservationAdminLogic.GetReservationByID(reservationID);
                        if (reservation != null)  // If reservation is found
                        {
                            // Display reservation details
                            DisplayReservationDetails(reservation);
                            isValid = true;  // Mark input as valid
                        }
                        else
                        {
                            Console.WriteLine("No reservation found with that ID. Try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Reservation ID format. Please try again.");
                    }
                    break;

                case "2":
                    // Filter by Date
                    Console.WriteLine("");
                    Console.Write("Enter Date (DD/MM/YYYY): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateParsed))  // Try to parse the date input
                    {
                        // Convert parsed date to an integer format (01122024 for 01/12/2024)
                        int dateInt = int.Parse(dateParsed.ToString("ddMMyyyy"));
                        // Get reservations by the parsed date
                        var reservationsByDate = ReservationAdminLogic.GetReservationsByDate(dateInt);
                        if (reservationsByDate.Count > 0)  // If reservations exist for the given date
                        {
                            // Display each reservation found for the selected date
                            foreach (var reservation in reservationsByDate)
                            {
                                DisplayReservationDetails(reservation);
                            }
                            isValid = true;  // Mark input as valid
                        }
                        else
                        {
                            Console.WriteLine("No reservations found for that date. Try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Date format. Please use DD/MM/YYYY.");
                    }
                    break;

                // case "3":
                //     // Filter by User ID
                //     Console.WriteLine("");
                //     Console.Write("Enter User ID: ");
                //     if (int.TryParse(Console.ReadLine(), out int userID))  // Try to parse the User ID input
                //     {
                //         // Get reservations by User ID
                //         var reservationsByUserID = ReservationAdminLogic.GetReservationsByUserID(userID);
                //         if (reservationsByUserID.Count > 0)  // If reservations exist for the given User ID
                //         {
                //             // Display each reservation found for the selected User ID
                //             foreach (var reservation in reservationsByUserID)
                //             {
                //                 DisplayReservationDetails(reservation);
                //             }
                //             isValid = true;  // Mark input as valid
                //         }
                //         else
                //         {
                //             // If no reservations are found for that User ID
                //             Console.WriteLine("No reservations found for that User ID. Try again.");
                //         }
                //     }
                //     else
                //     {
                //         // If the User ID is not in the correct format
                //         Console.WriteLine("Invalid User ID format. Please try again.");
                //     }
                //     break;

                default:
                    // If the user enters an invalid choice
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }

            // If input is invalid, the loop will continue asking for correct input
            Show();
        }
    }

    static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format the Date as dd/MM/yyyy from the stored integer representation
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);

        Console.WriteLine("");

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
