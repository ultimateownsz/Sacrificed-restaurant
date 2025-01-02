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
            ControlHelpPresent.Clear();
            ControlHelpPresent.ResetToDefault();
            ControlHelpPresent.ShowHelp();
            Console.WriteLine("Update Reservation Details");
            Console.WriteLine("--------------------------");

            // Display current reservation details
            DisplayReservationDetails(reservation);

            do
            {
                var options = new List<string> { "Date", "Table\n", "Back" };
                string menuTitle = "UPDATE RESERVATION\n\n";
                var selection = SelectionPresent.Show(options, menuTitle);

                if (selection.text == null || selection.text == "Back")
                {
                    ControlHelpPresent.DisplayFeedback("Update reservation canceled...", "bottom", "success");
                    return;
                }

                switch(selection.text)
                {
                    case "Date":
                        // Console.Clear();
                        UpdateReservationDate(reservation);
                        ControlHelpPresent.ResetToDefault();
                        // if (UpdateReservationDate(reservation))
                        //     ControlHelpPresent.DisplayFeedback("\nReservation date updated successfully.", "bottom", "success");
                        //     // Thread.Sleep(1500);
                        break;

                    case "Table":
                        UpdateTableID(reservation);
                        ControlHelpPresent.ResetToDefault();
                        // Console.Clear();
                        // if (UpdateTableID(reservation))
                        //     ControlHelpPresent.DisplayFeedback("\nTable number updated successfully.", "bottom", "success");
                            // Thread.Sleep(1500);
                        break;
                }
                Access.Reservations.Update(reservation);

            } while (true);
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

    // private static bool UpdateReservationDate(ReservationModel reservation)
    // {
    //     // ControlHelpPresent.Clear();
    //     // ControlHelpPresent.AddOptions("Escape", "<escape>");
    //     // ControlHelpPresent.ShowHelp();
    //     return TryCatchHelper.EscapeKeyWithResult(() =>
    //     {
    //         ControlHelpPresent.Clear();
    //         ControlHelpPresent.AddOptions("Escape", "<escape>");
    //         ControlHelpPresent.ShowHelp();
    //         string input = InputHelper.GetValidatedInput<string>(
    //             "Enter new reservation Date (DD/MM/YYYY): ",
    //             input =>
    //             {
    //                 // if (string.IsNullOrEmpty(input))
    //                 //     return (reservation.Date.ToString("dd/MM/yyyy"), null); // Fixed here

    //                 if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime newDate))
    //                 {
    //                     if (newDate.Date < DateTime.Today)
    //                         return (null, "The date cannot be in the past.");
    //                     if (newDate.Date > new DateTime(2025, 12, 31))
    //                         return (null, "Date must be before December 31, 2025.");
                        
    //                     ControlHelpPresent.DisplayFeedback("Reservation date updated successfully.", "bottom", "success");
    //                     return (newDate.ToString("dd/MM/yyyy"), null);
    //                 }

    //                 ControlHelpPresent.DisplayFeedback("Invalid date format. Please enter a date in the format dd/MM/yyyy.", "bottom", "error");
    //                 return (null, "Invalid date format. Please enter a date in the format dd/MM/yyyy.");
    //             }
    //         );

    //         if (input == null) return false; // Escape key was pressed

    //         reservation.Date = DateTime.ParseExact(input, "dd/MM/yyyy", null);
    //         ControlHelpPresent.DisplayFeedback("Reservation date updated successfully.", "bottom", "success");
    //         ControlHelpPresent.ResetToDefault();
    //         return true;
    //     });
    // }

        private static bool UpdateReservationDate(ReservationModel reservation)
    {
        return TryCatchHelper.EscapeKeyWithResult(() =>
        {
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Escape", "<escape>");
            ControlHelpPresent.ShowHelp();

            string input = InputHelper.GetValidatedInput<string>(
                "Enter new reservation date (DD/MM/YYYY): ",
                input =>
                {
                    var (parsedDate, error) = InputLogic.ParseValidDate(input); // Use InputLogic for date validation
                    if (parsedDate == null)
                    {
                        ControlHelpPresent.DisplayFeedback(error ?? "Invalid input.", "bottom", "error");
                        return (null, error); // Return error if validation fails
                    }

                    if (parsedDate.Value.Date < DateTime.Today)
                    {
                        string errorMessage = "The date cannot be in the past.";
                        ControlHelpPresent.DisplayFeedback(errorMessage, "bottom", "error");
                        return (null, errorMessage);
                    }

                    if (parsedDate.Value.Date > new DateTime(2025, 12, 31))
                    {
                        string errorMessage = "Date must be before December 31, 2025.";
                        ControlHelpPresent.DisplayFeedback(errorMessage, "bottom", "error");
                        return (null, errorMessage);
                    }

                    return (parsedDate.Value.ToString("dd/MM/yyyy"), null); // Return the valid date
                },
                menuTitle: "UPDATE RESERVATION DATE",
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );

            if (input == null) return false; // Escape key was pressed

            reservation.Date = DateTime.ParseExact(input, "dd/MM/yyyy", null); // Parse input into DateTime
            ControlHelpPresent.DisplayFeedback("Reservation date updated successfully.", "bottom", "success");
            ControlHelpPresent.ResetToDefault();
            return true;
        });
    }

    private static bool UpdateTableID(ReservationModel reservation)
    {
        return TryCatchHelper.EscapeKeyWithResult(() =>
        {
            ControlHelpPresent.Clear();
            ControlHelpPresent.AddOptions("Escape", "<escape>");
            ControlHelpPresent.ShowHelp();

            Console.WriteLine("\nTables:");
            Console.WriteLine("2-person tables: 1, 4, 5, 8, 9, 11, 12, 15");
            Console.WriteLine("4-person tables: 6, 7, 10, 13, 14");
            Console.WriteLine("6-person tables: 2, 3");

            string tableInput = InputHelper.GetValidatedInput<string>(
                "Enter new table number (1-15): ",
                input =>
                {
                    var (parsedTableID, error) = InputLogic.ParseValidInteger(input, 1); // Use InputLogic to validate table ID
                    if (parsedTableID == null || parsedTableID > 15)
                    {
                        string errorMessage = "Table number must be between 1 and 15.";
                        ControlHelpPresent.DisplayFeedback(errorMessage, "bottom", "error");
                        return (null, errorMessage);
                    }

                    if (IsTableTaken(reservation.Date, parsedTableID.Value))
                    {
                        string errorMessage = "This table is already reserved for the selected date.";
                        ControlHelpPresent.DisplayFeedback(errorMessage, "bottom", "error");
                        return (null, errorMessage);
                    }

                    return (parsedTableID.Value.ToString(), null);
                },
                menuTitle: "UPDATE TABLE NUMBER",
                showHelpAction: () => ControlHelpPresent.ShowHelp()
            );

            if (tableInput == null) return false; // Escape key pressed

            reservation.PlaceID = int.Parse(tableInput); // Safely parse the validated input
            ControlHelpPresent.DisplayFeedback("Table number updated successfully.", "bottom", "success");
            return true;
        });
    }

    private static void UpdateReservationAmount(ReservationModel reservation)
    {
        TryCatchHelper.EscapeKeyException(() =>
        {
            while (true)
            {
                ControlHelpPresent.Clear();
                ControlHelpPresent.AddOptions("Escape", "<escape>");
                ControlHelpPresent.ShowHelp();

                Console.WriteLine("\nEnter new number of guests (reservation amount):");
                string newAmountInput = InputHelper.GetValidatedInput<string>(
                    "Number of guests: ",
                    input =>
                    {
                        if (string.IsNullOrWhiteSpace(input))
                        {
                            return (null, "Input cannot be empty. Please provide a valid number.");
                        }

                        var (parsedAmount, error) = InputLogic.ParseValidInteger(input, 1); // Minimum 1 guest
                        if (parsedAmount == null)
                        {
                            ControlHelpPresent.DisplayFeedback(error!, "bottom", "error");
                            return (null, error);
                        }

                        if (!IsReservationAmountValid((long)reservation.PlaceID!, parsedAmount.Value))
                        {
                            string validationError = "Invalid number of guests for the selected table.";
                            ControlHelpPresent.DisplayFeedback(validationError, "bottom", "error");
                            return (null, validationError);
                        }

                        return (parsedAmount.Value.ToString(), null);
                    }
                );

                if (string.IsNullOrEmpty(newAmountInput))
                {
                    ControlHelpPresent.DisplayFeedback("Reservation amount not updated.", "bottom", "tip");
                    break;
                }

                reservation.PlaceID = int.Parse(newAmountInput);
                ControlHelpPresent.DisplayFeedback($"Reservation amount updated to {reservation.PlaceID}.", "bottom", "success");
                break;
            }
        });
    }

    // private static bool IsReservationAmountValid(long tableID, int reservationAmount)
    // {
    //     // Check table ID categories and validate reservation amount
    //     if (IsTwoPersonTable(tableID) && (reservationAmount == 1 || reservationAmount == 2))
    //     {
    //         return true;
    //     }
    //     else if (IsFourPersonTable(tableID) && (reservationAmount >= 3 && reservationAmount <= 4))
    //     {
    //         return true;
    //     }
    //     else if (IsSixPersonTable(tableID) && (reservationAmount >= 5 && reservationAmount <= 6))
    //     {
    //         return true;
    //     }

    //     // If none of the conditions are met, return false
    //     return false;
    // }

    private static bool IsReservationAmountValid(long tableID, int reservationAmount)
    {
        return TryCatchHelper.EscapeKeyWithResult(() =>
        {
            bool isValid = IsTwoPersonTable(tableID) && reservationAmount <= 2 ||
                        IsFourPersonTable(tableID) && reservationAmount <= 4 ||
                        IsSixPersonTable(tableID) && reservationAmount <= 6;

            if (!isValid)
            {
                ControlHelpPresent.DisplayFeedback($"Invalid reservation amount ({reservationAmount}) for table {tableID}.", "bottom", "error");
            }

            return isValid;
        });
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
    // private static bool IsTableTaken(DateTime? reservationDate, long tableID)
    // {
    //     var reservations = Access.Reservations.GetAllBy<DateTime?>("Date", reservationDate);

    //     foreach (var res in reservations)
    //     {
    //         if (res.PlaceID == tableID && res.Date != reservationDate) // Ignore the current reservation
    //         {
    //             ControlHelpPresent.DisplayFeedback("This table is already reserved for the selected date. Please choose a different table.", "bottom", "error");
    //             return true; // Table is taken
    //         }
    //     }

    //     return false; // Table is available
    // }

    private static bool IsTableTaken(DateTime? reservationDate, long tableID)
    {
        return TryCatchHelper.EscapeKeyWithResult(() =>
        {
            var reservations = Access.Reservations.GetAllBy<DateTime?>("Date", reservationDate);

            if (reservations.Any(res => res!.PlaceID == tableID && res.Date == reservationDate))
            {
                ControlHelpPresent.DisplayFeedback($"Table {tableID} is already reserved for {reservationDate?.ToString("dd/MM/yyyy")}. Please choose a different table.", "bottom", "error");
                return true; // Table is taken
            }

            return false; // Table is available
        });
    }
}
