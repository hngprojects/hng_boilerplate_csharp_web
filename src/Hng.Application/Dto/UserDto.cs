namespace Hng.Application.Dto
{
    public class UserDto
    {
        public string name { get; set; }
        public Guid id { get; set; }
        public string email { get; set; }
        public ProfileDto profile { get; set; }
        public List<OrganisationDto> organisations { get; set; }
        public List<ProductDto> products { get; set; }
    }
}