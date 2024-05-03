using API.Features.Domain;
using FluentAssertions;

// Ensure this is correctly using your namespace

namespace UnitTests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void Account_Should_Initialize_With_Correct_Values()
        {
            // Arrange
            var cpr = "123456-7890";
            var firstName = "John";
            var lastName = "Doe";
            var balance = 0;

            // Act
            var account = new Account
            {
                CPR = cpr,
                FirstName = firstName,
                LastName = lastName,
            };

            // Assert
            account.CPR.Should().Be(cpr, because: "CPR should be set correctly during initialization.");
            account.FirstName.Should().Be(firstName, because: "FirstName should be set correctly during initialization.");
            account.LastName.Should().Be(lastName, because: "LastName should be set correctly during initialization.");
            account.Balance.Should().Be(balance, because: "Balance should be set correctly and be immutable after initialization.");
        }
    }
}