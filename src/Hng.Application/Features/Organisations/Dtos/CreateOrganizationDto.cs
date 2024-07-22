namespace Hng.Application.Features.Organisations.Dtos
{
    public class CreateOrganizationDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Email { get; set; }

        public string Industry { get; set; }

        public string Type { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }
        
        public string State { get; set; }
    }
}