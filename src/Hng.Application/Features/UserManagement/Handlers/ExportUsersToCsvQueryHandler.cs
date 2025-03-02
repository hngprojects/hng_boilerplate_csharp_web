using AutoMapper;
using CsvHelper;
using Hng.Application.Features.UserManagement.Dtos;
using Hng.Application.Features.UserManagement.Queries;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Features.UserManagement.Handlers
{
    public class ExportUsersToCsvQueryHandler : IRequestHandler<ExportUsersToCsvQuery, byte[]>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ExportUsersToCsvQueryHandler(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<byte[]> Handle(ExportUsersToCsvQuery request, CancellationToken cancellationToken)
        {
            // Fetch all users
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            // Map to a simplified object for CSV export
            var csvData = userDtos.Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email
            });

            // Generate CSV
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(userDtos);
            await streamWriter.FlushAsync();

            return memoryStream.ToArray();
        }
    }
}