using NUnit.Framework;
using FluentAssertions;
using Moq;
using API.Features.Domain;

namespace API.Tests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void Constructor_ValidParameters_ShouldCreateTransaction()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var accountId = "123456789";
            var amount = 100m;

            // Act
            var transaction = new Transaction(id, accountId, amount);

            // Assert
            transaction.Should().NotBeNull();
            transaction.Id.Should().Be(id);
            transaction.AccountId.Should().Be(accountId);
            transaction.Amount.Should().Be(amount);
            transaction.Time.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Constructor_NullAccountId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            string accountId = null;
            var amount = 100m;

            // Act
            Action act = () => new Transaction(id, accountId, amount);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_EmptyAccountId_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var accountId = "";
            var amount = 100m;

            // Act
            Action act = () => new Transaction(id, accountId, amount);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var accountId = "123456789";
            var amount = 0m;

            // Act
            Action act = () => new Transaction(id, accountId, amount);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void InitializeTransaction_ValidCommandId_ShouldAddDomainEvent()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var accountId = "123456789";
            var amount = 100m;
            var transaction = new Transaction(id, accountId, amount);
            var commandId = Guid.NewGuid();

            // Act
            transaction.InitializeTransaction(commandId);

            // Assert
            transaction.DomainEvents.Should().ContainSingle();
            transaction.DomainEvents[0].Should().BeOfType<TransactionCreatedEvent>();
            var @event = (TransactionCreatedEvent)transaction.DomainEvents[0];
            @event.CommandId.Should().Be(commandId);
            @event.TransactionId.Should().Be(id);
            @event.AccountId.Should().Be(accountId);
            @event.Amount.Should().Be(amount);
            @event.CompletionTime.Should().Be(transaction.Time);
        }
    }
}
