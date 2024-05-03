using API.Features.Domain;
using FluentAssertions;

namespace UnitTests
{
    [TestFixture]
    public class TransactionCreatedEventTests
    {
        [Test]
        public void TransactionCreatedEvent_ConstructedProperly_ShouldSetPropertiesCorrectlyAndGenerateCorrectMessage()
        {
            // Arrange
            var requestId = Guid.NewGuid();
            var transactionId = Guid.NewGuid().ToString();
            var accountId = "1234567890";
            var completionTime = DateTime.UtcNow;
            var amount = 100;

            // Create the event
            var eventInstance = new TransactionCreatedEvent(requestId, transactionId, accountId, amount, completionTime);

            // Assert
            eventInstance.RequestId.Should().Be(requestId);
            eventInstance.TransactionId.Should().Be(transactionId);
            eventInstance.AccountId.Should().Be(accountId);
            eventInstance.CompletionTime.Should().Be(completionTime);

            // Check the message format
            var expectedMessage = $"Request {requestId} Created transaction {transactionId} for account {accountId} at {completionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
            eventInstance.Message.Should().Be(expectedMessage);
        }
    }
}