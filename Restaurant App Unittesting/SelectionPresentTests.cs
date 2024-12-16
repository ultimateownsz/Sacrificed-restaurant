using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Project.Tests
{
    [TestClass]
    public class SelectionMockTests
    {
        [TestMethod]
        public void TestSelectionMock_DownArrowAndEnter()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            var inputSequence = new List<ConsoleKey> { ConsoleKey.DownArrow, ConsoleKey.Enter };

            // Act
            dynamic result = SelectionMock.SimulateSelection(options, inputSequence);

            // Assert
            Assert.AreEqual("Option 2", result.text);
            Assert.AreEqual(1, result.index);
        }

        [TestMethod]
        public void TestSelectionMock_UpArrowAndEnter()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            var inputSequence = new List<ConsoleKey> { ConsoleKey.UpArrow, ConsoleKey.Enter };

            // Act
            dynamic result = SelectionMock.SimulateSelection(options, inputSequence);

            // Assert
            Assert.AreEqual("Option 3", result.text); // Wraps to last option
            Assert.AreEqual(2, result.index);
        }

        [TestMethod]
        public void TestSelectionMock_EscapeKey()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            var inputSequence = new List<ConsoleKey> { ConsoleKey.Escape };

            // Act
            dynamic result = SelectionMock.SimulateSelection(options, inputSequence);

            // Assert
            Assert.IsNull(result.text);
            Assert.AreEqual(-1, result.index);
        }

        [TestMethod]
        public void TestSelectionMock_ImmediateEnter()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            var inputSequence = new List<ConsoleKey> { ConsoleKey.Enter };

            // Act
            dynamic result = SelectionMock.SimulateSelection(options, inputSequence);

            // Assert
            Assert.AreEqual("Option 1", result.text);
            Assert.AreEqual(0, result.index);
        }

        [TestMethod]
        public void TestSelectionMock_MultipleDownArrowsWrapAround()
        {
            // Arrange
            var options = new List<string> { "Option 1", "Option 2", "Option 3" };
            var inputSequence = new List<ConsoleKey> { ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter };

            // Act
            dynamic result = SelectionMock.SimulateSelection(options, inputSequence);

            // Assert
            Assert.AreEqual("Option 1", result.text); // Wraps back to the first option
            Assert.AreEqual(0, result.index);
        }
    }

    public static class SelectionMock
    {
        /// <summary>
        /// Simulates the SelectionPresent.Show behavior for unit testing.
        /// </summary>
        /// <param name="options">List of selectable options.</param>
        /// <param name="inputSequence">Sequence of input actions (Up, Down, Enter, Escape).</param>
        /// <returns>A dynamic object containing the selected text and index.</returns>
        public static dynamic SimulateSelection(List<string> options, List<ConsoleKey> inputSequence)
        {
            int currentIndex = 0; // Start at the first option

            foreach (var key in inputSequence)
            {
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        currentIndex = (currentIndex + 1) % options.Count; // Navigate down and wrap
                        break;

                    case ConsoleKey.UpArrow:
                        currentIndex = (currentIndex - 1 + options.Count) % options.Count; // Navigate up and wrap
                        break;

                    case ConsoleKey.Enter:
                        return CreateDynamicResult(options[currentIndex], currentIndex);

                    case ConsoleKey.Escape:
                        return CreateDynamicResult(null, -1); // Escape key results in null selection

                    default:
                        throw new InvalidOperationException($"Unhandled key: {key}");
                }
            }

            // Default case (no selection made)
            return CreateDynamicResult(null, -1);
        }

        /// <summary>
        /// Helper to create dynamic selection result.
        /// </summary>
        private static dynamic CreateDynamicResult(string? text, int index)
        {
            dynamic result = new System.Dynamic.ExpandoObject();
            result.text = text;
            result.index = index;
            return result;
        }
    }
}
