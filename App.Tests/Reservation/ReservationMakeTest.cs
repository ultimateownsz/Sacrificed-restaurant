using App.Presentation.Reservation;
using Restaurant;
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
    public class ReservationMakeTest
    {
        private TestConsole _testConsole;
        private ReservationMakePresent _reservationMakePresent;

        [TestInitialize]
        public void Setup()
        {
            _testConsole = new TestConsole();
            ReservationMakePresent.SetConsole(_testConsole);
        }

        [TestMethod]
        public void MakingReservation_ShouldHandleInvalidGuestInput()
        {
            // Arrange
            var user = new UserModel { ID = 1, Admin = 1 };
            _testConsole.AddInput("invalid"); // Invalid input
            _testConsole.AddInput("7");      // Out-of-range input
            _testConsole.AddInput("3");     // Valid input

            // Act
            ReservationMakePresent.MakingReservation(user);

            // Assert
            var outputs = _testConsole.GetOutputs();
            Assert.IsTrue(outputs.Contains("Invalid number of guests. Please try again."));
            Assert.IsTrue(outputs.Contains("How many guests will be coming?"));
        }

        [TestMethod]
        public void MakingReservation_ShouldHandleSuccessfulReservation()
        {
            // Arrange
            var user = new UserModel { ID = 1, Admin = 1 };
            _testConsole.AddInput("2"); // Number of guests
            _testConsole.AddInput("1"); // Table ID

            // Act
            ReservationMakePresent.MakingReservation(user);

            // Assert
            var outputs = _testConsole.GetOutputs();
            Assert.IsTrue(outputs.Contains("How many guests will be coming?"));
            Assert.IsTrue(outputs.Contains("Select a date for your reservation:"));
            Assert.IsTrue(outputs.Contains("Select a table ID:"));
        }

        [TestMethod]
        public void MakingReservation_ShouldHandleNullUserId()
        {
            // Arrange
            var user = new UserModel { ID = null, Admin = 1 };
            _testConsole.AddInput("2"); // Number of guests

            // Act
            ReservationMakePresent.MakingReservation(user);

            // Assert
            var outputs = _testConsole.GetOutputs();
            Assert.IsTrue(outputs.Contains("Error: User ID is null."));
        }
    }
}