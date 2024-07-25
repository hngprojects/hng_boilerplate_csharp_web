using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.PaymentIntegrations.Paystack.Dtos.Common
{
    public class ErrorResponse
    {
        public bool Status { get; set; }
        public string Error { get; set; }
    }
}
