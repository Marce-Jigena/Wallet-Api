using Moq;
using Kata.Wallet.Services;
using Kata.Wallet.API.Controllers;
using Kata.Wallet.Domain;
using Kata.Wallet.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WalletTests.WalletControllerTests;

[TestClass]
public class GetAllTest
{
    private Mock<IWalletService> _mockWalletService;
    private WalletController _controller;

    [TestInitialize]
    public void Setup()
    {
        // Inicializa los Mocks de las dependencias
        _mockWalletService = new Mock<IWalletService>();

        // Inicializa el controlador y pasa el mock del servicio
        _controller = new WalletController(_mockWalletService.Object);
    }


    [TestMethod]
    public async Task GetAll_Returns_All_Wallets()
    {
        // Arrange
        var wallets = new List<WalletDto>
            {
                new WalletDto { Id = 1, Balance = 1000, UserDocument = "12345", UserName = "Juan Carlos", Currency = Currency.USD, IncomingTransactions = new List<TransactionDto>
                    { new TransactionDto { Id = 1, Amount = 200, Date = new DateTime(2025, 2, 9), Description = null, WalletIncomingId = 2, WalletOutgoingId = 0 } }, OutgoingTransactions = null },
                new WalletDto { Id = 2,  Balance = 2000, UserDocument = "67890", UserName = "Marcos", Currency = Currency.USD, IncomingTransactions = null, OutgoingTransactions = new List<TransactionDto>
                    { new TransactionDto { Id = 2, Amount = 200, Date = new DateTime(2025, 2, 9), Description = null, WalletIncomingId = 0, WalletOutgoingId = 1 } } }
            };

        _mockWalletService.Setup(s => s.GetAllAsync(null, null)).ReturnsAsync(wallets);

        //Act
        var result = await _controller.GetAll(null, null);

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);

        var walletsReturned = okResult.Value as List<WalletDto>;
        Assert.AreEqual(wallets.Count(), walletsReturned.Count());
        Assert.AreEqual(wallets, walletsReturned);
    }

    [TestMethod]
    public async Task GetAll_Returns_Filtered_Wallets()
    {
        // Arrange
        var currency = Currency.USD;
        var userDocument = "12345";
        var wallets = new List<WalletDto>
            {
                new WalletDto { Id = 1, Balance = 1000, UserDocument = "12345", UserName = "Juan Carlos", Currency = Currency.USD, IncomingTransactions = new List<TransactionDto>
                    { new TransactionDto { Id = 1, Amount = 200, Date = new DateTime(2025, 2, 9), Description = null, WalletIncomingId = 2, WalletOutgoingId = 0 } }, OutgoingTransactions = null },
                new WalletDto { Id = 2,  Balance = 2000, UserDocument = "67890", UserName = "Marcos", Currency = Currency.USD, IncomingTransactions = null, OutgoingTransactions = new List<TransactionDto>
                    { new TransactionDto { Id = 2, Amount = 200, Date = new DateTime(2025, 2, 9), Description = null, WalletIncomingId = 0, WalletOutgoingId = 1 } } }
            };

        _mockWalletService.Setup(service => service.GetAllAsync(currency, userDocument))
            .ReturnsAsync(wallets.FindAll(w => w.Currency == currency && w.UserDocument == userDocument));

        var walletsFiltered = wallets.Where(w => w.Currency == currency && w.UserDocument == userDocument).ToList();

        //Act
        var result = await _controller.GetAll(currency, userDocument);

        //Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));

        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);

        //Verifica que el valor retornado sea igual al esperado.
        var walletsReturned = okResult.Value as List<WalletDto>;
        Assert.AreEqual(walletsFiltered.Count(), walletsReturned.Count());
        Assert.AreEqual(walletsFiltered[0], walletsReturned[0]);
    }
}