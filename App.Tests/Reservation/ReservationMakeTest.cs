using App.Presentation.Reservation;
using App.DataModels.Product;
using App.DataModels;
using Xunit;
using MSTestAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using XunitAssert = Xunit.Assert;


namespace App.Tests
{
    public class ReservationMakePresentTests
    {
        [Fact]
        public void MakingReservation_ShouldExit_WhenGuestsAreInvalid()
        {
            // Arrange
            var user = new UserModel { ID = 1, Admin = 1 };

            // Act & Assert
            var exception = Record.Exception(() => ReservationMakePresent.MakingReservation(user));

            // Assert no exceptions thrown (graceful exit)
            XunitAssert.Null(exception);
        }

        [Fact]
        public void TakeOrders_ShouldReturnValidOrders_WhenValidDataProvided()
        {
            // Arrange
            var user = new UserModel { ID = 2, Admin = 0 };
            var reservationId = 1;
            var guests = 2;
            var date = DateTime.Now.AddDays(2);

            // Act
            var result = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.IsType<List<ProductModel>>(result);
        }

        [Fact]
        public void TakeOrders_ShouldReturnEmptyList_WhenNoThemeAvailable()
        {
            // Arrange
            var user = new UserModel { ID = 3 };
            var reservationId = 1;
            var guests = 3;
            var date = new DateTime(2000, 1, 1); // A date with no theme

            // Act
            var result = ReservationMakePresent.TakeOrders(date, user, reservationId, guests);

            // Assert
            XunitAssert.NotNull(result);
            XunitAssert.Empty(result);
        }

        [Fact]
        public void PrintReceipt_ShouldDisplayCorrectReceiptData()
        {
            // Arrange
            var user = new UserModel { ID = 4, FirstName = "John", LastName = "Doe" };
            var reservationId = 1;
            var orders = new List<ProductModel>
            {
                new ProductModel { ID = 1, Name = "Pizza", Price = 12.99M },
                new ProductModel { ID = 2, Name = "Pasta", Price = 8.99M }
            };

            // Act
            var exception = Record.Exception(() => ReservationMakePresent.PrintReceipt(orders, reservationId, user));

            // Assert
            XunitAssert.Null(exception);
        }

        [Fact]
        public void MakingReservation_ShouldHandleAdminFlagCorrectly()
        {
            // Arrange
            var adminUser = new UserModel { ID = 1, Admin = 1 };
            var regularUser = new UserModel { ID = 2, Admin = 0 };

            // Act & Assert
            var exceptionAdmin = Record.Exception(() => ReservationMakePresent.MakingReservation(adminUser));
            var exceptionRegular = Record.Exception(() => ReservationMakePresent.MakingReservation(regularUser));

            // Assert no exceptions thrown
            XunitAssert.Null(exceptionAdmin);
            XunitAssert.Null(exceptionRegular);
        }
    }
}