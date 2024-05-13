using API.Features.Domain;
using FluentAssertions;

namespace UnitTests;

[TestFixture]
public class AccountFactoryTests
{
    private AccountFactory _accountFactory;

    [SetUp]
    public void SetUp()
    {
        _accountFactory = new AccountFactory();
    }

    [Test]
    public void CreateAccount_ValidParameters_ShouldCreateAndInitializeAccount()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        var cpr = "1234567890";
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var account = _accountFactory.CreateAccount(commandId, cpr, firstName, lastName);

        // Assert
        account.Should().NotBeNull();
        account.ID.Should().NotBeNullOrEmpty();
        account.CPR.Should().Be(cpr);
        account.FirstName.Should().Be(firstName);
        account.LastName.Should().Be(lastName);
        account.DomainEvents.Should().ContainSingle();
        var domainEvent = (AccountCreatedEvent)account.DomainEvents[0];
        domainEvent.Should().NotBeNull();
        domainEvent.CommandId.Should().Be(commandId);
        domainEvent.AccountId.Should().Be(account.ID);
    }

    [Test]
    public void CreateAccount_InvalidParameters_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var commandId = Guid.NewGuid();
        string cpr = null; // Invalid CPR
        var firstName = "John";
        var lastName = "Doe";

        // Act
        Action act = () => _accountFactory.CreateAccount(commandId, cpr, firstName, lastName);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot create account because one or more required properties are not set.");
    }
}