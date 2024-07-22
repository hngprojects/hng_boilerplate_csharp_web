namespace Hng.Application.Dto
{
    public class UserDto
    {
        public string name { get; set; }
        public Guid id { get; set; }
        public string email { get; set; }
        public ProfileDto profile { get; set; }
        public IEnumerable<OrganizationDto> organizations { get; set; }
        public IEnumerable<ProductDto> products { get; set; }
    }
}
