using API.Features.Domain;
using FluentAssertions;

namespace UnitTests;

[TestFixture]
public class DomainEventTests
{
    [Test]
    public void AccountCreatedEvent_ConstructedCorrectly_ShouldSetPropertiesAndGenerateCorrectMessage()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var accountId = Guid.NewGuid().ToString();
        var completionTime = DateTime.UtcNow;

        // Act
        var eventInstance = new AccountCreatedEvent(requestId, accountId, completionTime);

        // Assert
        eventInstance.CommandId.Should().Be(requestId);
        eventInstance.AccountId.Should().Be(accountId);
        eventInstance.CompletionTime.Should().Be(completionTime);
        eventInstance.Message.Should().Be($"Request {requestId} Created Account {accountId} at {completionTime:yyyy-MM-dd HH:mm:ss} (UTC).");
    }

    [Test]
    public void BalanceUpdatedEvent_ConstructedCorrectly_ShouldSetPropertiesAndGenerateCorrectMessage()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var accountId = Guid.NewGuid().ToString();
        var amount = 150.75m;
        var completionTime = DateTime.UtcNow;

        // Act
        var eventInstance = new BalanceUpdatedEvent(requestId, accountId, amount, completionTime);

        // Assert
        eventInstance.CommandId.Should().Be(requestId);
        eventInstance.AccountId.Should().Be(accountId);
        eventInstance.Amount.Should().Be(amount);
        eventInstance.CompletionTime.Should().Be(completionTime);
        eventInstance.Message.Should().Be($"Request {requestId} updated account {accountId} with amount {amount} at {completionTime:yyyy-MM-dd HH:mm:ss} (UTC).");
    }
}