using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wego.Core.Models
{
    public class PaymentResponse
    {
        public bool Status { get; set; }
        public string PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string SecritKey { get; set; }
        public string Message { get; set; }
    }
}
