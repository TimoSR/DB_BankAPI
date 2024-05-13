using API.Features.Domain;
using FluentAssertions;

namespace UnitTests;

[TestFixture]
public class TransactionCreatedEventTests
{
    [Test]
    public void Constructor_WhenCalled_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var transactionId = "tx123";
        var accountId = "acc456";
        var amount = 100m;
        var completionTime = new DateTime(2023, 5, 1, 14, 30, 0);

        // Act
        var eventObj = new TransactionCreatedEvent(commandId, transactionId, accountId, amount, completionTime);

        // Assert
        eventObj.CommandId.Should().Be(commandId);
        eventObj.TransactionId.Should().Be(transactionId);
        eventObj.AccountId.Should().Be(accountId);
        eventObj.Amount.Should().Be(amount);
        eventObj.CompletionTime.Should().Be(completionTime);
    }

    [Test]
    public void Message_WhenCalled_ShouldReturnCorrectFormat()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var transactionId = "tx123";
        var accountId = "acc456";
        var amount = 100m;
        var completionTime = new DateTime(2023, 5, 1, 14, 30, 0);
        var eventObj = new TransactionCreatedEvent(commandId, transactionId, accountId, amount, completionTime);
        var expectedMessage = $"Command {commandId} Created transaction {transactionId} for account {accountId} with {amount} at {completionTime:yyyy-MM-dd HH:mm:ss} (UTC).";

        // Act
        var message = eventObj.Message;

        // Assert
        message.Should().Be(expectedMessage);
    }
}