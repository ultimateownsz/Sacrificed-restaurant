using App.DataAccess;
using Restaurant;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string themeName = "Nigerian";

            ThemeAccess themeAccess = new ThemeAccess();
            ScheduleAccess scheduleAccess = new ScheduleAccess();

            foreach (var theme in themeAccess.GetAll())
            {
                themeAccess.Delete(theme.ID);
            }
            
            foreach (var schedule in scheduleAccess.GetAll())
            {
                scheduleAccess.Delete(schedule.ID);
            }

            foreach (var theme in themeAccess.GetAll())
            {
                themeAccess.Delete(theme.ID);
            }
            
            themeAccess.Write(new ThemeModel { ID = 1, Name = "Japanese" });
            themeAccess.Write(new ThemeModel { ID = 2, Name = "Italian" });
            themeAccess.Write(new ThemeModel { ID = 3, Name = "British" });
            themeAccess.Write(new ThemeModel { ID = 4, Name = "Turkish" });
            themeAccess.Write(new ThemeModel { ID = 5, Name = "Irish" });
            themeAccess.Write(new ThemeModel { ID = 6, Name = "American" });

            scheduleAccess.Write(new ScheduleModel { ID = 1, Year = 2025, Month = 12, ThemeID = 1 });
            scheduleAccess.Write(new ScheduleModel { ID = 2, Year = 2025, Month = 10, ThemeID = 2 });

            // Act
            ThemeManageLogic.UpdateThemeSchedule(month, year, themeName);

            // Assert: Ensure the new theme and schedule are added
            Assert.AreEqual(7, themeAccess.GetAll().Count, "Themes count mismatch");
            Assert.AreEqual("Nigerian", themeAccess.GetAll().Last().Name, "Theme name mismatch");

            Assert.AreEqual(3, scheduleAccess.GetAll().Count, "Schedules count mismatch");
            var newSchedule = scheduleAccess.GetAll().Last();
            Assert.AreEqual(month, newSchedule.Month, "Month mismatch");
            Assert.AreEqual(year, newSchedule.Year, "Year mismatch");
        }
    }
}
