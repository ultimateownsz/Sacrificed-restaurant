using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Restaurant_App_Unittesting
{
    [TestClass]
    public class InputValidationTests
    {
        // Test valid input
        [TestMethod]
        public void TestInputHelper_ValidInput()
        {
            // Arrange
            string input = "12345678";

            // Act
            var result = InputHelper.GetValidatedInput<string>(input, input =>
                input.Length == 8 ? (input, null) : (null, "Invalid length")
            );

            // Assert
            Assert.AreEqual("12345678", result);
        }

        // Test invalid input
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInputHelper_InvalidInput()
        {
            // Arrange
            string input = "123";

            // Act
            InputHelper.GetValidatedInput<string>(input, input =>
                input.Length == 8 ? (input, null) : (null, "Invalid length")
            );

            // Exception expected
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