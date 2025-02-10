using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kata.Wallet.Services;

namespace Kata.Wallet.Tests

[TestClass]
public class GetAllWalletControllerTest
{
    [TestMethod]
    public async Task GetAllWalletControllerTest()
    {
        private Mock<IWalletService> _mockWalletService;
        private WalletController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Inicializa el Mock de IWalletService
            _mockWalletService = new Mock<IWalletService>();

            // Crea el controlador e inyecta el mock
            _controller = new WalletController(_mockWalletService.Object);
        }
    }
}