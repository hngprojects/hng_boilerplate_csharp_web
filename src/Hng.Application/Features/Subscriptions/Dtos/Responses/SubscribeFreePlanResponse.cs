namespace Hng.Application.Features.Subscriptions.Dtos.Responses
{
    public record SubscribeFreePlanResponse
    {
        public Guid SubscriptionId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? OrganizationId { get; set; }

        public string Plan { get; set; }

        public string Frequency { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
