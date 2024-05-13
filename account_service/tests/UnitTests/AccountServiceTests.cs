using API.Features.Application;
using API.Features.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests;

[TestFixture]
public class AccountServiceTests
{
    private AccountService _accountService;
    private Mock<IAccountRepository> _accountRepositoryMock;
    private Mock<IAccountFactory> _factoryMock;
    private Mock<IAccountSecurityDomainService> _securityMock;
    private Mock<ILogger<AccountService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _factoryMock = new Mock<IAccountFactory>();
        _securityMock = new Mock<IAccountSecurityDomainService>();
        _loggerMock = new Mock<ILogger<AccountService>>();
        _accountService = new AccountService(_accountRepositoryMock.Object, _factoryMock.Object, _securityMock.Object,
            _loggerMock.Object);
    }
    
    [Test]
    public async Task GetAllAccountsAsync_WhenAccountsExist_ShouldReturnAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account { ID = "1", FirstName = "John", LastName = "Doe" },
            new Account { ID = "2", FirstName = "Jane", LastName = "Smith" }
        };
        _accountRepositoryMock.Setup(x => x.GetAccountsAsync()).ReturnsAsync(accounts);

        // Act
        var result = await _accountService.GetAllAccountsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].FirstName.Should().Be("John");
    }
    
    [Test]
    public async Task GetAccountByIdAsync_WhenAccountExists_ShouldReturnAccount()
    {
        // Arrange
        var account = new Account { ID = "1", FirstName = "John", LastName = "Doe" };
        _accountRepositoryMock.Setup(x => x.GetAccountAsync("1")).ReturnsAsync(account);

        // Act
        var result = await _accountService.GetAccountByIdAsync("1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.FirstName.Should().Be("John");
        result.Data.LastName.Should().Be("Doe");
    }

    // [Test]
    // public async Task CreateAccountAsync_WhenCalled_ShouldCreateAccount()
    // {
    //     // Arrange
    //     var command = new CreateAccountCommand{Id = Guid.NewGuid(), CPR = "1234567890", FirstName = "John", LastName = "Doe"};
    //     var account = new Account { ID = command.Id.ToString(), FirstName = "John", LastName = "Doe"};
    //     _securityMock.Setup(x => x.Hash("1234567890")).Returns("hashedCPR");
    //     _factoryMock.Setup(x => x.CreateAccount(command.Id, "hashedCPR", "John", "Doe")).Returns(account);
    //     _accountRepositoryMock.Setup(x => x.CreateAccountAsync(command.Id, account)).Returns(Task.CompletedTask);
    //
    //     // Act
    //     var result = await _accountService.CreateAccountAsync(command);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     _loggerMock.Verify(x => x.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
    // }
    
    [TearDown]
    public void TearDown()
    {
        _accountRepositoryMock.VerifyAll();
        _factoryMock.VerifyAll();
        _securityMock.VerifyAll();
        _loggerMock.VerifyAll();
    }
}