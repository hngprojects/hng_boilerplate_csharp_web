using Hng.Infrastructure.Utilities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IEmailTemplateService
{
    public Task<string> GetOrganizationInviteTemplate();
}
