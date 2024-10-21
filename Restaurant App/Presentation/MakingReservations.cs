static class MakingReservations
{

    static private ReservationLogic reservationLogic = new ReservationLogic();

    public static void Start()//This function gets called through the menu to ask the user for his reservation information
    {
        //Ask the date of the reservation
        Console.WriteLine("Welcome to the reservation page");
        Console.WriteLine("1. Make reservation");
        Console.WriteLine("2. Remove reservation");
        string input = Console.ReadLine().ToLower();
        if  (input == "1" || input == "remove reservation")
        {
            Console.WriteLine("Please enter your desired date d/m/y"); 
            string date = Console.ReadLine();

            //Ask the user for the table of choice
            Console.WriteLine("Please enter your desired table choice\n1.Table for two\n2.Table for four\n3.Table for six");
            string tableChoice = Console.ReadLine();

            //Ask the user for the reservation amount
            Console.WriteLine("Please enter the number of guests");
            string reservationAmount = Console.ReadLine();

            reservationLogic.SaveReservation(date, tableChoice, reservationAmount);
        }
        else
        {
            Console.WriteLine("Please enter the reservation code(id)");
            int removeID = Convert.ToInt32(Console.ReadLine());
            reservationLogic.RemoveReservation(removeID);
            Console.WriteLine("The reservation has been cancelled");
        }
    }
}