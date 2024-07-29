using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Subscriptions.Dtos.Responses
{
    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        //public Guid UserId { get; set; }
        //public Guid OrganizationId { get; set; }
        public Guid TransactionId { get; set; }
        public string Plan { get; set; }
        public string Frequency { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
