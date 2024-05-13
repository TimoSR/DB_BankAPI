using API.Features.Domain;
using FluentAssertions;

namespace UnitTests;

[TestFixture]
    public class TransactionFactoryTests
    {
        private TransactionFactory _transactionFactory;

        [SetUp]
        public void Setup()
        {
            _transactionFactory = new TransactionFactory();
        }

        [Test]
        public void CreateTransaction_ValidParameters_ShouldCreateAndInitializeTransaction()
        {
            // Arrange
            var commandId = Guid.NewGuid();
            var accountId = "123456789";
            var amount = 100m;

            // Act
            var transaction = _transactionFactory.CreateTransaction(commandId, accountId, amount);

            // Assert
            transaction.Should().NotBeNull();
            transaction.AccountId.Should().Be(accountId);
            transaction.Amount.Should().Be(amount);
            transaction.DomainEvents.Should().ContainSingle();

            var domainEvent = (TransactionCreatedEvent)transaction.DomainEvents[0];
            domainEvent.Should().NotBeNull();
            domainEvent.CommandId.Should().Be(commandId);
            domainEvent.TransactionId.Should().Be(transaction.Id);
            domainEvent.AccountId.Should().Be(accountId);
            domainEvent.Amount.Should().Be(amount);
            domainEvent.CompletionTime.Should().Be(transaction.Time);
        }

        [Test]
        public void CreateTransaction_InvalidAccountId_ShouldThrowArgumentException()
        {
            // Arrange
            var commandId = Guid.NewGuid();
            string accountId = null;
            var amount = 100m;

            // Act
            Action act = () => _transactionFactory.CreateTransaction(commandId, accountId, amount);

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void CreateTransaction_ZeroAmount_ShouldThrowArgumentException()
        {
            // Arrange
            var commandId = Guid.NewGuid();
            var accountId = "123456789";
            var amount = 0m;

            // Act
            Action act = () => _transactionFactory.CreateTransaction(commandId, accountId, amount);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }