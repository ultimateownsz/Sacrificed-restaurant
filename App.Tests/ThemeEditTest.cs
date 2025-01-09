namespace App.Test;
using System.Text.RegularExpressions;

[TestClass]
public class ThemeEditTest
{
    [TestMethod]
    public void TestAddThemes()
    {
        // Arrange the Models(the models are the end of this file)
        var themeName = "Turkish";
        var schedule = new Schedule { Year = 2025, Month = 6 };

        // Act
        
        string result = SimulateAddTheme(schedule, themeName);

        // Assert
        Assert.AreEqual("Theme 'Turkish' has been added for June 2025.", result);
    }

    [TestMethod]
    public void TestEditThemes()
    {
        var existingTheme = new Theme { ID = 101, ThemeName = "Turkish" };
        var schedule = new Schedule { Year = 2025, Month = 7, ThemeID = 101 };
        var newThemeName = "Japanese";

        string result = SimulateEditTheme(existingTheme, schedule, newThemeName);

        Assert.AreEqual("Theme updated to 'Japanese' for July 2025.", result);
    }

    [TestMethod]
    public void TestDeleteThemes()
    {
        var themeToDelete = new Theme { ID = 102, ThemeName = "Japanese" };
        var schedule = new Schedule { Year = 2025, Month = 12, ThemeID = 102 };

        string result = SimulateDeleteTheme(themeToDelete, schedule);

        Assert.AreEqual("Theme 'Japanese' has been deleted for July 2025.", result);
    }

    [TestMethod]
    public void TestValidateThemeName()
    {
        string validThemeName = "Valid Theme";
        string invalidThemeName = "Invalid123!";

        bool isValid1 = ValidateThemeName(validThemeName);
        bool isValid2 = ValidateThemeName(invalidThemeName);

        Assert.IsTrue(isValid1, "Expected the theme name to be valid.");
        Assert.IsFalse(isValid2, "Expected the theme name to be invalid.");
    }

    // Simulated Methods for Testing
    //This Method simulates adding themes
    private string SimulateAddTheme(Schedule schedule, string themeName)
    {
        //calls ValidateThemeName to check if it has symbols in it
        if (!ValidateThemeName(themeName))
            return "Invalid theme name. Only letters and spaces are allowed.";

        return $"Theme '{themeName}' has been added for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //This Method simulates editing themes
    private string SimulateEditTheme(Theme existingTheme, Schedule schedule, string newThemeName)
    {
        if (!ValidateThemeName(newThemeName))
            return "Invalid theme name. Only letters and spaces are allowed.";

        existingTheme.ThemeName = newThemeName;
        return $"Theme updated to '{newThemeName}' for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //This Method simulates Deleting themes
    private string SimulateDeleteTheme(Theme themeToDelete, Schedule schedule)
    {
        return $"Theme '{themeToDelete.ThemeName}' has been deleted for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    //Validates the name of the theme
    private bool ValidateThemeName(string themeName)
    {
        return Regex.IsMatch(themeName, "^[A-Za-z ]+$"); //This line(FROM GOOGLE NOT GPT) validates if the name has anything other than letters or spaces, then proceeds to return true/false
    }

    // gets the month name
    private string GetMonthName(int month)
    {
        return new DateTime(1, month, 1).ToString("MMMM");
    }

    // Supporting Classes
    public class Theme
    {
        public int ID { get; set; }
        public string ThemeName { get; set; }
    }

    public class Schedule
    {
        public int ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int ThemeID { get; set; }
    }
}
