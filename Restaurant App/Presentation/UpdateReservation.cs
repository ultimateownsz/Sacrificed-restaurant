using Project.Presentation;

public static class UpdateReservation
{
    public static void Show(ReservationModel reservation)
    {
        Console.Clear();
        Console.WriteLine("Update Reservation Details");
        Console.WriteLine("--------------------------");

        // Display current reservation details
        DisplayReservationDetails(reservation);

        // Proceed to update reservation
        UpdateReservationDetails(reservation);

        // Save updated reservation
        ReservationAdminLogic.UpdateReservation(reservation);

        Console.WriteLine("\nReservation updated successfully.");
        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
    }

    public static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format date and display reservation details
        DateTime formattedDate = DateTime.ParseExact(reservation.Date.ToString("D8"), "ddMMyyyy", null);
        Console.WriteLine($"Reservation ID: {reservation.ID}");
        Console.WriteLine($"Date: {formattedDate:dd/MM/yyyy}");
        Console.WriteLine($"Table number: {reservation.TableID}");
        Console.WriteLine($"Number of People: {reservation.ReservationAmount}");
        Console.WriteLine($"User ID: {reservation.UserID}");
    }

    private static void UpdateReservationDetails(ReservationModel reservation)
    {
        // Update reservation date
        DateTime newDate;
        while (true)
        {
            Console.WriteLine("\nEnter new Reservation Date (DD/MM/YYYY) or press Enter to keep current:");
            string newDateInput = Console.ReadLine();
            if (string.IsNullOrEmpty(newDateInput))
            {
                Console.WriteLine("Reservation Date not updated.");
                break;
            }
            else if (DateTime.TryParseExact(newDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newDate))
            {
                if (newDate.Date < DateTime.Today)
                {
                    Console.WriteLine("The date cannot be in the past. Please enter a future date.");
                }
                else if (newDate.Date > new DateTime(2025, 12, 31))
                {
                    Console.WriteLine("The date cannot be after December 31, 2025. Please enter a valid date.");
                }
                else
                {
                    reservation.Date = long.Parse(newDate.ToString("ddMMyyyy")); // Store as long
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in the format dd/MM/yyyy.");
            }
        }

        // Update Table ID
        while (true)
        {
            // Display table capacities
            Console.WriteLine("\nTables:");
            Console.WriteLine("2-person tables: 1, 4, 5, 8, 9, 11, 12, 15");
            Console.WriteLine("4-person tables: 6, 7, 10, 13, 14");
            Console.WriteLine("6-person tables: 2, 3");

            Console.WriteLine("\nEnter new Table number (1-15) or press Enter to keep current:");
            string tableIdInput = Console.ReadLine();

            if (string.IsNullOrEmpty(tableIdInput))
            {
                Console.WriteLine("Table ID not updated.");
                break;
            }
            else if (int.TryParse(tableIdInput, out int tableID) && tableID >= 1 && tableID <= 15)
            {
                // Check if the table is taken for the given date
                if (IsTableTaken(reservation.Date, tableID))
                {
                    Console.WriteLine("This table is already reserved for the selected date. Please choose a different table.");
                }
                else
                {
                    // Assign the new table ID
                    reservation.TableID = tableID;
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid Table ID. Please choose a valid table ID between 1 and 15.");
            }
        }

        // Update Reservation Amount
        while (true)
        {
            Console.WriteLine("\nEnter new number of guests (Reservation Amount) or press Enter to keep current:");
            string newAmountInput = Console.ReadLine();

            if (string.IsNullOrEmpty(newAmountInput))
            {
                Console.WriteLine("Reservation Amount not updated.");
                break;
            }
            else if (int.TryParse(newAmountInput, out int newAmount))
            {
                // Validate the reservation amount based on the table ID
                if (IsReservationAmountValid(reservation.TableID, newAmount))
                {
                    reservation.ReservationAmount = newAmount;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid number of people for the selected table. Please enter a valid number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }
    }

    private static bool IsReservationAmountValid(long tableID, int reservationAmount)
    {
        // Check table ID categories and validate reservation amount
        if (IsTwoPersonTable(tableID) && (reservationAmount == 1 || reservationAmount == 2))
        {
            return true;
        }
        else if (IsFourPersonTable(tableID) && (reservationAmount >= 3 && reservationAmount <= 4))
        {
            return true;
        }
        else if (IsSixPersonTable(tableID) && (reservationAmount >= 5 && reservationAmount <= 6))
        {
            return true;
        }

        // If none of the conditions are met, return false
        return false;
    }

    // Helper methods for table categories
    private static bool IsTwoPersonTable(long tableID)
    {
        return tableID == 1 || tableID == 4 || tableID == 5 || tableID == 8 || tableID == 9 || tableID == 11 || tableID == 12 || tableID == 15;
    }

    private static bool IsFourPersonTable(long tableID)
    {
        return tableID == 6 || tableID == 7 || tableID == 10 || tableID == 13 || tableID == 14;
    }

    private static bool IsSixPersonTable(long tableID)
    {
        return tableID == 2 || tableID == 3;
    }

    // Helper method to check if the table is already reserved for the given date
    private static bool IsTableTaken(long reservationDate, long tableID)
    {
        int formattedDate = (int)reservationDate; // Convert long to int
        var reservations = ReservationAccess.GetReservationsByDate(formattedDate);

        foreach (var res in reservations)
        {
            if (res.TableID == tableID && res.ID != reservationDate) // Ignore the current reservation
            {
                return true; // Table is taken
            }
        }
        return false; // Table is available
    }
}
