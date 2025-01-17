using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace App.Tests;

[TestClass]
public class ControlsHelperPresentTest
{
    public TestContext TestContext { get; set; }
    private MockConsole? mockConsole;

    [TestInitialize]
    public void SetUp()
    {
        mockConsole = new MockConsole();
        ControlHelpPresent.SetConsoleMock(mockConsole);
        ControlHelpPresent.ResetToDefault();
    }

    [TestMethod]
    [DataRow("default")] // Context: Default
    [DataRow("admin")]   // Context: Admin
    [DataRow("user")]    // Context: User
    public void TestClearResetAndShowControls(string menuContext)
    {
        // Arrange: Clear controls and verify it's empty
        ControlHelpPresent.Clear();
        Assert.AreEqual(0, ControlHelpPresent.GetCurrentControls().Count, "Controls are not empty after Clear().");

        // Act: Reset controls to default
        ControlHelpPresent.ResetToDefault();
        var defaultControls = ControlHelpPresent.GetDefaultControls();
        var currentControls = ControlHelpPresent.GetCurrentControls();

        // Verify: Controls are reset to default
        Assert.AreEqual(defaultControls.Count, currentControls.Count, "Default controls count does not match current controls count.");
        foreach (var control in defaultControls)
        {
            Assert.IsTrue(currentControls.ContainsKey(control.Key),
                $"Control {control.Key} is missing after ResetToDefault().");
            Assert.AreEqual(control.Value, currentControls[control.Key],
                $"Control {control.Key} does not have the correct value after ResetToDefault().");
        }

        // Act: Show controls using ShowHelpForTesting
        ControlHelpPresent.ShowHelpForTesting(menuContext: menuContext);

        // Capture output from MockConsole
        var output = mockConsole!.GetOutput();

        // Debug: Log captured output
        TestContext.WriteLine("Captured output:");
        foreach (var line in output)
        {
            TestContext.WriteLine(line);
        }

        // Log controls explicitly on success
        TestContext.WriteLine("Navigation controls after reset:");
        foreach (var control in currentControls)
        {
            TestContext.WriteLine($"Key: {control.Key}, Value: {control.Value}");
        }

        // Assert: Verify each control appears in the output
        foreach (var control in defaultControls)
        {
            string expectedLine = $"Press {control.Value} to {control.Key.ToLower()}.";
            Assert.IsTrue(output.Contains(expectedLine),
                $"Missing control for {control.Key}. Expected line: '{expectedLine}'");
        }
    }

    [TestMethod]
    public void TestControlsShownAtBottom()
    {
        // clear controls and reset to default
        ControlHelpPresent.Clear();
        ControlHelpPresent.ResetToDefault();

        // show help
        ControlHelpPresent.ShowHelpForTesting();

        // capture output and cursor positions
        var output = mockConsole!.GetOutput();
        var cursorPositions = mockConsole.GetCursorPositions();
        
        int footerHeight = ControlHelpPresent.GetFooterHeight();
        int expectedFooterStart = mockConsole.WindowHeight - footerHeight;

        // assert: cursortop should align with the footer
        Assert.AreEqual(expectedFooterStart, cursorPositions.FirstOrDefault(),
        "The starting position of the controls does not match the expected footer position.");

        

    }
}
