namespace Hng.Application.Dto
{
    public class JobListingDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Salary { get; set; }
        public string JobType { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
