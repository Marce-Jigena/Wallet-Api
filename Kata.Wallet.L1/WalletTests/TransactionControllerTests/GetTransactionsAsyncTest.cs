using Moq;
using Kata.Wallet.Services;
using Microsoft.AspNetCore.Mvc;
using Kata.Wallet.Api.Controllers;
using Kata.Wallet.Dtos;

namespace Kata.Wallet.Tests
{
    [TestClass]
    public class TransactionControllerTests
    {
        private Mock<ITransactionService> _mockTransactionService;
        private TransactionController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Inicializa el Mock de ITransactionService
            _mockTransactionService = new Mock<ITransactionService>();

            // Crea el controlador e inyecta el mock
            _controller = new TransactionController(_mockTransactionService.Object);
        }

        [TestMethod]
        public async Task GetTransactionsAsync_Returns_Transactions()
        {
            // Arrange
            int walletId = 1;
            var transactions = new List<TransactionDto>
            {
                new TransactionDto { Id = 1, Amount = 200, Date = new DateTime(2025, 2, 9), Description = "Test", WalletIncomingId = 2, WalletOutgoingId = 1 },
                new TransactionDto { Id = 2, Amount = 300, Date = new DateTime(2025, 2, 10), Description = "Test", WalletIncomingId = 3, WalletOutgoingId = 1 }
            };

            _mockTransactionService.Setup(service => service.GetTransactionsAsync(walletId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _controller.GetTransactionsAsync(walletId);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedTransactions = okResult.Value as List<TransactionDto>;
            Assert.IsNotNull(returnedTransactions);
            Assert.AreEqual(transactions.Count, returnedTransactions.Count);
        }

        [TestMethod]
        public async Task GetTransactionsAsync_Returns_BadRequest_If_No_Transactions()
        {
            // Arrange
            int walletId = 1;

            _mockTransactionService.Setup(service => service.GetTransactionsAsync(walletId))
                .ReturnsAsync((List<TransactionDto>)null);

            // Act
            var result = await _controller.GetTransactionsAsync(walletId);

            // Assert
            var badRequestResult = result.Result as BadRequestResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }
    }
}
