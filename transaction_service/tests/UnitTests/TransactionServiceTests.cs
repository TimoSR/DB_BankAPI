using System.Net;
using API.Features.Application;
using API.Features.Domain;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;

namespace UnitTests;

[TestFixture]
public class TransactionServiceTests
{
    private Mock<ITransactionRepository> _repositoryMock;
    private Mock<ITransactionFactory> _factoryMock;
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private Mock<ILogger<TransactionService>> _loggerMock;
    private TransactionService _service;
    private MockHttpMessageHandler _mockHttpHandler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<ITransactionRepository>();
        _factoryMock = new Mock<ITransactionFactory>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _loggerMock = new Mock<ILogger<TransactionService>>();
        _mockHttpHandler = new MockHttpMessageHandler();
        var client = new HttpClient(_mockHttpHandler);
        _httpClientFactoryMock.Setup(f => f.CreateClient("AccountServiceClient")).Returns(client);
        _service = new TransactionService(_repositoryMock.Object, _factoryMock.Object, _httpClientFactoryMock.Object, _loggerMock.Object);
    }
    
    [Test]
    public async Task GetAllTransactionsAsync_ShouldReturnTransactionDTOs_WhenTransactionsAreAvailable()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction("1", "123", 100m),
            new Transaction("2", "456", 200m)
        };
        _repositoryMock.Setup(r => r.GetTransactionsAsync()).ReturnsAsync(transactions);

        // Act
        var result = await _service.GetAllTransactionsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Id.Should().Be("1");
        result.Data[1].Amount.Should().Be(200m);
    }

    [Test]
    public async Task GetTransactionByIdAsync_ShouldReturnTransactionDTO_WhenTransactionIsFound()
    {
        // Arrange
        var transaction = new Transaction("1", "123", 100m);
        _repositoryMock.Setup(r => r.GetTransactionAsync("1")).ReturnsAsync(transaction);

        // Act
        var result = await _service.GetTransactionByIdAsync("1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Id.Should().Be("1");
    }
    
    
    /*[Test]
    public async Task CreateTransactionAsync_WhenHttpClientReturnsSuccess_ShouldCreateTransaction()
    {
        // Arrange
        var command = new CreateTransactionCommand { Id = Guid.NewGuid(), AccountId = "123", Amount = 100m };
        _mockHttpHandler.When(HttpMethod.Get, $"http://example.com/Account/GetById?id={command.AccountId}")
            .Respond(HttpStatusCode.OK);
        var transaction = new Transaction(command.Id.ToString(), command.AccountId, command.Amount);
        _factoryMock.Setup(f => f.CreateTransaction(command.Id, command.AccountId, command.Amount)).Returns(transaction);
        _repositoryMock.Setup(r => r.AddTransactionAsync(command.Id, transaction)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateTransactionAsync(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Messages.Should().Contain("Transaction " + transaction.Id + " Created Successfully!");
        _loggerMock.Verify(log => log.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        //_repositoryMock.Verify(r => r.AddTransactionAsync(transaction.Id, transaction), Times.Once);
    }*/
    
    [Test]
    public async Task GetLast10AccountTransactionsAsync_ShouldReturnTransactions_WhenTransactionsAreAvailable()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction("1", "123", 50m),
            new Transaction("2", "123", 150m)
        };
        _repositoryMock.Setup(r => r.GetLast10AccountTransactionsAsync("123")).ReturnsAsync(transactions);

        // Act
        var result = await _service.GetLast10AccountTransactionsAsync("123");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].Amount.Should().Be(50m);
        result.Data[1].Amount.Should().Be(150m);
    }
    
    [TearDown]
    public void TearDown()
    {
        _mockHttpHandler.Dispose();  // Dispose of the MockHttpMessageHandler
    }
}