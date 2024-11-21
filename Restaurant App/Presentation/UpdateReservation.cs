public static class UpdateReservation
{
    public static void Show()
    {
        // Flag to control when the user has entered valid input
        bool isValid = false;

        // Loop until valid input is provided
        while (!isValid)
        {
            Console.Clear();
            Console.Write("(Q)uit or reservation ID: ");
            string input = Console.ReadLine().ToLower();

            if (input == "q")
            {
                AdminMenu.AdminStart();
                return;
            }

            // Check if the input can be parsed into an integer (Reservation ID)
            if (int.TryParse(input, out int reservationID))
            {
                // Retrieve the reservation details by ID
                var reservation = ReservationAdminLogic.GetReservationByID(reservationID);
                if (reservation != null)  // If reservation exists
                {
                    Console.Clear();
                    Console.WriteLine("Current Reservation Details:");
                    DisplayReservationDetails(reservation);

                    // Allow the admin to update the reservation details
                    UpdateReservationDetails(reservation);

                    // Save the updated reservation to the database or system
                    ReservationAdminLogic.UpdateReservation(reservation);

                    Console.WriteLine("\nReservation updated successfully.");
                    isValid = true;  // Mark input as valid and exit the loop
                }
                else
                {
                    Console.WriteLine("Reservation not found. Try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Reservation ID format. Please try again.");
            }
        
        Console.Write("\nPress enter to continue..."); Console.ReadKey(); return;
        
        }
    }

    private static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format the Date (stored as an integer) to dd/MM/yyyy format
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);

        // Display the reservation details including the formatted date, table choice, number of people, and user ID
        Console.WriteLine($"ReservationID: {reservation.ID}, Date: {formattedDate:dd/MM/yyyy}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");
    }

    // Method to handle updates to reservation details (Date, Reservation Amount, Table Choice)
    private static void UpdateReservationDetails(ReservationModel reservation)
    {
        DateTime newDate;
        while (true)
        {
            Console.Write("\nEnter new Reservation Date (DD/MM/YYYY) or press Enter to keep current: ");
            string newDateInput = Console.ReadLine();
            if (string.IsNullOrEmpty(newDateInput))
            {
                Console.WriteLine("Reservation Date not updated.");
                break;
            }
            else if (DateTime.TryParseExact(newDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newDate))
            {
                // Check if the date is in the future and before the end of 2025
                DateTime maxDate = new DateTime(2025, 12, 31);
                if (newDate.Date < DateTime.Today)
                {
                    Console.WriteLine("The date cannot be in the past. Please enter a future date.");
                }
                else if (newDate.Date > maxDate)
                {
                    Console.WriteLine("The date cannot be after December 31, 2025. Please enter a valid date.");
                }
                else
                {
                    reservation.Date = int.Parse(newDate.ToString("ddMMyyyy"));
                    break; // Valid date, exit the loop
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in the format dd/MM/yyyy.");
            }
        }

        long newTableChoice;
        while (true)
        {
            Console.Write("\nEnter new Table Choice (2, 4, or 6) or press Enter to keep current: ");
            string newTableChoiceInput = Console.ReadLine();
            if (string.IsNullOrEmpty(newTableChoiceInput))
            {
                Console.WriteLine("Table Choice not updated.");
                break;
            }
            else if (long.TryParse(newTableChoiceInput, out newTableChoice) && (newTableChoice == 2 || newTableChoice == 4 || newTableChoice == 6))
            {
                reservation.TableChoice = newTableChoice;
                break; // Valid table choice, exit the loop
            }
            else
            {
                Console.WriteLine("Invalid table choice. Please choose a table with 2, 4, or 6 seats.");
            }
        }

        int newAmount;
        while (true)
        {
            Console.Write("\nEnter new number of people (Reservation Amount) or press Enter to keep current: ");
            string newAmountInput = Console.ReadLine();
            if (string.IsNullOrEmpty(newAmountInput))
            {
                Console.WriteLine("Reservation Amount not updated.");
                break;
            }
            else if (int.TryParse(newAmountInput, out newAmount))
            {
                // Validate the number of people based on the table choice
                if (reservation.TableChoice == 2)
                {
                    if (newAmount == 1 || newAmount == 2)
                    {
                        reservation.ReservationAmount = newAmount;
                        break; // Valid amount, exit the loop
                    }
                    else
                    {
                        Console.WriteLine("For a table for 2, the number of people can only be 1 or 2.");
                    }
                }
                else if (reservation.TableChoice == 4)
                {
                    if (newAmount == 3 || newAmount == 4)
                    {
                        reservation.ReservationAmount = newAmount;
                        break; // Valid amount, exit the loop
                    }
                    else
                    {
                        Console.WriteLine("For a table for 4, the number of people can only be 3 or 4.");
                    }
                }
                else if (reservation.TableChoice == 6)
                {
                    if (newAmount == 5 || newAmount == 6)
                    {
                        reservation.ReservationAmount = newAmount;
                        break; // Valid amount, exit the loop
                    }
                    else
                    {
                        Console.WriteLine("For a table for 6, the number of people can only be 5 or 6.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Invalid number of people. Please enter a valid number for table choice {reservation.TableChoice}.");
            }
        }
    }
}
