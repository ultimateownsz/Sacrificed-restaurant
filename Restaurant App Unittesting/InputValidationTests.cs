using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class InputValidationTests
    {
        // Test valid input
        [TestMethod]
        [DataRow("12345678", true)] // Valid input
        [DataRow("87654321", true)] // Another valid input
        public void TestInputHelper_ValidInput(string input, bool expectedResult)
        {
            // Arrange
            bool isValid = true;

            try
            {
                // Act
                var result = InputHelper.GetValidatedInput<string>(
                    input,
                    input => input.Length == 8 && int.TryParse(input, out _) 
                        ? (input, null) 
                        : (null, "Invalid input")
                );
            }
            catch (ArgumentException)
            {
                isValid = false;
            }

            // Assert
            Assert.AreEqual(expectedResult, isValid);
        }

        // Test invalid input
        [TestMethod]
        [DataRow("123", false)]         // Too short
        [DataRow("abcdefgh", false)]    // Non-numeric
        [DataRow("", false)]            // Empty string
        [DataRow(null, false)]          // Null input
        public void TestInputHelper_InvalidInput(string input, bool expectedResult)
        {
            // Arrange
            bool isValid = true;

            try
            {
                // Act
                var result = InputHelper.GetValidatedInput<string>(
                    input,
                    input => !string.IsNullOrEmpty(input) && input.Length == 8 && int.TryParse(input, out _)
                        ? (input, null)
                        : (null, "Invalid input")
                );
            }
            catch (ArgumentException)
            {
                isValid = false;
            }

            // Assert
            Assert.AreEqual(expectedResult, isValid);
        }

        // Supporting InputHelper
        public static class InputHelper
        {
            public static T GetValidatedInput<T>(string input, Func<string, (T? result, string? error)> validateAndParse)
            {
                if (string.IsNullOrEmpty(input))
                    throw new ArgumentException("Input cannot be null or empty.");

                var (result, error) = validateAndParse(input);
                if (error != null) throw new ArgumentException(error);
                return result!;
            }
        }
    }
}
