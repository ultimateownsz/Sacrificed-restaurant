using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Project;
using Project.Presentation;

public static class UpdateReservation
{
    public static void Show(ReservationModel reservation, bool admin) // Adding modularity for admin and user
    {
        TryCatchHelper.EscapeKeyException(() =>
        {
            Console.Clear();
            Console.WriteLine("Update Reservation Details");
            Console.WriteLine("--------------------------");

            // Display current reservation details
            DisplayReservationDetails(reservation);

            do
            {
                var options = new List<string> { "Date", "Table\n", "Back" };
                string menuTitle = "UPDATE RESERVATION\n\n";
                dynamic selection = SelectionPresent.Show(options, menuTitle);

                if (selection.text == null)
                {
                    return;
                }

                switch(selection.text)
                {
                    case "Date":
                        Console.Clear();
                        if (UpdateReservationDate(reservation))
                            Console.WriteLine("\nDate updated successfully.");
                            // Thread.Sleep(1500);
                        break;

                    case "Table":
                        Console.Clear();
                        if (UpdateTableID(reservation))
                            Console.WriteLine("\nTable number updated successfully.");
                            // Thread.Sleep(1500);
                        break;

                    case "Back":
                        return;
                }
                Access.Reservations.Update(reservation);

            } while (true);
            // // Proceed to update reservation
            // if (admin)
            // {
            //     UpdateReservationAdmin(reservation);
            // }
            // else
            // {
            //     UpdateReservationUser(reservation);
            // }

            // Save updated reservation
        });
    } 

    public static void DisplayReservationDetails(ReservationModel reservation)
    {
        // Format date and display reservation details
        Console.WriteLine($"Reservation ID: {reservation.ID}");
        Console.WriteLine($"Date: {reservation.Date}");
        Console.WriteLine($"Table number: {reservation.PlaceID}");
        Console.WriteLine($"User ID: {reservation.UserID}");
    }

    // public static void UpdateReservationAdmin(ReservationModel reservation)
    // {
    //     TryCatchHelper.EscapeKeyException(() =>
    //     {
    //         string confirmChoice = "UPDATE RESERVATION\n\n";
    //         do
    //         {
    //             Console.Clear();
    //             dynamic selection = SelectionPresent.Show(["Date", "Table\n", "Back"], confirmChoice);
    //             if (selection.text == null)
    //             {
    //                 Thread.Sleep(1500);  // wait 1,5 seconds before you return to last menu
    //                 return;
    //             }
                
    //             switch (selection.text)
    //             {
    //                 case "Date":
    //                     Console.Clear();
    //                     UpdateReservationDate(reservation);
    //                     Console.WriteLine("\nDate process ended successfully.");
    //                     Console.WriteLine("Press any key to return.");
    //                     Console.ReadKey();
    //                     break;

    //                 case "Table\n":
    //                     Console.Clear();
    //                     UpdateTableID(reservation);
    //                     Console.WriteLine("\nTable number process ended successfully.");
    //                     Console.WriteLine("Press any key to return.");
    //                     Console.ReadKey();
    //                     break;

    //                 case "Back":
    //                     return;
    //             }
    //         } while (true);

    //     }
    //     );
    // }

    // public static void UpdateReservationUser(ReservationModel reservation)
    // {
    //     string confirmChoice = "UPDATE RESERVATION\n\n";
    //     while (true)
    //     {

    //         switch (SelectionPresent.Show(["Date", "Table\n", "Back"], confirmChoice).text)
    //         {
    //             case "Date":
    //                 Console.Clear();
    //                 UpdateReservationDate(reservation);
    //                 Console.WriteLine("\nDate process ended successfully.");
    //                 Console.WriteLine("Press any key to return.");
    //                 Console.ReadKey();
    //                 break;
               
    //             case "Table\n":
    //                 Console.Clear();
    //                 UpdateTableID(reservation);
    //                 Console.WriteLine("\nTable number process ended successfully.");
    //                 Console.WriteLine("Press any key to return.");
    //                 Console.ReadKey();
    //                 break;
               
    //             // THIS WILL BE IMPLEMENTED AFTER MAKING A WAY TO STORE THE AMOUNT OF GUESTS
    //             // EDIT: THEN DON"T FUCKING IMPLEMENT THE LOGIC UNTIL IT'S DONE
    //             case "Number of guests":
    //                 Console.Clear();
    //                 Console.WriteLine("\nThis is a concept that might or might not be approved by the PO.");
    //                 Console.WriteLine("Press any key to return.");
    //                 Console.ReadKey();
    //                 break;
                
    //             case "Back":
    //                 return;
    //         }
    //     }
    // }

