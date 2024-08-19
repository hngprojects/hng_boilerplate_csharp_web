namespace Hng.Infrastructure.Services.Interfaces;

public interface IEmailTemplateService
{
    public Task<string> GetOrganizationInviteTemplate();

    public Task<string> GetForgotPasswordEmailTemplate();

    Task<string> GetForgotPasswordMobileEmailTemplate();
}
