using Hng.Application.Features.EmailTemplates.DTOs;
using MediatR;

namespace Hng.Application.Features.EmailTemplates.Commands;

public record CreateEmailTemplateCommand(CreateEmailTemplateDTO TemplateDTO) : IRequest<EmailTemplateDTO>;

