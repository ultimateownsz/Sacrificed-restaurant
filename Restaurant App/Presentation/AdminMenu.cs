static class AdminMenu
{
    static public void AdminStart()
    {
        System.Console.WriteLine("Welcome to the Admin page!");
        System.Console.WriteLine("1. View all reservations");
        System.Console.WriteLine("2. Filter reservations");
        System.Console.WriteLine("3. Update a reservation");
        System.Console.WriteLine("4. Delete a reservation");
        System.Console.WriteLine("Q. Go back to the main menu");

        string choice = Console.ReadLine().ToLower();
        switch (choice)
        {
            case "1":
                ShowAllReservations();
                break;
            case "2":
                FilterReservations();
                break;
            case "3":
                UpdateReservation();
                break;
            case "4":
                DeleteReservation();
                break;
            case "q":
                Menu.Start();
                break;
            default:
                Console.WriteLine("Invalid input. Try again.");
                AdminStart();
                break;
        }
    }

    static void ShowAllReservations()
    {
        var reservations = ReservationAdminLogic.GetAllReservations();
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found.");
        }
        else
        {
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"ID: {reservation.ID}, Date: {reservation.Date}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");
            }
        }
        AdminStart();
    }

    static void FilterReservations()
    {
        Console.WriteLine("Enter filter criteria:");
        Console.WriteLine("1. Filter by Reservation ID");
        Console.WriteLine("2. Filter by Date");
        Console.WriteLine("3. Filter by User ID");

        string filterChoice = Console.ReadLine();
        List<ReservationModel> filteredReservations = new List<ReservationModel>();

        switch (filterChoice)
        {
            case "1":
                Console.Write("Enter Reservation ID: ");
                if (int.TryParse(Console.ReadLine(), out int reservationID))
                {
                    var reservation = ReservationAdminLogic.GetReservationByID(reservationID);
                    if (reservation != null)
                    {
                        Console.WriteLine($"ID: {reservation.ID}, Date: {reservation.Date}, Table Choice: {reservation.TableChoice}, Number of People: {reservation.ReservationAmount}, UserID: {reservation.UserID}");
                    }
                    else
                    {
                        Console.WriteLine("No reservation found with that ID.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Reservation ID format.");
                }
                break;

            case "2":
                Console.Write("Enter Date (e.g., YYYYMMDD): ");
                if (int.TryParse(Console.ReadLine(), out int date))
                {
                    filteredReservations = ReservationAdminLogic.GetReservationsByDate(date);
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
                break;

            case "3":
                Console.Write("Enter User ID: ");
                if (int.TryParse(Console.ReadLine(), out int userID))
                {
                    filteredReservations = ReservationAdminLogic.GetReservationsByUserID(userID);
                }
                else
                {
                    Console.WriteLine("Invalid User ID format.");
                }
                break;

            default:
                Console.WriteLine("Invalid choice.");
                FilterReservations();
                return;
        }

        if (filteredReservations.Count == 0)
        {
            Console.WriteLine("No reservations found matching the criteria.");
        }
        else
        {
            Console.WriteLine("Filtered Reservations:");
            foreach (var res in filteredReservations)
            {
                Console.WriteLine($"ID: {res.ID}, Date: {res.Date}, Table Choice: {res.TableChoice}, Number of People: {res.ReservationAmount}, UserID: {res.UserID}");
            }
        }
        AdminStart();
    }

    static void UpdateReservation()
    {
        Console.Write("Enter the Reservation ID you want to update: ");
        if (int.TryParse(Console.ReadLine(), out int reservationID))
        {
            var reservation = ReservationAdminLogic.GetReservationByID(reservationID);

            if (reservation != null)
            {
                Console.Write("Enter new Date (current: {0}): ", reservation.Date);
                if (int.TryParse(Console.ReadLine(), out int newDate))
                {
                    Console.Write("Enter new Table Choice (current: {0}): ", reservation.TableChoice);
                    if (int.TryParse(Console.ReadLine(), out int newTableChoice))
                    {
                        Console.Write("Enter new Number of People (current: {0}): ", reservation.ReservationAmount);
                        if (int.TryParse(Console.ReadLine(), out int newAmount))
                        {
                            reservation.Date = newDate;
                            reservation.TableChoice = newTableChoice;
                            reservation.ReservationAmount = newAmount;

                            ReservationAdminLogic.UpdateReservation(reservation);
                            Console.WriteLine("Reservation updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Number of People format.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Table Choice format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Date format.");
                }
            }
            else
            {
                Console.WriteLine("Reservation not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Reservation ID format.");
        }
        AdminStart();
    }

    static void DeleteReservation()
    {
        Console.Write("Enter the Reservation ID you want to delete: ");
        if (int.TryParse(Console.ReadLine(), out int reservationID))
        {
            var reservation = ReservationAdminLogic.GetReservationByID(reservationID);
            if (reservation != null)
            {
                ReservationAdminLogic.DeleteReservation(reservationID);
                Console.WriteLine("Reservation deleted successfully.");
            }
            else
            {
                Console.WriteLine("Reservation not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Reservation ID format.");
        }
        AdminStart();
    }
}
