using API.Features.Domain;
using FluentAssertions;
using Moq;

namespace UnitTests
{
    [TestFixture]
    public class AccountTests
    {
        private Account _account;
        private Guid _requestId;
        private Mock<IAccount> _mockAccount;

        [SetUp]
        public void Setup()
        {
            _account = new Account();
            _requestId = Guid.NewGuid();
            _mockAccount = new Mock<IAccount>();
        }

        [Test]
        public void CreateAccount_WithAllPropertiesSet_ShouldNotThrowException()
        {
            // Arrange
            _account.CPR = "1234567890";
            _account.FirstName = "John";
            _account.LastName = "Doe";

            // Act
            Action act = () => _account.InitializeAccount(_requestId);

            // Assert
            act.Should().NotThrow<InvalidOperationException>();
            _account.DomainEvents.Should().ContainSingle(e => e.GetType() == typeof(AccountCreatedEvent));
        }

        [Test]
        public void CreateAccount_WithMissingProperties_ShouldThrowInvalidOperationException()
        {
            // Arrange
            _account.CPR = "1234567890";
            _account.FirstName = null;  // Missing first name

            // Act
            Action act = () => _account.InitializeAccount(_requestId);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot create account because one or more required properties are not set.");
        }

        [Test]
        public void UpdateBalance_WithNonZeroAmount_ShouldNotThrowException()
        {
            // Arrange
            _account.CPR = "1234567890";
            _account.FirstName = "John";
            _account.LastName = "Doe";
            decimal amount = 100m;

            // Act
            Action act = () => _account.UpdateBalance(_requestId, amount);

            // Assert
            act.Should().NotThrow<InvalidOperationException>();
            _account.Balance.Should().Be(amount);
            _account.DomainEvents.Should().ContainSingle(e => e.GetType() == typeof(BalanceUpdatedEvent));
        }

        [Test]
        public void UpdateBalance_WithZeroAmount_ShouldThrowInvalidOperationException()
        {
            // Arrange
            decimal amount = 0;

            // Act
            Action act = () => _account.UpdateBalance(_requestId, amount);

            // Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot update account balance as the value 0 is not a valid input.");
        }
    }
}
