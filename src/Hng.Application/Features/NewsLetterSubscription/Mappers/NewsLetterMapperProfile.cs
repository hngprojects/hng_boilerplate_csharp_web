using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Hng.Application.Features.NewsLetterSubscription.Mappers
{
    public class NewsLetterMapperProfile : Profile
    {
        public NewsLetterMapperProfile()
        {
            CreateMap<NewsLetterSubscriber, NewsLetterSubscriptionDto>()
                .ReverseMap();
        }

    }
}