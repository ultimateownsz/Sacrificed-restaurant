using App.Presentation.Reservation;
using Restaurant;
using App.Logic.Reservation;
using App.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace App.Tests.Reservation
{
    public class TestConsole : IConsole
    {
        private readonly Queue<string> _inputs = new();
        private readonly List<string> _outputs = new();
        private readonly Queue<ConsoleKeyInfo> _keyInputs = new();

        public void AddInput(string input) => _inputs.Enqueue(input);
        public void AddKeyInput(ConsoleKey key) => _keyInputs.Enqueue(new ConsoleKeyInfo((char)key, key, false, false, false));
        public IEnumerable<string> GetOutputs() => _outputs;

        public string ReadLine() => _inputs.Count > 0 ? _inputs.Dequeue() : string.Empty;
        public ConsoleKeyInfo ReadKey(bool intercept = false) => _keyInputs.Count > 0 ? _keyInputs.Dequeue() : new ConsoleKeyInfo();
        public void WriteLine(string message) => _outputs.Add(message);
        public void Clear() => _outputs.Add("[Screen Cleared]");
    }

    [TestClass]
    public static class ReservationMakePresent
{
    private static ReservationLogic reservationLogic = new();
    private static ReservationMenuLogic reservationMenuLogic = new();
    private static OrderLogic orderLogic = new();

    private static IConsole Console { get; set; } = new ConsoleWrapper();

    public static void SetConsole(IConsole console)
    {
        Console = console;
    }

    public static void MakingReservation(UserModel acc)
    {
        bool isAdmin = acc.Admin.HasValue && acc.Admin.Value == 1;

    START: // Cleanest implementation, sorry

        // Step 1: Ask for the number of guests (only once)
        List<string> options = new() { "1", "2", "3", "4", "5", "6" };
        Console.WriteLine("How many guests will be coming?");
        string guestsInput = Console.ReadLine();

        if (!int.TryParse(guestsInput, out int guests) || guests < 1 || guests > 6)
        {
            Console.WriteLine("Invalid number of guests. Please try again.");
            goto START;
        }

        DateTime selectedDate;

        // Fetch inactive tables
        var inactiveTables = Access.PlaceAccess.Read()
            .Where(p => p.Active == 0)
            .Select(p => p.ID.Value)
            .ToArray();

        while (true) // Loop to manage Calendar -> Table Selection navigation
        {
            Console.WriteLine("Select a date for your reservation:");
            selectedDate = DateTime.Now.AddDays(1); // Mocked date for simplicity

            if (selectedDate == DateTime.MinValue)
                goto START;

            // Step 3: Filter available tables based on the number of guests
            int[] availableTables = guests switch
            {
                1 or 2 => new int[] { 1, 4, 5, 8, 9, 11, 12, 15 },
                3 or 4 => new int[] { 6, 7, 10, 13, 14 },
                5 or 6 => new int[] { 2, 3 },
                _ => Array.Empty<int>()
            };

            Console.WriteLine("Select a table ID:");
            string tableInput = Console.ReadLine();

            if (!int.TryParse(tableInput, out int selectedTable) || !availableTables.Contains(selectedTable))
            {
                Console.WriteLine("Invalid table selection. Please try again.");
                continue;
            }

            // Save the reservation
            if (!acc.ID.HasValue)
            {
                Console.WriteLine("Error: User ID is null. Unable to create reservation.");
                return;
            }

            int reservationId = reservationLogic.SaveReservation(selectedDate, acc.ID.Value, selectedTable);
            if (reservationId == 0)
            {
                Console.WriteLine("Failed to create a reservation. Please try again.");
                continue;
            }

            Console.WriteLine("Reservation created successfully!");
            return;
        }
    }
}
}