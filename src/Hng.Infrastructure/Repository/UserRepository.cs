using AutoMapper;
using Hng.Application.Dto;
using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Hng.Infrastructure.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hng.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly MyDBContext myDBContext;
        private readonly IMapper mapper;

        public UserRepository(MyDBContext myDBContext, IMapper mapper) : base(myDBContext)
        {
            this.myDBContext = myDBContext;
            this.mapper = mapper;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var data = await myDBContext.Users.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return null;
            }

            var profile = await myDBContext.Profiles.FirstOrDefaultAsync(x => x.UserId == id);
            var organisationUsers = await myDBContext.OrganisationUsers
            .Include(x => x.Organisation)
            .Where(x => x.UserId == data.Id).ToListAsync();

            var userOrgList = new List<OrganisationDto>();
            foreach (var organisationUser in organisationUsers)
            {
                userOrgList.Add(new OrganisationDto
                {
                    //Org_id = organisationUser.Organisation.Id,
                    Name = organisationUser.Organisation.Name,
                    Description = organisationUser.Organisation.Description

                });
            }
            return null;

            //return new UserForReturnDto
            //{
            //    Name = $"{data.FirstName} {data.LastName}",
            //    Id = data.Id.ToString(),
            //    Email = data.Email,
            //    Profile = mapper.Map<ProfileForReturn>(profile),
            //    Organisations = userOrgList,
            //    Products = mapper.Map<List<ProductDto>>(data.Products)
            //};
        }
    }
}