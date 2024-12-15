using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ShowReservationTests
{
    

    // Supporting Classes
    public class UserModel
    {
        public int ID { get; set; }
        public int? Admin { get; set; }
        public string FirstName { get; set; }
    }

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