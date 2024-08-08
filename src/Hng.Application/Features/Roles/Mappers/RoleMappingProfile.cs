using Hng.Application.Features.Roles.Command;
using Hng.Application.Features.Roles.Dto;
using Hng.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.Roles.Mappers
{
    public class RoleMappingProfile : AutoMapper.Profile
    {
        public RoleMappingProfile()
        {
            // Mapping from Role to CreateRoleResponseDto
            CreateMap<Role, CreateRoleResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();

            // Mapping from CreateRoleCommand to Role
            CreateMap<CreateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleRequestBody.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RoleRequestBody.Description))
                .ReverseMap();

            // Mapping from Role to RoleDetailsDto
            CreateMap<Role, RoleDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions))
                .ReverseMap();

            // Mapping from Role to RoleDto
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();


            CreateMap<UpdateRoleCommand, Role>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UPTRoleRequest.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.UPTRoleRequest.Description))
                .ReverseMap();

            // Mapping from Role to UpdateRoleResponse
            CreateMap<Role, UpdateRoleResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();
        }
    }
}
