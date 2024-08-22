using System.Text;
using Hng.Infrastructure.EmailTemplates;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities.StringKeys;
using Microsoft.Extensions.Logging;
namespace Hng.Infrastructure.Services;

public class EmailTemplateService(TemplateDir templateDir, ILogger<EmailTemplateService> logger) : IEmailTemplateService
{
    private readonly TemplateDir templateDir = templateDir;
    private readonly ILogger<EmailTemplateService> logger = logger;

    public async Task<string> GetOrganizationInviteTemplate()
    {
        logger.LogInformation("Getting organisation invite email template");
        string path = templateDir.Path;
        path = Path.Combine(path, $"{EmailConstants.inviteEmailTemplate}");
        string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
        return template;
    }

    public async Task<string> GetForgotPasswordEmailTemplate()
    {
        logger.LogInformation("Getting forgot password template");
        string path = templateDir.Path;
        path = Path.Combine(path, $"{EmailConstants.ForgotPasswordTemplate}");
        string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
        return template;
    }

    public async Task<string> GetForgotPasswordMobileEmailTemplate()
    {
        logger.LogInformation("Getting forgot password template");
        string path = templateDir.Path;
        path = Path.Combine(path, $"{EmailConstants.ForgotPasswordMobileTemplate}");
        string template = await File.ReadAllTextAsync(path, Encoding.UTF8);
        return template;
    }
}

