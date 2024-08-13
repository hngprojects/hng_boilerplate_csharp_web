using AutoMapper;
using Hng.Application.Features.Jobs.Dtos;
using Hng.Application.Features.Jobs.Mappers;
using Hng.Domain.Enums;
using Xunit;

namespace Hng.Application.Test.Features.Job
{
    public class GetJobByIdQueryShould
    {
        private readonly IMapper _mapper;

        public GetJobByIdQueryShould()
        {
            var jobMappingProfile = new JobMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(jobMappingProfile));
            _mapper = new Mapper(configuration);
        }

        [Fact]
        public void Map_Job_To_JobDto()
        {
            // Arrange
            var expected = new Domain.Entities.Job
            {
                Id = Guid.NewGuid(),
                Title = "Test Job",
                Description = "Job Description",
                Location = "Test Location",
                Salary = 1500.00,
                Level = ExperienceLevel.Intermediate,
                Company = "Test Company",
                DatePosted = DateTime.UtcNow
            };
            // Act
            var result = _mapper.Map<JobDto>(expected);

            // Assert
            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Title, result.Title);
            Assert.Equal(expected.Description, result.Description);
            Assert.Equal(expected.Location, result.Location);
            Assert.Equal(expected.Salary, result.Salary);
            Assert.Equal(expected.Level, result.Level);
            Assert.Equal(expected.Company, result.Company);
            Assert.Equal(expected.DatePosted, result.DatePosted);
        }
    }
}
