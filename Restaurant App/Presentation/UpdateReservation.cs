using Project;
using Project.Presentation;
using System;
using System.Linq;
using System.Collections.Generic;

public static class UpdateReservation
{
    // Mapping of tables to their seat capacities
    private static readonly Dictionary<int, int> TableSeats = new Dictionary<int, int>
    {
        { 1, 2 },  // Table 1 has 2 seats
        { 2, 4 },  // Table 2 has 4 seats
        { 3, 4 },  // Table 3 has 4 seats
        { 4, 6 },  // Table 4 has 6 seats
        { 5, 2 },  // Table 5 has 2 seats
        { 6, 6 },  // Table 6 has 6 seats
        { 7, 4 },  // Table 7 has 4 seats
        { 8, 2 },  // Table 8 has 2 seats
        { 9, 2 },  // Table 9 has 2 seats
        { 10, 4 }, // Table 10 has 4 seats
        { 11, 2 }, // Table 11 has 2 seats
        { 12, 2 }, // Table 12 has 2 seats
        { 13, 4 }, // Table 13 has 4 seats
        { 14, 4 }, // Table 14 has 4 seats
        { 15, 6 }  // Table 15 has 6 seats
    };

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
        Access.Reservations.Update(reservation);

        Console.WriteLine("\nReservation updated.");
        Console.WriteLine("Press any key to return.");
        Console.ReadKey();
        ShowReservations.ShowReservationOptions(reservation);
    }

    public static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format date and display reservation details
        Console.WriteLine($"Reservation ID: {reservation.ID}");
        Console.WriteLine($"Date: {reservation.Date:dd/MM/yyyy}");
        Console.WriteLine($"Table number: {reservation.Place}");
        Console.WriteLine($"User ID: {reservation.UserID}");
    }

    private static void UpdateReservationDetails(ReservationModel reservation)
    {
        // Ask for new date
        DateTime newDate = DateTime.MinValue;  // Initialize newDate to avoid unassigned variable error
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
                    reservation.Date = newDate; // Store as long
                    break;
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in the format dd/MM/yyyy.");
            }
        }

        // Ask for the number of people (1-6)
        int numberOfPeople = AskForNumberOfPeople();

        // Display available tables based on number of people
        var availableTables = GetAvailableTables(newDate, numberOfPeople);

        if (availableTables.Count == 0)
        {
            Console.WriteLine("No available tables for the selected number of people.");
            return;
        }

        // Display available tables
        Console.WriteLine("\nAvailable Tables:");
        foreach (var table in availableTables)
        {
            Console.WriteLine($"Table {table} ({TableSeats[table]} seats)");
        }

        // Ask user to select a new table
        while (true)
        {
            Console.WriteLine("\nEnter new Table number or press Enter to keep current:");
            string tableIdInput = Console.ReadLine();

            if (string.IsNullOrEmpty(tableIdInput))
            {
                Console.WriteLine("Table ID not updated.");
                break;
            }
            else if (int.TryParse(tableIdInput, out int tableID) && availableTables.Contains(tableID))
            {
                reservation.Place = tableID;
                Console.WriteLine($"Table updated to Table {tableID}.");
                break;
            }
            else
            {
                Console.WriteLine("Invalid Table ID. Please choose a valid table ID.");
            }
        }
    }

    private static int AskForNumberOfPeople()
    {
        int numberOfPeople = 0;
        while (true)
        {
            Console.WriteLine("Enter the number of people for the reservation (1-6):");
            string input = Console.ReadLine();
            if (int.TryParse(input, out numberOfPeople) && numberOfPeople >= 1 && numberOfPeople <= 6)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid number. Please enter a number between 1 and 6.");
            }
        }
        return numberOfPeople;
    }

    private static List<int> GetAvailableTables(DateTime date, int numberOfPeople)
    {
        // Get all reservations for the given date
        var potentialReservations = Access.Reservations.GetAll();
        var reservedTables = potentialReservations
            .Where(r => r.Date.HasValue && r.Date.Value.Date == date.Date)
            .Select(r => r.Place)  // Get the table numbers that are already reserved
            .ToList();

        // Filter tables based on the number of people and availability
        var availableTables = TableSeats
            .Where(t => IsValidReservationAmount(t.Key, numberOfPeople) && !reservedTables.Contains(t.Key))
            .Select(t => t.Key)  // Table numbers that are available and can fit the number of people
            .ToList();

        return availableTables;
    }

    private static bool IsValidReservationAmount(int tableID, int numberOfPeople)
    {
        if (TableSeats[tableID] == 2 && (numberOfPeople == 1 || numberOfPeople == 2))
        {
            return true;
        }
        else if (TableSeats[tableID] == 4 && (numberOfPeople == 3 || numberOfPeople == 4))
        {
            return true;
        }
        else if (TableSeats[tableID] == 6 && (numberOfPeople == 5 || numberOfPeople == 6))
        {
            return true;
        }
        return false;
    }
}