    private static bool UpdateReservationDate(ReservationModel reservation)
{
    return TryCatchHelper.EscapeKeyWithResult(() =>
    {
        string input = InputHelper.GetValidatedInput<string>(
            "Enter new Reservation Date (DD/MM/YYYY) or press Escape to cancel: ",
            input =>
            {
                // if (string.IsNullOrEmpty(input))
                //     return (reservation.Date.ToString("dd/MM/yyyy"), null); // Fixed here

                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newDate))
                {
                    if (newDate.Date < DateTime.Today)
                        return (null, "The date cannot be in the past.");
                    if (newDate.Date > new DateTime(2025, 12, 31))
                        return (null, "Date must be before December 31, 2025.");
                    
                    return (newDate.ToString("dd/MM/yyyy"), null);
                }

                return (null, "Invalid date format. Please use DD/MM/YYYY.");
            }
        );

        if (input == null) return false; // Escape key was pressed

        reservation.Date = DateTime.ParseExact(input, "dd/MM/yyyy", null);
        return true;
    });
}

        // // Update reservation date
        // DateTime newDate;
        // while (true)
        // {
        //     Console.WriteLine("\nEnter new Reservation Date (DD/MM/YYYY) or press Enter to keep current:");
        //     string newDateInput = Console.ReadLine();
        //     if (string.IsNullOrEmpty(newDateInput))
        //     {
        //         Console.WriteLine("Reservation Date not updated.");
        //         break;
        //     }
        //     else if (DateTime.TryParseExact(newDateInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newDate))
        //     {
        //         if (newDate.Date < DateTime.Today)
        //         {
        //             Console.WriteLine("The date cannot be in the past. Please enter a future date.");
        //         }
        //         else if (newDate.Date > new DateTime(2025, 12, 31))
        //         {
        //             Console.WriteLine("The date cannot be after December 31, 2025. Please enter a valid date.");
        //         }
        //         else
        //         {
        //             reservation.Date = newDate; // Store as long
        //             break;
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("Invalid date format. Please enter a date in the format dd/MM/yyyy.");
        //     }
        // }
    // }
    
    private static bool UpdateTableID(ReservationModel reservation)
    {
        return TryCatchHelper.EscapeKeyWithResult(() =>
        {
            Console.WriteLine("\nTables:");
            Console.WriteLine("2-person tables: 1, 4, 5, 8, 9, 11, 12, 15");
            Console.WriteLine("4-person tables: 6, 7, 10, 13, 14");
            Console.WriteLine("6-person tables: 2, 3");
        
            string tableInput = InputHelper.GetValidatedInput<string>(
                "Enter new Table number (1-15) or press Escape to cancel: ",
                input =>
                {
                    if (int.TryParse(input, out int tableID) && tableID >= 1 && tableID <= 15)
                    {
                        if (IsTableTaken(reservation.Date, tableID))
                            return (null, "This table is already reserved for the selected date.");
                        return (tableID.ToString(), null);
                    }
                    return (null, "Invalid Table ID. Please choose a valid table number between 1 and 15.");
                });

            if (tableInput == null) return false; // Escape key pressed
            reservation.PlaceID = int.Parse(tableInput);
            return true;
        });

        
        // // Update Table ID
        // while (true)
        // {
        //     // Display table capacities
        //     Console.WriteLine("\nTables:");
        //     Console.WriteLine("2-person tables: 1, 4, 5, 8, 9, 11, 12, 15");
        //     Console.WriteLine("4-person tables: 6, 7, 10, 13, 14");
        //     Console.WriteLine("6-person tables: 2, 3");

        //     Console.WriteLine("\nEnter new Table number (1-15) or press Enter to keep current:");
        //     string tableIdInput = Console.ReadLine();

        //     if (string.IsNullOrEmpty(tableIdInput))
        //     {
        //         Console.WriteLine("Table ID not updated.");
        //         break;
        //     }
        //     else if (int.TryParse(tableIdInput, out int tableID) && tableID >= 1 && tableID <= 15)
        //     {
        //         // Check if the table is taken for the given date
        //         if (IsTableTaken(reservation.Date, tableID))
        //         {
        //             Console.WriteLine("This table is already reserved for the selected date. Please choose a different table.");
        //         }
        //         else
        //         {
        //             // Assign the new table ID
        //             reservation.PlaceID = tableID;
        //             break;
        //         }
        //     }
        //     else
        //     {
        //         Console.WriteLine("Invalid Table ID. Please choose a valid table ID between 1 and 15.");
        //     }
        // }
    }

    private static void UpdateReservationAmount(ReservationModel reservation)
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
    private static bool IsTableTaken(DateTime? reservationDate, long tableID)
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
