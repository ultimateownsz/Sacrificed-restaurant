using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ThemeViewTests
{
    // Simulated Methods for Testing
    private string SimulateAddTheme(Schedule schedule, string themeName)
    {
        if (!ValidateThemeName(themeName))
            return "Invalid theme name. Only letters and spaces are allowed.";

        return $"Theme '{themeName}' has been added for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    private string SimulateEditTheme(Theme existingTheme, Schedule schedule, string newThemeName)
    {
        if (!ValidateThemeName(newThemeName))
            return "Invalid theme name. Only letters and spaces are allowed.";

        existingTheme.ThemeName = newThemeName;
        return $"Theme updated to '{newThemeName}' for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    private string SimulateDeleteTheme(Theme themeToDelete, Schedule schedule)
    {
        return $"Theme '{themeToDelete.ThemeName}' has been deleted for {GetMonthName(schedule.Month)} {schedule.Year}.";
    }

    private bool ValidateThemeName(string themeName)
    {
        return Regex.IsMatch(themeName, "^[A-Za-z ]+$");
    }

    // Utility Method for Month Name
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
