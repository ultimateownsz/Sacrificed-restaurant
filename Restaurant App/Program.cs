using System;
using Presentation;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Reservation System");
        Console.WriteLine("Starting Calendar Navigation...");
        
        // Start the calendar navigation to test the calendar and reservation functionality
        // MakingReservations.CalendarNavigation();
        AdminMenu.AdminStart();
        
        Console.WriteLine("Thank you for using the Reservation System. Goodbye!");
    }
}