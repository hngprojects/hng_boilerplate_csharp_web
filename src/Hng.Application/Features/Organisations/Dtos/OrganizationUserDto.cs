using Hng.Domain.Entities;
using System.Text.Json.Serialization;

namespace Hng.Application.Features.Organisations.Dtos
{
    public class OrganizationUserDto
    {
        public ICollection<User> Users { get; set; }
    }
}
