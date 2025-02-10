using Moq;
using Kata.Wallet.Services;
using Kata.Wallet.API.Controllers;
using Kata.Wallet.Domain;
using Microsoft.AspNetCore.Mvc;

namespace WalletTests.WalletControllerTests;

[TestClass]
public class NewTransferTest
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
    public async Task NewTransfer_ValidWallets_ReturnsOk()
    {
        // Arrange
        int originWalletId = 1;
        int destinationWalletId = 2;
        decimal amount = 100;
        string description = "Test";

        _mockWalletService.Setup(service => service.NewTransfer(originWalletId, destinationWalletId, amount, description)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.NewTransfer(originWalletId, destinationWalletId, amount, description);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkResult)); // Esperamos un OkResult
    }

    [TestMethod]
    public async Task NewTransfer_DifferentCurrency_ReturnsBadRequest()
    {
        // Arrange
        int originWalletId = 1;
        int destinationWalletId = 2;
        decimal amount = 100;
        string description = "Transfer test";
        var exception = new DifferentCurrencyException(409, "Currency", "La moneda de ambas cuentas debe ser la misma.");

        _mockWalletService.Setup(service => service.NewTransfer(originWalletId, destinationWalletId, amount, description))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.NewTransfer(originWalletId, destinationWalletId, amount, description);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task NewTransfer_NotEnoughFunds_ReturnsBadRequest()
    {
        // Arrange
        int originWalletId = 1;
        int destinationWalletId = 2;
        decimal amount = 100;
        string description = "Test";
        var exception = new NotEnoughFundsException(409, "Wallet", "No hay suficientes fondos para realizar la transaccion");

        _mockWalletService.Setup(service => service.NewTransfer(originWalletId, destinationWalletId, amount, description))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.NewTransfer(originWalletId, destinationWalletId, amount, description);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task NewTransfer_WalletNotFound_ReturnsBadRequest()
    {
        // Arrange
        int originWalletId = 1;
        int destinationWalletId = 2;
        decimal amount = 100;
        string description = "Test";
        var exception = new KeyNotFoundException("Una o mas wallets no fueron encontradas.");

        _mockWalletService.Setup(service => service.NewTransfer(originWalletId, destinationWalletId, amount, description))
            .ThrowsAsync(exception);

        // Act
        var result = await _controller.NewTransfer(originWalletId, destinationWalletId, amount, description);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
}
