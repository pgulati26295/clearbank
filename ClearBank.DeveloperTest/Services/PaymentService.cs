using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IBackupAccountDataStore _backupAccountDataStore;
        private readonly IConfiguration _configuration;
        private readonly MakePaymentResult _makePaymentResult;

        public PaymentService(IAccountDataStore accountDataStore, IBackupAccountDataStore backupAccountDataStore, IConfiguration configuration , MakePaymentResult makePaymentResult)
        {
            _accountDataStore = accountDataStore;
            _backupAccountDataStore = backupAccountDataStore;
            _configuration = configuration;
            _makePaymentResult = makePaymentResult;
        }
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var dataStoreType = _configuration.GetValue<string>("DataStoreType");
            bool isBackup = string.Equals(dataStoreType, "Backup", StringComparison.OrdinalIgnoreCase);

            Account account = null;

            if (isBackup)
            {
          
                account = _backupAccountDataStore.GetAccount(request.DebtorAccountNumber);
            }
            else
            {
              
                account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            }

            var result = IsValidAccount(account, request);

            if (result.Success)
            {
                account.Balance -= request.Amount;

                if (isBackup)
                {
                    _backupAccountDataStore.UpdateAccount(account);
                }
                else
                {

                    _accountDataStore.UpdateAccount(account);
                }
            }

            return result;
        }

        private MakePaymentResult IsValidAccount(Account account, MakePaymentRequest request)
        {
           
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        _makePaymentResult.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        _makePaymentResult.Success = false;
                    }
                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        _makePaymentResult.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        _makePaymentResult.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        _makePaymentResult.Success = false;
                    }
                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        _makePaymentResult.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        _makePaymentResult.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        _makePaymentResult.Success = false;
                    }
                    break;
            }

            return _makePaymentResult;
        }
    }
}
