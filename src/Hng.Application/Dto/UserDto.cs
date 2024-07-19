namespace Hng.Application.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ProfileDto Profile { get; set; }
        public List<OrganisationDto> Organisations { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}