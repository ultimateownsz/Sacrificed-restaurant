public static class DeleteReservation
{
    public static void Show()
    {
        // Flag to control when the user has entered valid input
        bool isValid = false;

        // Loop until valid input is provided
        while (!isValid)
        {
            Console.WriteLine("");
            Console.Write("Enter the Reservation ID you want to delete (or Q to go back): ");
            string? input = Console.ReadLine()?.ToLower();

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
                    // Delete the reservation from the system
                    ReservationAdminLogic.DeleteReservation(reservationID);
                    Console.WriteLine("Reservation deleted successfully.");
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
        }

        Show();
    }
}
