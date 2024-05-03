using API.Features.Domain;
using FluentAssertions;

namespace UnitTests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void CreateTransaction_WhenCalled_SetsTimeAndRaisesTransactionCreatedEvent()
        {
            // Arrange
            var transaction = new Transaction
            {
                AccountId = "123",
                Amount = 100m
            };
            var requestId = Guid.NewGuid();

            // Act
            transaction.CreateTransaction(requestId);

            // Assert
            transaction.Time.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            transaction.DomainEvents.Should().ContainSingle();
        }
    }
}