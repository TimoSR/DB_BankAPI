using API.Features.Domain;
using FluentAssertions;

namespace UnitTests;

[TestFixture]
public class AccountTests
{
    private Account _account;

    [SetUp]
    public void SetUp()
    {
        _account = new Account
        {
            ID = Guid.NewGuid().ToString(),
            CPR = "1234567890",
            FirstName = "John",
            LastName = "Doe"
        };
    }

    [Test]
    public void InitializeAccount_ValidConditions_ShouldSucceed()
    {
        // Arrange
        var commandId = Guid.NewGuid();

        // Act
        Action act = () => _account.InitializeAccount(commandId);

        // Assert
        act.Should().NotThrow();
        _account.DomainEvents.Should().ContainSingle();
        var domainEvent = (AccountCreatedEvent)_account.DomainEvents[0];
        domainEvent.Should().NotBeNull();
        domainEvent.CommandId.Should().Be(commandId);
        domainEvent.AccountId.Should().Be(_account.ID);
    }

    [Test]
    public void InitializeAccount_InvalidConditions_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var account = new Account();
        var commandId = Guid.NewGuid();

        // Act
        Action act = () => account.InitializeAccount(commandId);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void UpdateBalance_ValidAmount_ShouldUpdateBalance()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var initialBalance = _account.Balance;
        var amount = 500m;

        // Act
        _account.UpdateBalance(commandId, amount);

        // Assert
        _account.Balance.Should().Be(initialBalance + amount);
        _account.DomainEvents.Should().ContainSingle();
        var domainEvent = (BalanceUpdatedEvent)_account.DomainEvents[0];
        domainEvent.CommandId.Should().Be(commandId);
        domainEvent.Amount.Should().Be(amount);
        domainEvent.Amount.Should().Be(initialBalance + amount);
    }

    [Test]
    public void UpdateBalance_ZeroAmount_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var amount = 0m;

        // Act
        Action act = () => _account.UpdateBalance(commandId, amount);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}