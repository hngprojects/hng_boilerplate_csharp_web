using System.Text;
using Hng.Infrastructure.EmailTemplates;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Hng.Infrastructure.Utilities.Errors.Emails;
using Microsoft.Extensions.Logging;
namespace Hng.Infrastructure.Services;

public class EmailTemplateService(TemplateDir templateDir, ILogger<EmailTemplateService> logger) : IEmailTemplateService
{
    private readonly TemplateDir templateDir = templateDir;
    private readonly ILogger<EmailTemplateService> logger = logger;

    public async Task<string> GetOrganizationInviteTemplate()
    {
        string path = templateDir.Path;
        path = Path.Combine(path, $"{EmailConstants.inviteEmailTemplate}");
        string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
        return template;
    }
}

