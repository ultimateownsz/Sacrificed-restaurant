using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ThemeViewTests
{


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
