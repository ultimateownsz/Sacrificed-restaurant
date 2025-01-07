using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Restaurant_App_Unittesting
{
    // Unit Tests for deleting reservations as a user
    [TestClass]
    public class UserDeleteReservationTest
    {
        // Unit Test for criteria: User selects "Yes" on the delete confirmation
        [TestMethod]
        [DataRow(1, "Yes", true)] // Reservation exists and is deleted
        [DataRow(2, "Yes", false)] // Reservation does not exist and is not deleted
        public void TestDeleteReservation_ConfirmYes(int reservationId, string userConfirmation, bool expectedResult)
        {

        }

        // Unit Test for Criteria: User selects "No" on the delete confirmation
        [TestMethod]
        [DataRow(1, "No", false)]  // Reservation exists but user cancels
        [DataRow(2, "No", false)]  // Reservation does not exist and is not deleted
        public void TestDeleteReservation_ConfirmNo(int reservationId, string userConfirmation, bool expectedResult)
        {

        }
    }
}