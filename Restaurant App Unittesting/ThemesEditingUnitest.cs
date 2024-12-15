using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ThemeViewTests
{
    [TestMethod]
    public void TestAddThemes()
    {
        // Arrange the Models(the models are the end of this file)
        var themeName = "Turkish";
        var schedule = new Schedule { Year = 2025, Month = 6 };

        // Act
        string result = SimulateAddTheme(user, schedule, themeName);

        // Assert
        Assert.AreEqual("Theme 'Turkish' has been added for June 2025.", result);
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
