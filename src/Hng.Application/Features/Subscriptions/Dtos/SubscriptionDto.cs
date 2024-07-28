using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hng.Domain.Enums;

namespace Hng.Application.Features.Subscriptions.Dtos
{
	public class SubscriptionDto
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }
		[JsonPropertyName("userId")]
		public Guid? UserId { get; set; }
		[JsonPropertyName("organizationId")]
		public Guid? OrganizationId { get; set; }
		[JsonPropertyName("plan")]
		public SubscriptionPlan Plan { get; set; }
		[JsonPropertyName("frequency")]
		public SubscriptionFrequency Frequency { get; set; }
		[JsonPropertyName("isActive")]
		public bool IsActive { get; set; }
		[JsonPropertyName("amount")]
		public decimal Amount { get; set; }
		[JsonPropertyName("startDate")]
		public DateTime? StartDate { get; set; }
	}
}
