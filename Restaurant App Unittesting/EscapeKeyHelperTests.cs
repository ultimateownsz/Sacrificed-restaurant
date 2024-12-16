using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class EscapeKeyHelperTests
    {
        // Test Escape key behavior (null input)
        [TestMethod]
        public void TestInputHelper_EscapeKeyPressed()
        {
            // Arrange
            string input = null; // Simulate Escape key behavior

            // Act
            var result = InputHelper.GetValidatedInput<string>(input, input =>
                (input, null) // Dummy validator
            );

            // Assert
            Assert.IsNull(result);
        }

        // Test valid input (no escape key)
        [TestMethod]
        public void TestInputHelper_ValidInput()
        {
            // Arrange: Simulate user input
            string input = "ValidInput";

            // Act
            var result = InputHelper.GetValidatedInput<string>(
                input,
                input => !string.IsNullOrWhiteSpace(input) ? (input, null) : (null, "Invalid input")
            );

            // Assert
            Assert.AreEqual("ValidInput", result);
        }

        // Test invalid input
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInputHelper_InvalidInput()
        {
            // Arrange: Simulate invalid input
            string input = "";

            // Act: Expect exception
            InputHelper.GetValidatedInput<string>(
                input,
                input => !string.IsNullOrWhiteSpace(input) ? (input, null) : (null, "Input cannot be empty")
            );
        }

        public static class InputHelper
        {
            public static T GetValidatedInput<T>(string prompt, Func<string, (T? result, string? error)> validateAndParse)
            {
                var (result, error) = validateAndParse(prompt);
                if (error != null) throw new ArgumentException(error);
                return result!;
            }
        }
    }
}
