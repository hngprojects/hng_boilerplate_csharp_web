using AutoMapper;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Application.Test.Features.HelpCenter
{
    public class HelpCenterTopicsTestsSetup
    {
        protected readonly Mock<IRepository<HelpCenterTopic>> _repositoryMock;
        protected readonly Mock<IMapper> _mapperMock;
        protected readonly Mock<IMediator> _mediatorMock;

        public HelpCenterTopicsTestsSetup()
        {
            _repositoryMock = new Mock<IRepository<HelpCenterTopic>>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();
        }
    }

}
