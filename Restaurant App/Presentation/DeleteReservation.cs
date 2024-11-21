public static class DeleteReservation
{
    public static void Show()
    {
        // Flag to control when the user has entered valid input
        bool isValid = false;

        // Loop until valid input is provided
        while (!isValid)
        {
            Console.Clear();
            Console.Write("(Q)uit or reservation code: ");
            string input = Console.ReadLine().ToLower();

            if (input == "q")
            {
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
                    Console.Clear();
                    Console.WriteLine("Reservation deleted successfully.");
                    Console.Write("\nPress enter to continue..."); Console.ReadKey();
                    return;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Reservation not found. Try again.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid Reservation ID format. Please try again.");
            }

            Console.Write("\nPress enter to continue..."); Console.ReadKey();
        }
    }
}
