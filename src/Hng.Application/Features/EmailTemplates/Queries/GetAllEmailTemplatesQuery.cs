using Hng.Application.Features.EmailTemplates.DTOs;
using MediatR;

namespace Hng.Application.Features.EmailTemplates.Queries;

public class GetAllEmailTemplatesQuery : IRequest<IEnumerable<EmailTemplateDTO>>;
