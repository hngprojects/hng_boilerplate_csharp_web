using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hng.Domain.Models;
using Hng.Infrastructure.Context;
using Hng.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hng.Web.Controllers
{
    [ApiController]
    [Route("api/v1/user/")]
    public class UserController : ControllerBase
    {
        private readonly MyDBContext myDBContext;
        private readonly IMapper mapper;

        public UserController(MyDBContext myDBContext, IMapper mapper)
        {
            this.myDBContext = myDBContext;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var data = await myDBContext.Users
            .Include(x=> x.Products)
            .FirstOrDefaultAsync(x=> x.Id == id);

            if (data == null)
            {
                return NotFound(new {
                    Message = "user not found"
                });
            }

            var organisationUsers = await myDBContext.OrganisationUsers
            .Include(x=> x.Organisation)
            .Where(x=> x.UserId == data.Id).ToListAsync();

            var userOrgList = new List<OrganisationDto>();
            foreach (var organisationUser in organisationUsers)
            {
                userOrgList.Add(new OrganisationDto
                {
                    Org_id = organisationUser.Organisation.OrgId,
                    Name = organisationUser.Organisation.Name,
                    Description = organisationUser.Organisation.Description

                });
            }

            return Ok(new {
                name = $"{data.FirstName} {data.LastName}",
                id = data.Id,
                email = data.Email,
                profile = new {
                    first_name = data.FirstName,
                    last_name = data.LastName,
                    phone = data.PhoneNumber,
                    avatar_url = data.AvatarUrl
                },
                organisations = userOrgList,
                products = mapper.Map<List<ProductDto>>(data.Products)
            });
        }
        
    }
}