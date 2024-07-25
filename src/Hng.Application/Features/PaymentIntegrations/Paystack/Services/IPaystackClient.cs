using CSharpFunctionalExtensions;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Common;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Requests;
using Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Services
{
    public interface IPaystackClient
    {
        Task<Result<VerifyTransactionResponse>> VerifyTransaction(VerifyTransactionRequest request);
        Task<Result<VerifyTransferResponse>> VerifyTransfer(VerifyTransactionRequest request);
        Task<Result<InitializeTransactionResponse>> InitializeTransaction(InitializeTransactionRequest request);
    }
}