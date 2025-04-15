using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wego.Core.Services.Contract;
using Stripe;
using Wego.Core.Models;
using Microsoft.Extensions.Logging;
namespace Wego.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly string _secretKey;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger)
        {
            _logger = logger;
            _secretKey = configuration["StripeSettings:SecretKey"];
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(decimal amount, string currency)
        {
            try
            {
                if (amount <= 0)
                    throw new ArgumentException("Amount must be greater than zero.");

                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100), // Convert to smallest unit
                    Currency = currency.ToLower(),
                    PaymentMethodTypes = new List<string> { "card" },
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                bool isSuccessful = paymentIntent.Status != null;

                return new PaymentResponse
                {
                    Status = isSuccessful,
                    SecritKey = paymentIntent.ClientSecret, // Fixed typo
                    PaymentId = paymentIntent.Id,
                    Amount = amount,
                    Message = isSuccessful ? "Payment successful" : "Payment requires further action"
                };
            }
            catch (StripeException ex)
            {
                _logger.LogError($"Stripe Error: {ex.Message}");
                return new PaymentResponse
                {
                    Status = false,
                    Message = $"Stripe Error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error: {ex.Message}");
                return new PaymentResponse
                {
                    Status = false,
                    Message = $"Server Error: {ex.Message}"
                };
            }
        }
    }

}
