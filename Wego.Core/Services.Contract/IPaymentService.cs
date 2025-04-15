using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Models;

namespace Wego.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(decimal amount, string currency);
    }
}
