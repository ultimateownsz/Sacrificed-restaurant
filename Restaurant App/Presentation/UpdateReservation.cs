using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Project;
using Project.Presentation;

namespace Presentation
{

    public static class UpdateReservation
    {
        public static void Show(ReservationModel reservation, UserModel acc) // Adding modularity for admin and user
        {
            bool admin = acc.Admin.HasValue && acc.Admin.Value == 1;

            Console.Clear();
            Console.WriteLine("Update Reservation Details");
            Console.WriteLine("--------------------------");

            // Display current reservation details
            DisplayReservationDetails(reservation);

            // Proceed to update reservation
            if (admin)
            {
                UpdateReservationAdmin(reservation, acc);
            }
            else
            {
                UpdateReservationUser(reservation, acc);
            }

            // Save updated reservation
            Access.Reservations.Update(reservation);

        } 

        public static void DisplayReservationDetails(ReservationModel reservation)
        {
            // Format date and display reservation details
            Console.WriteLine($"Reservation ID: {reservation.ID}");
            Console.WriteLine($"Date: {reservation.Date}");
            Console.WriteLine($"Table number: {reservation.PlaceID}");
            Console.WriteLine($"User ID: {reservation.UserID}");
        }

    public static void UpdateReservationAdmin(ReservationModel reservation, UserModel acc)
    {
        string confirmChoice = $"UPDATE RESERVATION\nReservation for date: {reservation.Date:dd/MM/yyyy}";
        while (true)
        {
            switch (SelectionPresent.Show(["Date", "Table"], banner: confirmChoice).ElementAt(0).text)
            {
                case "Date":
                    Console.Clear();
                    UpdateReservationDate(reservation, acc);
                    break;

                case "Table":
                    Console.Clear();
                    UpdateTableID(reservation);
                    break;

                case "":
                    return;
                }
            }
        }

    public static void UpdateReservationUser(ReservationModel reservation, UserModel acc)
    {
        string confirmChoice = $"UPDATE RESERVATION\nReservation for the date {reservation.Date:dd/MM/yyyy}";
        while (true)
        {
            switch (SelectionPresent.Show(["Date", "Table", "Cancel reservation"], banner: confirmChoice).ElementAt(0).text)
            {
                case "Date":
                    Console.Clear();
                    UpdateReservationDate(reservation, acc);
                    break;
               
                case "Table":
                    Console.Clear();
                    UpdateTableID(reservation);
                    break;
                
                case "Cancel reservation":
                    Console.Clear();
                    if (DeleteReservation(reservation))
                    {
                        return; // Exit after deletion
                    }
                    break;
                
                case "":
                    return;
            }
        }
    }

        private static void UpdateReservationDate(ReservationModel reservation, UserModel acc)
        {
            // Update reservation date
            bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;
            int guests = 1;

            while (true)
            {
                DateTime selectedDate = CalendarPresent.Show(DateTime.Now, isAdmin, guests, acc);

                if (selectedDate.Date == DateTime.MinValue)
                {
                    return;
                }

                reservation.Date = selectedDate;
                break;
            }
        }
        
        private static void UpdateTableID(ReservationModel reservation)
        {
            bool isAdmin = false;
            // Update Table ID
            while (true)
            {
                List<string> options = new List<string>() { "1", "2", "3", "4", "5", "6" };
                string banner = "How many guests are reserved for your table?";
                int guests = options.Count() - SelectionPresent.Show(options, banner: banner, mode: SelectionLogic.Mode.Scroll).ElementAt(0).index;

                // because 6 - (-1) = 7
                if (guests == 7)
                    return;

                int[] inactiveTables = Access.Places.Read()
                    .Where(p => p.Active == 0)
                    .Select(p => p.ID.Value)
                    .ToArray();

                DateTime? selectedDate = reservation.Date;
                int[] reservedTables = Access.Reservations
                    .GetAllBy<DateTime?>("Date", selectedDate)
                    .Where(r => r?.PlaceID != null)
                    .Select(r => r!.PlaceID!.Value)
                    .ToArray();

                TableSelection tableSelection = new();
                int[] availableTables = guests switch
                {
                    1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                    3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                    5 or 6 => new int[] { 2, 3 },
                    _ => Array.Empty<int>()
                };

                while (true)
                {
                    int selectedTable = tableSelection.SelectTable(availableTables, inactiveTables, reservedTables, guests, isAdmin);

                    if (selectedTable == -1)
                    {
                        Console.WriteLine("Returning to previous menu...");
                        break;
                    }

                    if (IsTableTaken(reservation.Date, selectedTable))
                    {
                        Console.WriteLine("This table is already reserved for this particular date...");
                    }
                    else
                    {
                        if (reservation.PlaceID.HasValue)
                        {
                            Console.WriteLine("Old reserved table has been replaced...");
                        }

                        reservation.PlaceID = selectedTable;
                        return;
                    }
                }
            }
        }

    private static bool DeleteReservation(ReservationModel reservation)
    {
        // Check if the reservation date is in the past
        if (reservation.Date < DateTime.Today)
        {
            Terminable.Write("You cannot cancel a reservation that is in the past.");
            Thread.Sleep(1000);
            return false; // Cant cancel a past reservation
        }

        // Confirm deletion
        var options = new List<string> { "Yes", "No" };
        var choice = SelectionPresent.Show(options, banner: "Are you sure you?").ElementAt(0);

        if (choice.text == "Yes")
        {
            Access.Reservations.Delete(reservation.ID);
            Terminable.Write($"Reservation for {reservation.Date:dd/MM/yyyy} cancelled successfully.");
            Thread.Sleep(1000);
            return true; // Deletion was successful
        }
        else
        {
            Terminable.Write("Reservation not cancelled.");
            Thread.Sleep(1000);
            return false; // Deletion was cancelled
        }
    }

        private static void UdpateReservationAmount(ReservationModel reservation)
        {
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
                    //if (IsReservationAmountValid(reservation.Place, newAmount))
                    //{
                    //    reservation.ReservationAmount = newAmount;
                    //    break;
                    //}
                    //else
                    //{
                    //    Console.WriteLine("Invalid number of people for the selected table. Please enter a valid number.");
                    //}
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
        private static bool IsTableTaken(DateTime? reservationDate, int tableID)
        {
            var reservations = Access.Reservations.GetAllBy<DateTime?>("Date", reservationDate);

            foreach (var res in reservations)
            {
                if (res.PlaceID == tableID && res.Date != reservationDate) // Ignore the current reservation
                {
                    return true; // Table is taken
                }
            }
            return false; // Table is available
        }
    }
}
