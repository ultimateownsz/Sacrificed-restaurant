using App.Presentation.Reservation;
using Xunit;
using Moq;
using System;

namespace App.Tests
{
    public class ReservationMakePresentTests
    {
        [Fact]
        public void MakingReservation_ShouldHandleInvalidAdmin()
        {
            // Arrange
            var user = new UserModel { ID = 1, Admin = null }; // Non-admin user

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReservationMakePresent.MakingReservation(user));
        }

        [Fact]
        public void TakeOrders_ShouldReturnEmptyList_WhenReservationIdIsInvalid()
        {
            // Act
            var result = ReservationMakePresent.TakeOrders(DateTime.Now, new UserModel { ID = 1 }, 0, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void PrintReceipt_ShouldHandleInvalidReservation()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                ReservationMakePresent.PrintReceipt(new List<ProductModel>(), 0, new UserModel { ID = 1 }));
        }
    }
}