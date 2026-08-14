using AutoMapper;
using Hng.Application.Features.NewsLetterSubscription.Dtos;
using Hng.Application.Features.NewsLetterSubscription.Queries;
using Hng.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.NewsLetterSubscription.Handlers
{
    public class GetPaginatedSubscribersHandler : IRequestHandler<GetPaginatedSubscribersQuery, PaginatedSubscribersResponseDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaginatedSubscribersHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedSubscribersResponseDto> Handle(GetPaginatedSubscribersQuery request, CancellationToken cancellationToken)
        {
            if (request.Page <= 0 || request.Limit <= 0)
            {
                return new PaginatedSubscribersResponseDto
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Invalid pagination parameters"
                };
            }

            var totalSubscribers = await _context.NewsLetterSubscribers.CountAsync(s => s.LeftOn == null, cancellationToken);
            var subscribers = await _context.NewsLetterSubscribers
                .Where(s => s.LeftOn == null)
                .OrderByDescending(s => s.CreatedAt)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            if (!subscribers.Any())
            {
                return new PaginatedSubscribersResponseDto
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "No subscribers found"
                };
            }

            return new PaginatedSubscribersResponseDto
            {
                Success = true,
                StatusCode = 200,
                Message = "Subscribers retrieved successfully",
                Data = _mapper.Map<IEnumerable<NewsLetterSubscriptionDto>>(subscribers), // Keep Guid as Id
                Pagination = new PaginationMetadata
                {
                    Page = request.Page,
                    Limit = request.Limit,
                    Total = totalSubscribers
                }
            };
        }
    }
}

