using App.DataAccess;
using App.DataModels;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using App.Logic.Theme;


namespace App.Tests
{
    [TestClass]
    public class ThemeEditTest
    {

        [TestMethod]
        public void Test_UpdateThemeSchedule_NewScheduleAndTheme()
        {
            // Arrange
            int month = 3;
            int year = 2025;
            string themeName = "Turkish";

            // Simulate in-memory data
            Access.Schedules = new List<ScheduleModel>
            {
                new ScheduleModel { ID = 1, Year = 2025, Month = 12, ThemeID = 2 }
            };

            Access.Themes = new List<ThemeModel>
            {
                new ThemeModel { ID = 1, Name = "Japanese" }
            };

            // Act
            ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);
            Console.WriteLine("Access.Themes.Count: " + Access.Themes.Count);

            // Assert: Ensure the new theme and schedule are added
            Assert.AreEqual(2, Access.Themes.Count);
            Assert.AreEqual("Turkish", Access.Themes.Last().Name);

            Assert.AreEqual(2, Access.Schedules.Count);
            var newSchedule = Access.Schedules.Last();
            Assert.AreEqual(month, newSchedule.Month);
            Assert.AreEqual(year, newSchedule.Year);
            Assert.AreEqual(2, newSchedule.ThemeID); // Assuming theme ID increments
        }

        [TestMethod]
        public void Test_UpdateThemeSchedule_ExistingScheduleAndTheme()
        {
            // Arrange
            int month = 3;
            int year = 2025;
            string themeName = "Italian";

            // Simulate in-memory data
            Access.Schedules = new List<ScheduleModel>
            {
                new ScheduleModel { ID = 1, Year = year, Month = month, ThemeID = 1 }
            };

            Access.Themes = new List<ThemeModel>
            {
                new ThemeModel { ID = 1, Name = "Italian" }
            };

            // Act
            ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);

            // Assert: Ensure no new data is added and existing schedule is updated
            Assert.AreEqual(1, Access.Themes.Count);
            Assert.AreEqual(1, Access.Schedules.Count);

            var existingSchedule = Access.Schedules.First();
            Assert.AreEqual(1, existingSchedule.ThemeID);
        }
        public static class Access
        {
            public static List<ScheduleModel> Schedules { get; set; }
            public static List<ThemeModel> Themes { get; set; }
        }
    }
}

    //     [TestMethod]
    //     public void TestEditThemes()
    //     {
    //         var existingTheme = new Theme { ID = 101, ThemeName = "Turkish" };
    //         var schedule = new Schedule { Year = 2025, Month = 7, ThemeID = 101 };
    //         var newThemeName = "Japanese";

    //         string result = SimulateEditTheme(existingTheme, schedule, newThemeName);

    //         Assert.AreEqual("Theme updated to 'Japanese' for July 2025.", result);
    //     }

    //     [TestMethod]
    //     public void TestDeleteThemes()
    //     {
    //         var themeToDelete = new Theme { ID = 102, ThemeName = "Japanese" };
    //         var schedule = new Schedule { Year = 2025, Month = 12, ThemeID = 102 };

    //         string result = SimulateDeleteTheme(themeToDelete, schedule);

    //         Assert.AreEqual("Theme 'Japanese' has been deleted for July 2025.", result);
    //     }

    //     [TestMethod]
    //     public void TestValidateThemeName()
    //     {
    //         string validThemeName = "Valid Theme";
    //         string invalidThemeName = "Invalid123!";

    //         bool isValid1 = ValidateThemeName(validThemeName);
    //         bool isValid2 = ValidateThemeName(invalidThemeName);

    //         Assert.IsTrue(isValid1, "Expected the theme name to be valid.");
    //         Assert.IsFalse(isValid2, "Expected the theme name to be invalid.");
    //     }
    // }
