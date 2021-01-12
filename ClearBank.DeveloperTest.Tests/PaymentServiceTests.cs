using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ClearBank.DeveloperTest.Tests
{
    [TestClass]
    public  class PaymentServiceTests
    {
        #region Mock Services

        private readonly Mock<IAccountDataStore> _mockAccountDataStore;

        private readonly Mock<IBackupAccountDataStore> _mockBackupAccountDataStore;

        private  IConfiguration _configuration;

        private MakePaymentResult _makePaymentResult;

        #endregion

        #region Construtor

        public PaymentServiceTests()
        {
            _mockAccountDataStore = new Mock<IAccountDataStore>(MockBehavior.Strict);
            _mockBackupAccountDataStore = new Mock<IBackupAccountDataStore>(MockBehavior.Strict);
            _makePaymentResult = new MakePaymentResult();

        }

        #endregion


        #region Tests

        [TestMethod]
        public void BackupAccountWithPaymentAsBacsAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Bacs };

            // Act
           var makePaymentResult =  paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }

        [TestMethod]
        public void BackupAccountWithPaymentAsBacsAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs};
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockBackupAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Bacs };
            _makePaymentResult.Success = true;

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }

        [TestMethod]
        public void NonBackupAccountWithPaymentAsBacsAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Bacs };

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }


        [TestMethod]
        public void NonBackupAccountWithPaymentAsBacsAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs, Balance = 100 };
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Bacs };
            _makePaymentResult.Success = true;
            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }



        [TestMethod]
        public void NonBackupAccountWithPaymentAsFasterPaymentAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments };

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }

        [TestMethod]
        public void NonBackupAccountWithPaymentAsFasterPaymentAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = 100 };
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments };
            _makePaymentResult.Success = true;

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }

        [TestMethod]
        public void BackupAccountWithPaymentAsFasterPaymentAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments };

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }

        [TestMethod]
        public void BackupAccountWithPaymentAsFasterPaymentAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments };
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockBackupAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.FasterPayments };
            _makePaymentResult.Success = true;

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }

        [TestMethod]
        public void BackupAccountWithPaymentAsChapsAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Chaps };

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }

        [TestMethod]
        public void BackupAccountWithPaymentAsChapsAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps };
            SetMockConfigValueAsBackup();
            _mockBackupAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockBackupAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Chaps };
            _makePaymentResult.Success = true;

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockBackupAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockBackupAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }

        [TestMethod]
        public void NonBackupAccountWithPaymentAsChapsAndAccountAsNull()
        {
            // Arrange
            Account account = null;
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Chaps };

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == false);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Never);

        }

        [TestMethod]
        public void NonBackupAccountWithPaymentAsChapsAndValidAccount()
        {
            // Arrange
            Account account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Balance = 100 };
            SetMockConfigValueAsNonBackup();
            _mockAccountDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(account);
            _mockAccountDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>()));
            var paymentService = GetPaymentService();
            MakePaymentRequest makePaymentRequest = new MakePaymentRequest() { PaymentScheme = PaymentScheme.Chaps };
            _makePaymentResult.Success = true;

            // Act
            var makePaymentResult = paymentService.MakePayment(makePaymentRequest);

            //Assert

            Assert.IsTrue(makePaymentResult.Success == true);
            _mockAccountDataStore.Verify(s => s.GetAccount(It.IsAny<string>()), Times.Once);
            _mockAccountDataStore.Verify(s => s.UpdateAccount(It.IsAny<Account>()), Times.Once);

        }



        #endregion

        private PaymentService GetPaymentService()
        {

            var service = new PaymentService(
                _mockAccountDataStore.Object,
                _mockBackupAccountDataStore.Object,
                _configuration,
               _makePaymentResult);

            return service;
        }

        private void SetMockConfigValueAsBackup()
        {
            var dict = new Dictionary<string, string> {
                     {"DataStoreType", "Backup"}
                };

             _configuration = new ConfigurationBuilder()
              .AddInMemoryCollection(dict)
            .Build();
        }

        private void SetMockConfigValueAsNonBackup()
        {
            var dict = new Dictionary<string, string> {
                     {"DataStoreType", "NonBackup"}
                };

            _configuration = new ConfigurationBuilder()
             .AddInMemoryCollection(dict)
           .Build();
        }

    }
}
